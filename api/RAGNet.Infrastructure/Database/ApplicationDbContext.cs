using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.Filters;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Rankers;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Infrastructure.Database
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<CallbackUrl> CallbackUrls { get; set; }
        public DbSet<ProviderApiKey> ProviderApiKeys { get; set; }
        public DbSet<Chunker> Chunkers { get; set; }
        public DbSet<QueryEnhancer> QueryEnhancers { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<Ranker> Rankers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Chunk> Chunks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
