using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Infrastructure.Data;
using RAGNET.Infrastructure.Repositories;


namespace tests.RAGNet.Infrastructure.Tests.Repositories
{
    public class ChunkerRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ChunkerRepository _repository;

        public ChunkerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ChunkerRepository(_context);
        }

        [Fact]
        public async Task GetWithMeta_ShouldReturnChunkersWithMeta()
        {
            // Arrange
            var chunker = new Chunker
            {
                Id = Guid.NewGuid(),
                WorkflowId = Guid.NewGuid(),
                StrategyType = ChunkerStrategy.SEMANTIC,
                Metas =
                [
                    new() { Key = "key1", Value = "value1" }
                ]
            };

            await _context.Chunkers.AddAsync(chunker);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetWithMetaAsync(chunker.Id);

            // Assert
            Assert.NotEmpty(result);
            Assert.Single(result.First().Metas);
            Assert.Equal("key1", result.First().Metas.First().Key);
            Assert.Equal("value1", result.First().Metas.First().Value);
        }

        [Fact]
        public async Task ShouldReturnChunkersWithMeta()
        {
            // Arrange
            var chunker = new Chunker
            {
                Id = Guid.NewGuid(),
                WorkflowId = Guid.NewGuid(),
                StrategyType = ChunkerStrategy.PROPOSITION,
                Metas =
                [
                    new() { Key = "key1", Value = "value1" }
                ]
            };

            _context.Chunkers.Add(chunker);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetWithMetaAsync(chunker.Id);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal("key1", result.First().Metas.First().Key);
            Assert.Equal("value1", result.First().Metas.First().Value);
        }

    }
}
