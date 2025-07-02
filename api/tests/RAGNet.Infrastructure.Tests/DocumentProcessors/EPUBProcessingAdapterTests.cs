using Moq;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.DocumentProcessors;
using tests.Helpers;

namespace tests.RAGNet.Infrastructure.Tests.DocumentProcessors
{
    public class EpubProcessingAdapterTests
    {
        private readonly Mock<IDocumentRepository> _documentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly EpubProcessingAdapter _adapter;

        public EpubProcessingAdapterTests()
        {
            _documentRepositoryMock = new Mock<IDocumentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _adapter = new EpubProcessingAdapter(_documentRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CreateDocumentWithPagesAsync_Should_CreateDocumentAndAddPages()
        {
            // Arrange
            var title = "Documento EPUB de Teste";
            var workflowId = new WorkflowId(); ;
            var docId = new DocumentId();

            var pages = new List<string> { "Page 1", "Page 2" };

            var createdDocument = Document.Create(
                id: docId,
                title: new Text(title),
                workflowId: workflowId
            );

            _documentRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Document>()))
                .ReturnsAsync(createdDocument);

            // Act
            var document = await _adapter.CreateDocumentWithPagesAsync(createdDocument, pages);

            // Assert
            Assert.Equal(createdDocument.Id, document.Id);

            Assert.All(document.Pages, page => Assert.Contains(page.Text.Value, pages));

            _documentRepositoryMock.Verify(r => r.AddAsync(It.Is<Document>(d =>
                d.Title.Value == title && d.WorkflowId == workflowId)), Times.Once);
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
                Mock.Of<IUnitOfWork>()
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
