using Microsoft.EntityFrameworkCore;
using Moq;

using RAGNET.Domain.Entities;
using RAGNET.Domain.SharedKernel.Providers;

using RAGNET.Infrastructure.Data;
using RAGNET.Infrastructure.Repositories;

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
            var workflow = new Workflow
            {
                Id = It.IsAny<Guid>(),
                Name = "Name",
                UserId = It.IsAny<Guid>().ToString(),
                ApiKey = It.IsAny<Guid>().ToString("N"),
                CollectionId = It.IsAny<Guid>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

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
            var embeddingProvider = new EmbeddingProviderConfig
            {
                Id = It.IsAny<Guid>(),
                Provider = EmbeddingProviderEnum.OPENAI,
                VectorSize = 1000
            };

            var workflow = new Workflow
            {
                Id = It.IsAny<Guid>(),
                Name = "Name",
                UserId = It.IsAny<Guid>().ToString(),
                ApiKey = It.IsAny<Guid>().ToString("N"),
                CollectionId = It.IsAny<Guid>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EmbeddingProviderConfig = embeddingProvider
            };

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
            string correctApiKey = Guid.NewGuid().ToString("N");
            string wrongApiKey = "wrong-api-key";

            var embeddingProvider = new EmbeddingProviderConfig
            {
                Id = Guid.NewGuid(),
                Provider = EmbeddingProviderEnum.OPENAI,
                VectorSize = 1000
            };

            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                Name = "Valid Workflow",
                UserId = Guid.NewGuid().ToString(),
                ApiKey = correctApiKey,
                CollectionId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EmbeddingProviderConfig = embeddingProvider
            };

            // Act
            await _repository.AddAsync(workflow);
            await _context.SaveChangesAsync();

            var search = await _repository.GetWithRelationsByApiKey(wrongApiKey);

            // Assert
            Assert.Null(search);
        }

        [Fact]
        public async Task ShouldDelete()
        {
            // Arrange
            var id = Guid.NewGuid();
            var apiKey = Guid.NewGuid().ToString("N");

            var embeddingProvider = new EmbeddingProviderConfig
            {
                Id = Guid.NewGuid(),
                Provider = EmbeddingProviderEnum.OPENAI,
                VectorSize = 1000
            };

            var workflow = new Workflow
            {
                Id = id,
                Name = "Test Workflow",
                UserId = Guid.NewGuid().ToString(),
                ApiKey = apiKey,
                CollectionId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EmbeddingProviderConfig = embeddingProvider
            };

            // Act
            await _repository.AddAsync(workflow);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(workflow);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }
    }
}