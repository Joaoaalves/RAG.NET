using Microsoft.AspNetCore.Http;
using Moq;
using RAGNET.Application.DTOs;
using RAGNET.Application.UseCases.EmbeddingUseCases;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using tests.RAGNet.Application.Tests.Dummies;

namespace tests.RAGNet.Application.Tests
{
    public class ProcessEmbeddingUseCaseTests
    {
        private readonly Mock<IWorkflowRepository> _workflowRepositoryMock;
        private readonly Mock<IPdfTextExtractorService> _pdfTextExtractorMock;
        private readonly Mock<ITextChunkerFactory> _chunkerFactoryMock;
        private readonly Mock<IEmbedderFactory> _embedderFactoryMock;
        private readonly Mock<IVectorDatabaseService> _vectorDatabaseServiceMock;
        private readonly ProcessEmbeddingUseCase _useCase;

        public ProcessEmbeddingUseCaseTests()
        {
            _workflowRepositoryMock = new Mock<IWorkflowRepository>();
            _pdfTextExtractorMock = new Mock<IPdfTextExtractorService>();
            _chunkerFactoryMock = new Mock<ITextChunkerFactory>();
            _embedderFactoryMock = new Mock<IEmbedderFactory>();
            _vectorDatabaseServiceMock = new Mock<IVectorDatabaseService>();

            _useCase = new ProcessEmbeddingUseCase(
                _workflowRepositoryMock.Object,
                _pdfTextExtractorMock.Object,
                _chunkerFactoryMock.Object,
                _embedderFactoryMock.Object,
                _vectorDatabaseServiceMock.Object);
        }

        [Fact]
        public async Task Execute_Returns_Correct_ProcessedChunks()
        {
            // Arrange
            string apiKey = It.IsAny<string>();
            string fileContent = "Chunk1. Chunk2. Chunk3.";

            var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
            IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, It.IsAny<string>(), It.IsAny<string>());

            // Cria um workflow dummy com as configurações necessárias.
            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                CollectionId = Guid.NewGuid(),
                Chunker = new DummyChunkerConfig(),
                EmbeddingProviderConfig = new EmbeddingProviderConfig { ApiKey = It.IsAny<string>() }
            };

            _workflowRepositoryMock
                .Setup(repo => repo.GetWithRelationsByApiKey(apiKey))
                .ReturnsAsync(workflow);

            _pdfTextExtractorMock
                .Setup(extractor => extractor.ExtractTextAsync(file))
                .ReturnsAsync(fileContent);

            // Configura o factory para retornar o dummy do chunker.
            var dummyChunker = new DummyTextChunker();
            _chunkerFactoryMock
                .Setup(factory => factory.CreateChunker(workflow.Chunker))
                .Returns(dummyChunker);

            // Configura o factory para retornar o dummy do embedder.
            var dummyEmbedder = new DummyEmbedder();
            _embedderFactoryMock
                .Setup(factory => factory.CreateEmbeddingService(workflow.EmbeddingProviderConfig.ApiKey, It.IsAny<string>()))
                .Returns(dummyEmbedder);

            // VectorServiceMock
            _vectorDatabaseServiceMock
                .Setup(service => service.InsertAsync(
                    It.IsAny<string>(),
                    It.IsAny<float[]>(),
                    workflow.CollectionId.ToString(),
                    It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.CompletedTask);

            // Act
            int processedChunks = await _useCase.Execute(file, apiKey);

            // Assert
            Assert.Equal(3, processedChunks);
        }

        [Fact]
        public async Task ExecuteStreaming_Returns_All_Progress_Updates()
        {
            // Arrange
            string apiKey = It.IsAny<string>();
            string fileContent = "Chunk1. Chunk2. Chunk3.";
            var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
            IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, It.IsAny<string>(), It.IsAny<string>());

            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                CollectionId = Guid.NewGuid(),
                Chunker = new DummyChunkerConfig(),
                EmbeddingProviderConfig = new EmbeddingProviderConfig { ApiKey = It.IsAny<string>() }
            };

            _workflowRepositoryMock
                .Setup(repo => repo.GetWithRelationsByApiKey(apiKey))
                .ReturnsAsync(workflow);

            _pdfTextExtractorMock
                .Setup(extractor => extractor.ExtractTextAsync(file))
                .ReturnsAsync(fileContent);

            var dummyChunker = new DummyTextChunker();
            _chunkerFactoryMock
                .Setup(factory => factory.CreateChunker(workflow.Chunker))
                .Returns(dummyChunker);

            var dummyEmbedder = new DummyEmbedder();
            _embedderFactoryMock
                .Setup(factory => factory.CreateEmbeddingService(workflow.EmbeddingProviderConfig.ApiKey, It.IsAny<string>()))
                .Returns(dummyEmbedder);

            _vectorDatabaseServiceMock
                .Setup(service => service.InsertAsync(
                    It.IsAny<string>(),
                    It.IsAny<float[]>(),
                    workflow.CollectionId.ToString(),
                    It.IsAny<Dictionary<string, string>>()))
                .Returns(Task.CompletedTask);

            // Act
            var progressUpdates = new List<EmbeddingProgressDTO>();
            await foreach (var progress in _useCase.ExecuteStreaming(file, apiKey))
            {
                progressUpdates.Add(progress);
            }

            // Assert
            // Due to thread safety, we can`t assure the last chunk would be the third
            Assert.InRange(progressUpdates.Last().ProcessedChunks, 2, 3);
            Assert.All(progressUpdates, update => Assert.Equal(3, update.TotalChunks));
        }

    }

    internal class DummyChunkerConfig : Chunker
    {
    }


}

