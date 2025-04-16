using Microsoft.AspNetCore.Http;
using Moq;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Adapters.Document;
using tests.Helpers;

namespace tests.RAGNet.Infrastructure.Tests.Adapters
{
    public class PdfProcessingAdapterTests
    {
        private readonly Mock<IDocumentRepository> _documentRepositoryMock;
        private readonly Mock<IPageRepository> _pageRepositoryMock;
        private readonly PDFProcessingAdapter _adapter;

        public PdfProcessingAdapterTests()
        {
            _documentRepositoryMock = new Mock<IDocumentRepository>();
            _pageRepositoryMock = new Mock<IPageRepository>();

            _adapter = new PDFProcessingAdapter(_documentRepositoryMock.Object, _pageRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateDocumentWithPagesAsync_Should_CreateDocumentAndAddPages()
        {
            // Arrange
            var title = "Documento de Teste";
            var workflowId = Guid.NewGuid();
            var pages = new List<string> { "Página 1", "Página 2", "Página 3" };

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
        public async Task ExtractTextAsync_Should_ExtractPagesFromPDF()
        {
            // Arrange
            string pdfPath = TestFileHelper.GetTestFilePath("sample.pdf");
            Assert.True(File.Exists(pdfPath), "PDF test file not found.");

            using FileStream stream = File.OpenRead(pdfPath);
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.OpenReadStream()).Returns(stream);

            // Act
            var result = await _adapter.ExtractTextAsync(formFileMock.Object);

            // Assert
            Assert.NotEmpty(result.Pages);
        }
    }
}
