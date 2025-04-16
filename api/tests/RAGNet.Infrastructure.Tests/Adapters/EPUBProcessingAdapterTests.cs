using System.Text;
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
            Console.WriteLine($"Total pages extracted: {result.Pages.Count}");
        }

        [Fact]
        public void PaginateText_Should_Return_PagesWithExpectedWordCount()
        {
            // Arrange
            string texto = "This is a test message that will be divided in pages " +
                            "Each page should contain a fixed number of words to ensure that the text is paginated correctly. ";
            int wordsPerPage = 5;

            // reflection for private method access
            var method = typeof(EpubProcessingAdapter).GetMethod("PaginateText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var result = method.Invoke(_adapter, [texto, wordsPerPage]) as List<string>;

            // Assert
            Assert.NotNull(result);

            // Valida que cada página possui no máximo as palavras definidas (exceto possivelmente a última)
            foreach (var page in result)
            {
                var count = page.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                Assert.True(count <= wordsPerPage);
            }
        }
    }
}
