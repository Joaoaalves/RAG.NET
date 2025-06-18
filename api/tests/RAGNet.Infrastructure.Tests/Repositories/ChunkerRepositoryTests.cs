using Microsoft.EntityFrameworkCore;
using Moq;
using RAGNET.Domain.Chunkers;
using RAGNET.Infrastructure.Database;
using RAGNET.Infrastructure.Domain.Chunkers;


namespace tests.RAGNet.Infrastructure.Tests.Repositories
{
    public class ChunkerRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ChunkerRepository _repository;
        private readonly Guid _workflowId = Guid.NewGuid();
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
            var chunker = Chunker.Create(
                ChunkerStrategy.SEMANTIC,
                _workflowId,
                It.IsAny<string>(),
                [
                    new( "key1", "value1")
                ]
            );

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
            var chunker = Chunker.Create(
                ChunkerStrategy.SEMANTIC,
                _workflowId,
                It.IsAny<string>(),
                [
                    new("key1", "value1")
                ]
            );

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
