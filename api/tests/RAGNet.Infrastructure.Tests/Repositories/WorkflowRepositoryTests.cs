using System;
using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
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
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
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
                Id = Guid.NewGuid(),
                Name = "Name",
                UserId = Guid.NewGuid().ToString(),
                ApiKey = Guid.NewGuid().ToString("N"),
                CollectionId = Guid.NewGuid(),
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
                Id = Guid.NewGuid(),
                ApiKey = "Random Key",
                Provider = EmbeddingProviderEnum.OPENAI,
                VectorSize = 1000
            };

            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                UserId = Guid.NewGuid().ToString(),
                ApiKey = Guid.NewGuid().ToString("N"),
                CollectionId = Guid.NewGuid(),
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
            Assert.Equal("Random Key", result.EmbeddingProviderConfig.ApiKey);
            Assert.Equal(EmbeddingProviderEnum.OPENAI, result.EmbeddingProviderConfig.Provider);
        }
    }
}