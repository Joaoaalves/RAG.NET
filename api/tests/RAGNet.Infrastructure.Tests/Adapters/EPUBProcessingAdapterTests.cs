using Moq;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Adapters.Documents;
using tests.Helpers;

namespace tests.RAGNet.Infrastructure.Tests.Adapters
{
    public class EpubProcessingAdapterTests
    {
        private readonly Mock<IDocumentRepository> _documentRepositoryMock;
        private readonly Mock<IPageRepository> _pageRepositoryMock;
        private readonly EpubProcessingAdapter _adapter;

        public EpubProcessingAdapterTests()
        {
            _documentRepositoryMock = new Mock<IDocumentRepository>();
            _pageRepositoryMock = new Mock<IPageRepository>();

            _adapter = new EpubProcessingAdapter(_documentRepositoryMock.Object, _pageRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateDocumentWithPagesAsync_Deve_CriarDocumentoEAdicionarPaginas()
        {
            // Arrange
            var title = "Documento EPUB de Teste";
            var workflowId = Guid.NewGuid();
            var pages = new List<string> { "Página 1", "Página 2" };

            var documentoCriado = new Document { Id = Guid.NewGuid(), Title = title, WorkflowId = workflowId };
            _documentRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Document>()))
                .ReturnsAsync(documentoCriado);

            // Act
            var document = await _adapter.CreateDocumentWithPagesAsync(title, workflowId, pages);

            // Assert
            Assert.Equal(documentoCriado.Id, document.Id);
            _documentRepositoryMock.Verify(r => r.AddAsync(It.Is<Document>(d =>
                d.Title == title && d.WorkflowId == workflowId)), Times.Once);

            foreach (var page in pages)
            {
                _pageRepositoryMock.Verify(r => r.AddAsync(It.Is<Page>(
                    p => p.Text == page.Trim() && p.DocumentId == document.Id)), Times.Once);
            }
        }

        [Fact]
        public async Task ExtractTextAsync_Shoul_ExtractPagesFromEPUB()
        {
            // Arrange
            var epubFilePath = TestFileHelper.GetTestFilePath("sample.epub");
            Assert.True(File.Exists(epubFilePath), "EPUB test file not found.");

            var epubBytes = File.ReadAllBytes(epubFilePath);

            // Create a memory stream instead of mocking IFormFile
            await using var ms = new MemoryStream(epubBytes);

            // Act
            // Pass the stream and the file name to the new signature
            var result = await _adapter.ExtractTextAsync(ms);

            // Assert
            Assert.NotEmpty(result.Pages);
        }

        [Fact]
        public async Task ExtractTextAsync_Should_ReturnPagesFromRealEpubFile()
        {
            // Arrange
            var epubFilePath = TestFileHelper.GetTestFilePath("sample.epub");
            Assert.True(File.Exists(epubFilePath), "EPUB test file not found.");

            // Load bytes and wrap in MemoryStream
            var epubBytes = File.ReadAllBytes(epubFilePath);
            await using var ms = new MemoryStream(epubBytes);

            // Create adapter (no need to mock IFormFile anymore)
            var adapter = new EpubProcessingAdapter(
                Mock.Of<IDocumentRepository>(),
                Mock.Of<IPageRepository>()
            );

            // Act
            // Call the new signature: (Stream, fileName)
            var result = await adapter.ExtractTextAsync(
                fileStream: ms
            );

            // Assert basic contract
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.DocumentTitle),
                "Document title should not be empty.");
            Assert.NotEmpty(result.Pages);

            // Ensure pages are cleaned up (no style/meta tags etc.)
            foreach (var page in result.Pages)
            {
                Assert.False(string.IsNullOrWhiteSpace(page),
                    "Page content should not be empty.");
                Assert.DoesNotContain("<style", page,
                    StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("<meta", page,
                    StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
