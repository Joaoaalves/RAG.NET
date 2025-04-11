using Microsoft.AspNetCore.Http;
using Moq;
using RAGNET.Application.DTOs;
using RAGNET.Application.UseCases.EmbeddingUseCases;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace tests.RAGNet.Application.Tests
{
    public class ProcessEmbeddingUseCaseTests
    {
        private readonly Mock<IWorkflowRepository> _workflowRepositoryMock;
        private readonly Mock<IPDFProcessingService> _pdfProcessingServiceMock;
        private readonly Mock<IEmbeddingProcessingService> _embeddingProcessingServiceMock;
        private readonly ProcessEmbeddingUseCase _useCase;
        private static readonly float[] sourceArray = { 0.1f, 0.2f, 0.3f };
        private static readonly float[] singleArray = new float[] { 0.1f, 0.2f, 0.3f };

        public ProcessEmbeddingUseCaseTests()
        {
            _workflowRepositoryMock = new Mock<IWorkflowRepository>();
            _pdfProcessingServiceMock = new Mock<IPDFProcessingService>();
            _embeddingProcessingServiceMock = new Mock<IEmbeddingProcessingService>();

            _useCase = new ProcessEmbeddingUseCase(
                _workflowRepositoryMock.Object,
                _pdfProcessingServiceMock.Object,
                _embeddingProcessingServiceMock.Object);
        }

        [Fact]
        public async Task Execute_Returns_Correct_ProcessedChunks()
        {
            // Arrange
            string apiKey = "dummyApiKey";

            // Create a dummy PDF extraction result with one page
            var pdfResult = new PDFExtractResult
            {
                DocumentTitle = "TestDocument",
                Pages = new List<string> { "Chunk 1. Chunk 2. Chunk 3." }
            };

            // Create a dummy IFormFile
            var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(pdfResult.Pages.First()));
            IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, "dummy", "dummy.pdf");

            // Create a dummy workflow
            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                CollectionId = Guid.NewGuid(),
                Chunker = new DummyChunkerConfig(),
                EmbeddingProviderConfig = new EmbeddingProviderConfig { ApiKey = "embeddingApiKey" },
                ConversationProviderConfig = new ConversationProviderConfig { ApiKey = "conversationApiKey" },
                DocumentsCount = 0
            };

            // Set up the workflow repository mock
            _workflowRepositoryMock
                .Setup(repo => repo.GetWithRelationsByApiKey(apiKey))
                .ReturnsAsync(workflow);

            // Set up the PDF extraction service mock
            _pdfProcessingServiceMock
                .Setup(service => service.ExtractTextAsync(file))
                .ReturnsAsync(pdfResult);

            // Create dummy pages for the document
            var dummyPages = pdfResult.Pages
                .Select(text => new Page { Id = Guid.NewGuid(), Text = text })
                .ToList();
            var document = new Document
            {
                Id = Guid.NewGuid(),
                Title = "TestDocument",
                Pages = dummyPages
            };

            _pdfProcessingServiceMock
                .Setup(service => service.CreateDocumentWithPagesAsync(
                    It.IsAny<string>(),
                    workflow.Id,
                    pdfResult.Pages))
                .ReturnsAsync(document);

            // Set up the chunking method to split text using period as a delimiter
            _embeddingProcessingServiceMock
                .Setup(service => service.ChunkTextAsync(
                    It.IsAny<string>(),
                    workflow.Chunker,
                    workflow.ConversationProviderConfig))
                .ReturnsAsync((string text, Chunker chunker, ConversationProviderConfig config) =>
                {
                    return text.Split('.', StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => x.Trim())
                               .Where(x => !string.IsNullOrEmpty(x));
                });

            // Set up the method to get embeddings for chunks
            _embeddingProcessingServiceMock
                .Setup(service => service.GetEmbeddingsAsync(
                    It.IsAny<List<string>>(),
                    workflow.EmbeddingProviderConfig))
                .ReturnsAsync((List<string> chunks, EmbeddingProviderConfig _) =>
                {
                    return chunks.Select(chunk => (
                        ChunkText: chunk,
                        VectorId: Guid.NewGuid().ToString(),
                        Embedding: singleArray)).ToList();
                });

            // Set up the method to insert embeddings into the vector database
            _embeddingProcessingServiceMock
                .Setup(service => service.InsertEmbeddingBatchAsync(
                    It.IsAny<List<(string VectorId, float[] Embedding, Dictionary<string, string> Metadata)>>(),
                    It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Set up the method to insert chunks into the relational database
            _embeddingProcessingServiceMock
                .Setup(service => service.AddChunksAsync(It.IsAny<List<Chunk>>()))
                .Returns(Task.CompletedTask);

            // Act
            int processedChunks = await _useCase.Execute(file, workflow);

            // Assert
            // We expect three chunks to be processed (based on the tokens extracted from the single page)
            Assert.Equal(3, processedChunks);
            Assert.Equal(1, workflow.DocumentsCount);
        }

        [Fact]
        public async Task ExecuteStreaming_Returns_All_Progress_Updates()
        {
            // Arrange
            string apiKey = "dummyApiKey";

            // Create a dummy page with three chunks
            List<string> pages = new List<string> { "Chunk 1. Chunk 2. Chunk 3." };
            var pdfResult = new PDFExtractResult
            {
                DocumentTitle = "TestDocument",
                Pages = pages
            };

            // Create a dummy IFormFile
            var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(pages[0]));
            IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, "dummy", "dummy.pdf");

            // Create a dummy workflow
            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                CollectionId = Guid.NewGuid(),
                Chunker = new DummyChunkerConfig(),
                EmbeddingProviderConfig = new EmbeddingProviderConfig { ApiKey = "embeddingApiKey" },
                ConversationProviderConfig = new ConversationProviderConfig { ApiKey = "conversationApiKey" },
                DocumentsCount = 0
            };

            // Set up mocks
            _workflowRepositoryMock
                .Setup(repo => repo.GetWithRelationsByApiKey(apiKey))
                .ReturnsAsync(workflow);

            _pdfProcessingServiceMock
                .Setup(service => service.ExtractTextAsync(file))
                .ReturnsAsync(pdfResult);

            var dummyPage = new Page { Id = Guid.NewGuid(), Text = pages[0] };
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

            // Set up ChunkTextAsync to split text into three chunks
            _embeddingProcessingServiceMock
                .Setup(service => service.ChunkTextAsync(
                    dummyPage.Text,
                    workflow.Chunker,
                    workflow.ConversationProviderConfig))
                .ReturnsAsync((string text, Chunker chunker, ConversationProviderConfig config) =>
                {
                    return text.Split('.', StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => x.Trim())
                               .Where(x => !string.IsNullOrEmpty(x));
                });

            // Set up the method to get embeddings for chunks
            _embeddingProcessingServiceMock
                .Setup(service => service.GetEmbeddingsAsync(
                    It.IsAny<List<string>>(),
                    workflow.EmbeddingProviderConfig))
                .ReturnsAsync((List<string> chunks, EmbeddingProviderConfig _) =>
                {
                    return chunks.Select(chunk => (
                        ChunkText: chunk,
                        VectorId: Guid.NewGuid().ToString(),
                        Embedding: new float[] { 0.1f, 0.2f, 0.3f }
                    )).ToList();
                });

            // Set up the method to insert embeddings into the vector database
            _embeddingProcessingServiceMock
                .Setup(service => service.InsertEmbeddingBatchAsync(
                    It.IsAny<List<(string VectorId, float[] Embedding, Dictionary<string, string> Metadata)>>(),
                    It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Set up the method to insert chunks into the relational database
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
            // We expect the total number of chunks to be 3 and that the progress updates indicate ProcessedChunks >= 3
            Assert.Equal(3, progressUpdates.Last().TotalChunks);
            Assert.True(progressUpdates.Last().ProcessedChunks >= 3);
            Assert.Equal(1, workflow.DocumentsCount);
        }
    }

    internal class DummyChunkerConfig : Chunker
    {
    }
}
