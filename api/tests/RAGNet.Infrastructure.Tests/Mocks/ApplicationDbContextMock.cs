using Microsoft.EntityFrameworkCore;
using Moq;
using RAGNET.Infrastructure.Data;
using RAGNET.Domain.Entities;

namespace tests.RAGNet.Infrastructure.Tests.Mocks
{
    public class ApplicationDbContextMock
    {
        public static Mock<ApplicationDbContext> GetDbContextMock()
        {
            var dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            var dbSetMock = new Mock<DbSet<Chunker>>();
            dbContextMock.Setup(x => x.Set<Chunker>()).Returns(dbSetMock.Object);

            return dbContextMock;
        }
    }
}
