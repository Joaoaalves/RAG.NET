using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Database;
using RAGNET.Infrastructure.Domain.Workflows;
using RAGNET.Infrastructure.SeedWork;
using tests.RAGNet.Domain.Tests.Chunkers;
using tests.RAGNet.Infrastructure.Tests.Database;

namespace tests.RAGNet.Infrastructure.Tests.Repositories
{
    public class WorkflowRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkflowRepository _repository;

        public WorkflowRepositoryTests()
        {
            _context = TestDbContextFactory.CreateInMemoryContext();
            _repository = new WorkflowRepository(_context);
        }
        [Fact]
        public async Task ShouldCreateWithoutEmbeddingProvider()
        {
            // Arrange
            var workflowId = new WorkflowId();
            var workflow = new WorkflowBuilder()
                .WithName("Name")
                .ForUser(It.IsAny<Guid>().ToString())
                .WithApiKey(It.IsAny<Guid>().ToString("N"))
                .Build(workflowId);


            // Act
            var result = await _repository.AddAsync(workflow);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Name);
            Assert.NotNull(result.UserId);
            Assert.NotNull(result.ApiKey);
        }

        [Fact]
        public async Task ShouldCreateWithEmbeddingProvider()
        {
            // Arrange
            var embeddingProvider = new EmbeddingProviderConfig(
                provider: EmbeddingProviderEnum.OPENAI,
                model: It.IsAny<string>(),
                vectorSize: 1000
            );

            var workflowId = new WorkflowId();
            var workflow = new WorkflowBuilder()
                .WithName("Name")
                .ForUser(It.IsAny<Guid>().ToString())
                .WithApiKey(It.IsAny<Guid>().ToString("N"))
                .WithEmbeddingProvider(embeddingProvider)
                .Build(workflowId);

            // Act
            var result = await _repository.AddAsync(workflow);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.EmbeddingProviderConfig);
            Assert.Equal(1000, result.EmbeddingProviderConfig.VectorSize);
            Assert.Equal(EmbeddingProviderEnum.OPENAI, result.EmbeddingProviderConfig.Provider);
        }

        [Fact]
        public async Task ShoulReturnNullWithWrongApiKey()
        {
            // Arrange
            string wrongApiKey = "wrong-api-key";

            var embeddingProvider = new EmbeddingProviderConfig(
                provider: EmbeddingProviderEnum.OPENAI,
                model: "model",
                vectorSize: 1000
            );

            var conversationProvider = new ConversationProviderConfig(
                provider: ConversationProviderEnum.OPENAI,
                model: "model"
            );

            var workflowId = new WorkflowId();
            var workflow = new WorkflowBuilder()
                .WithName("Name")
                .ForUser(It.IsAny<Guid>().ToString())
                .WithApiKey(It.IsAny<Guid>().ToString("N"))
                .WithEmbeddingProvider(embeddingProvider)
                .WithConversationProvider(conversationProvider)
                .Build(workflowId);

            // Act
            await _repository.AddAsync(workflow);
            await _context.SaveChangesAsync();

            var search = await _repository.GetByApiKey(wrongApiKey);

            // Assert
            Assert.Null(search);
        }

        [Fact]
        public async Task ShouldDelete()
        {
            // Arrange
            var apiKey = Guid.NewGuid().ToString("N");
            var userId = Guid.NewGuid().ToString();

            var embeddingProvider = new EmbeddingProviderConfig(
                provider: EmbeddingProviderEnum.OPENAI,
                model: "model",
                vectorSize: 1000
            );

            var workflowId = new WorkflowId();
            var workflow = new WorkflowBuilder()
                .WithName("Name")
                .ForUser(userId)
                .WithApiKey(apiKey)
                .WithEmbeddingProvider(embeddingProvider)
                .WithChunker(DummyChunker.CreateDummy())
                .Build(workflowId);

            // Add workflow to the context
            await _repository.AddAsync(workflow);
            await _context.SaveChangesAsync();

            // Act - 1st delete call: should deactivate the workflow
            await _repository.DeleteAsync(workflow, userId);
            await _context.SaveChangesAsync();

            var deactivated = await _repository.GetByIdAsync(workflowId, userId);

            // Assert that workflow is still there but inactive
            Assert.NotNull(deactivated);
            Assert.False(deactivated!.IsActive);

            // Act - 2nd delete call: should remove the workflow from DB
            await _repository.DeleteAsync(workflow, userId);
            await _context.SaveChangesAsync();

            var removed = await _repository.GetByIdAsync(workflowId, userId);

            // Assert that workflow no longer exists
            Assert.Null(removed);
        }

    }
}