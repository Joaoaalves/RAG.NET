using Microsoft.EntityFrameworkCore;
using Moq;

using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Database;
using RAGNET.Infrastructure.Domain.Workflows;

namespace tests.RAGNet.Infrastructure.Tests.Repositories
{
    public class WorkflowRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkflowRepository _repository;

        public WorkflowRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(It.IsAny<Guid>().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new WorkflowRepository(_context);
        }

        [Fact]
        public async Task ShouldCreateWithoutEmbeddingProvider()
        {
            // Arrange
            var workflowId = new WorkflowId(Guid.NewGuid());
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

            var workflowId = new WorkflowId(Guid.NewGuid());
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
                model: It.IsAny<string>(),
                vectorSize: 1000
            );

            var workflowId = new WorkflowId(Guid.NewGuid());
            var workflow = new WorkflowBuilder()
                .WithName("Name")
                .ForUser(It.IsAny<Guid>().ToString())
                .WithApiKey(It.IsAny<Guid>().ToString("N"))
                .WithEmbeddingProvider(embeddingProvider)
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

            var embeddingProvider = new EmbeddingProviderConfig(
                provider: EmbeddingProviderEnum.OPENAI,
                model: It.IsAny<string>(),
                vectorSize: 1000
            );

            var workflowId = new WorkflowId(Guid.NewGuid());
            var workflow = new WorkflowBuilder()
                .WithName("Name")
                .ForUser(It.IsAny<Guid>().ToString())
                .WithApiKey(apiKey)
                .WithEmbeddingProvider(embeddingProvider)
                .Build(workflowId);

            // Act
            await _repository.AddAsync(workflow);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(workflow, It.IsAny<Guid>().ToString());
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(workflowId, It.IsAny<Guid>().ToString());

            // Assert
            Assert.Null(result);
        }
    }
}