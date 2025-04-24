using Microsoft.AspNetCore.Http;
using Moq;
using RAGNET.Application.DTOs;
using RAGNET.Application.UseCases.EmbeddingUseCases;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.ApiKey;

namespace tests.RAGNet.Application.Tests
{
    public class ProcessEmbeddingUseCaseTests
    {
        private readonly Mock<IWorkflowRepository> _workflowRepositoryMock;
        private readonly Mock<IDocumentProcessorFactory> _documentProcessorFactoryMock;
        private readonly Mock<IJobStatusRepository> _jobStatusRepositoryMock;
        private readonly Mock<IDocumentProcessingService> _pdfProcessingServiceMock;
        private readonly Mock<IEmbeddingProcessingService> _embeddingProcessingServiceMock;
        private readonly Mock<IApiKeyResolverService> _apiKeyResolverServiceMock;
        private readonly ProcessEmbeddingUseCase _useCase;
        private static readonly float[] singleArray = [0.1f, 0.2f, 0.3f];

        public ProcessEmbeddingUseCaseTests()
        {
            _workflowRepositoryMock = new Mock<IWorkflowRepository>();
            _documentProcessorFactoryMock = new Mock<IDocumentProcessorFactory>();
            _pdfProcessingServiceMock = new Mock<IDocumentProcessingService>();
            _embeddingProcessingServiceMock = new Mock<IEmbeddingProcessingService>();
            _apiKeyResolverServiceMock = new Mock<IApiKeyResolverService>();
            _jobStatusRepositoryMock = new Mock<IJobStatusRepository>();

            // Configures the factory to return the PDF adapter
            _documentProcessorFactoryMock
                .Setup(factory => factory.CreateDocumentProcessor(It.Is<string>(ext => ext.Equals(".pdf", StringComparison.OrdinalIgnoreCase))))
                .Returns(_pdfProcessingServiceMock.Object);

            _useCase = new ProcessEmbeddingUseCase(
                _workflowRepositoryMock.Object,
                _documentProcessorFactoryMock.Object,
                _embeddingProcessingServiceMock.Object,
                _jobStatusRepositoryMock.Object,
                _apiKeyResolverServiceMock.Object
                );
        }

        [Fact]
        public async Task Execute_Returns_Correct_ProcessedChunks()
        {
            // Arrange
            const string dummyApiKey = "dummyApiKey";

            // Creates a dummy DocumentExtractResult with a single page (containing three "chunks")
            var pdfResult = new DocumentExtractResult
            {
                DocumentTitle = "TestDocument",
                Pages = ["Chunk 1. Chunk 2. Chunk 3."]
            };

            // Creates a dummy IFormFile representing a PDF file
            var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(pdfResult.Pages.First()));
            IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, "dummy", "dummy.pdf");

            // Creates a dummy Workflow
            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                CollectionId = Guid.NewGuid(),
                Chunker = new DummyChunkerConfig(),
                EmbeddingProviderConfig = new EmbeddingProviderConfig { },
                ConversationProviderConfig = new ConversationProviderConfig { },
                DocumentsCount = 0,
                ApiKey = dummyApiKey
            };

            // Configures the workflow repository
            _workflowRepositoryMock
                .Setup(repo => repo.GetWithRelationsByApiKey(dummyApiKey))
                .ReturnsAsync(workflow);

            // Configures the extract method to return the simulated result
            _pdfProcessingServiceMock
                .Setup(service => service.ExtractTextAsync(file))
                .ReturnsAsync(pdfResult);

            // Creates dummy Pages to compose the document
            var dummyPages = pdfResult.Pages
                .Select(text => new Page { Id = Guid.NewGuid(), Text = text })
                .ToList();

            var document = new Document
            {
                Id = Guid.NewGuid(),
                Title = "TestDocument",
                Pages = dummyPages
            };

            // Configures the creation of the document from the adapter
            _pdfProcessingServiceMock
                .Setup(service => service.CreateDocumentWithPagesAsync(
                    It.IsAny<string>(),
                    workflow.Id,
                    pdfResult.Pages))
                .ReturnsAsync(document);

            // Configures the chunking method: splits the page text into three chunks using periods as delimiters
            _embeddingProcessingServiceMock
                .Setup(service => service.ChunkTextAsync(
                    It.IsAny<string>(),
                    workflow.Chunker,
                    workflow.ConversationProviderConfig,
                    It.IsAny<string>() // ApiKey
                    ))
                .ReturnsAsync((string text, Chunker chunker, ConversationProviderConfig config, string userEmbeddingProviderApiKey) =>
                {
                    return text.Split('.', StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => x.Trim())
                               .Where(x => !string.IsNullOrEmpty(x));
                });

            // Configures the method to obtain embeddings for the chunks
            _embeddingProcessingServiceMock
                .Setup(service => service.GetEmbeddingsAsync(
                    It.IsAny<List<string>>(),
                    workflow.EmbeddingProviderConfig,
                    It.IsAny<string>() // ApiKey
                    ))
                .ReturnsAsync((List<string> chunks, EmbeddingProviderConfig _, string userEmbeddingProviderApiKey) =>
                {
                    return chunks.Select(chunk => (
                        ChunkText: chunk,
                        VectorId: Guid.NewGuid().ToString(),
                        Embedding: singleArray
                    )).ToList();
                });

            // Configures the methods for inserting embeddings and chunks
            _embeddingProcessingServiceMock
                .Setup(service => service.InsertEmbeddingBatchAsync(
                    It.IsAny<List<(string VectorId, float[] Embedding, Dictionary<string, string> Metadata)>>(),
                    It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _embeddingProcessingServiceMock
                .Setup(service => service.AddChunksAsync(It.IsAny<List<Chunk>>()))
                .Returns(Task.CompletedTask);

            // Act
            int processedChunks = await _useCase.Execute(file, workflow);

            // Assert
            // Expected to process three chunks, based on the page split
            Assert.Equal(3, processedChunks);
            Assert.Equal(1, workflow.DocumentsCount);
        }

        [Fact]
        public async Task ExecuteStreaming_Returns_All_Progress_Updates()
        {
            // Arrange
            const string dummyApiKey = "dummyApiKey";

            // Creates a dummy page with three chunks
            List<string> pages = ["Chunk 1. Chunk 2. Chunk 3."];
            var pdfResult = new DocumentExtractResult
            {
                DocumentTitle = "TestDocument",
                Pages = pages
            };

            // Creates a dummy IFormFile representing a PDF file
            var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(pages.First()));
            IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, "dummy", "dummy.pdf");

            // Creates a dummy Workflow
            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                CollectionId = Guid.NewGuid(),
                Chunker = new DummyChunkerConfig(),
                EmbeddingProviderConfig = new EmbeddingProviderConfig { },
                ConversationProviderConfig = new ConversationProviderConfig { },
                DocumentsCount = 0,
                ApiKey = dummyApiKey
            };

            // Configures workflow and extraction mocks
            _workflowRepositoryMock
                .Setup(repo => repo.GetWithRelationsByApiKey(dummyApiKey))
                .ReturnsAsync(workflow);

            _pdfProcessingServiceMock
                .Setup(service => service.ExtractTextAsync(file))
                .ReturnsAsync(pdfResult);

            var dummyPage = new Page { Id = Guid.NewGuid(), Text = pages.First() };
            var document = new Document
            {
                Id = Guid.NewGuid(),
                Title = "TestDocument",
                Pages = new List<Page> { dummyPage }
            };

            _pdfProcessingServiceMock
                .Setup(service => service.CreateDocumentWithPagesAsync(
                    It.IsAny<string>(),
                    workflow.Id,
                    pdfResult.Pages))
                .ReturnsAsync(document);

            // Configures chunking method to split the text into three chunks
            _embeddingProcessingServiceMock
                .Setup(service => service.ChunkTextAsync(
                    dummyPage.Text,
                    workflow.Chunker,
                    workflow.ConversationProviderConfig,
                    It.IsAny<string>() // ApiKey
                    ))
                .ReturnsAsync((string text, Chunker chunker, ConversationProviderConfig config, string userEmbeddingProviderApiKey) =>
                {
                    return text.Split('.', StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => x.Trim())
                               .Where(x => !string.IsNullOrEmpty(x));
                });

            // Configures the method to obtain embeddings for the chunks
            _embeddingProcessingServiceMock
                .Setup(service => service.GetEmbeddingsAsync(
                    It.IsAny<List<string>>(),
                    workflow.EmbeddingProviderConfig,
                    It.IsAny<string>() // ApiKey
                    ))
                .ReturnsAsync((List<string> chunks, EmbeddingProviderConfig _, string userEmbeddingProviderApiKey) =>
                {
                    return chunks.Select(chunk => (
                        ChunkText: chunk,
                        VectorId: Guid.NewGuid().ToString(),
                        Embedding: new float[] { 0.1f, 0.2f, 0.3f }
                    )).ToList();
                });

            // Configures the methods for inserting embeddings and chunks
            _embeddingProcessingServiceMock
                .Setup(service => service.InsertEmbeddingBatchAsync(
                    It.IsAny<List<(string VectorId, float[] Embedding, Dictionary<string, string> Metadata)>>(),
                    It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _embeddingProcessingServiceMock
                .Setup(service => service.AddChunksAsync(It.IsAny<List<Chunk>>()))
                .Returns(Task.CompletedTask);

            // Act
            var progressUpdates = new List<EmbeddingProgressDTO>();
            await foreach (var progress in _useCase.ExecuteStreaming(file, workflow))
            {
                progressUpdates.Add(progress);
            }

            // Assert
            Assert.Equal(3, progressUpdates.Last().TotalChunks);
            Assert.True(progressUpdates.Last().ProcessedChunks >= 3);
            Assert.Equal(1, workflow.DocumentsCount);
        }
    }

    internal class DummyChunkerConfig : Chunker
    {
    }
}
