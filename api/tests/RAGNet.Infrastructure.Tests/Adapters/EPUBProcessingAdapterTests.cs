using Microsoft.AspNetCore.Http;
using Moq;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Adapters.Document;
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

            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(epubBytes));

            // Act
            var result = await _adapter.ExtractTextAsync(formFileMock.Object);

            // Assert
            Assert.NotEmpty(result.Pages);
        }

        [Fact]
        public async Task ExtractTextAsync_Should_ReturnPagesFromRealEpubFile()
        {
            // Arrange
            var epubFilePath = TestFileHelper.GetTestFilePath("sample.epub");
            Assert.True(File.Exists(epubFilePath), "EPUB test file not found.");
            var epubBytes = File.ReadAllBytes(epubFilePath);

            // Simulate uploaded file (IFormFile)
            var stream = new MemoryStream(epubBytes);
            var formFile = new FormFile(stream, 0, epubBytes.Length, "file", "sample.epub")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/epub+zip"
            };

            var adapter = new EpubProcessingAdapter(
                Mock.Of<IDocumentRepository>(),
                Mock.Of<IPageRepository>()
            );

            // Act
            var result = await adapter.ExtractTextAsync(formFile);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.DocumentTitle), "Document title should not be empty.");
            Assert.NotEmpty(result.Pages); // At least one page should be returned

            foreach (var page in result.Pages)
            {
                Assert.False(string.IsNullOrWhiteSpace(page), "Page content should not be empty.");
                Assert.DoesNotContain("<style", page); // Example: style tags should be removed
                Assert.DoesNotContain("<meta", page);  // Example: meta tags should be removed
            }
        }
    }
}
