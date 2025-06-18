using Moq;
using tests.Helpers;

using RAGNET.Domain.Documents;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Infrastructure.DocumentProcessors;
using RAGNET.Domain.Workflows;


namespace tests.RAGNet.Infrastructure.Tests.Adapters
{
    public class PdfProcessingAdapterTests
    {
        private readonly Mock<IDocumentRepository> _documentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly PDFProcessingAdapter _adapter;

        public PdfProcessingAdapterTests()
        {
            _documentRepositoryMock = new Mock<IDocumentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _adapter = new PDFProcessingAdapter(_documentRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CreateDocumentWithPagesAsync_Should_CreateDocumentAndAddPages()
        {
            // Arrange
            var title = "Documento de Teste";
            var workflowId = new WorkflowId(Guid.NewGuid());
            var pages = new List<string> { "Page 1", "Page 2", "Page 3" };
            var docId = Guid.NewGuid();

            var createdDocument = Document.Create(
                id: docId,
                title: new Text(title),
                workflowId: workflowId,
                pages: pages.Select(p => Page.Create(new Text(p), docId)).ToList()
            );

            _documentRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Document>()))
                .ReturnsAsync(createdDocument);

            // Act
            var document = await _adapter.CreateDocumentWithPagesAsync(title, workflowId, pages);

            // Assert
            Assert.Equal(createdDocument.Id, document.Id);
            _documentRepositoryMock.Verify(r => r.AddAsync(It.Is<Document>(d =>
                d.Title.Value == title && d.WorkflowId == workflowId)), Times.Once);

        }

        [Fact]
        public async Task ExtractTextAsync_Should_ExtractPagesFromPDF()
        {
            // Arrange
            string pdfPath = TestFileHelper.GetTestFilePath("sample.pdf");
            Assert.True(File.Exists(pdfPath), "PDF test file not found.");

            using FileStream stream = File.OpenRead(pdfPath);

            // Act
            var result = await _adapter.ExtractTextAsync(stream);

            // Assert
            Assert.NotEmpty(result.Pages);
        }
    }
}
