using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using RAGNET.Domain.Entities;

namespace RAGNET.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<Chunker> Chunkers { get; set; }
        public DbSet<ChunkerMeta> ChunkerMetas { get; set; }
        public DbSet<EmbeddingProviderConfig> EmbeddingProviderConfigs { get; set; }
        public DbSet<ConversationProviderConfig> ConversationProviderConfigs { get; set; }
        public DbSet<QueryEnhancer> QueryEnhancers { get; set; }
        public DbSet<QueryEnhancerMeta> QueryEnhancerMetas { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<FilterMeta> FilterMetas { get; set; }
        public DbSet<Ranker> Rankers { get; set; }
        public DbSet<RankerMeta> RankerMetas { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Chunk> Chunks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relationship: Workflow -> Chunkers
            builder.Entity<Workflow>()
                        .HasOne(w => w.Chunker)
                        .WithOne(c => c.Workflow)
                        .HasForeignKey<Chunker>(c => c.WorkflowId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Workflow -> QueryEnhancers
            builder.Entity<Workflow>()
                        .HasMany(w => w.QueryEnhancers)
                        .WithOne(q => q.Workflow)
                        .HasForeignKey(q => q.WorkflowId)
                        .OnDelete(DeleteBehavior.Cascade);


            // Relationship: Workflow -> Rankers
            builder.Entity<Workflow>()
                        .HasMany(w => w.Rankers)
                        .WithOne(r => r.Workflow)
                        .HasForeignKey(r => r.WorkflowId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Workflow -> Documents
            builder.Entity<Workflow>()
                        .HasMany(w => w.Documents)
                        .WithOne(d => d.Workflow)
                        .HasForeignKey(d => d.WorkflowId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Chunker -> ChunkerMetas
            builder.Entity<ChunkerMeta>()
                        .HasOne(cm => cm.Chunker)
                        .WithMany(c => c.Metas)
                        .HasForeignKey(cm => cm.ChunkerId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Chunker -> EmbeddingProvider
            builder.Entity<Workflow>()
                .HasOne(w => w.EmbeddingProviderConfig)
                .WithOne(e => e.Workflow)
                .HasForeignKey<EmbeddingProviderConfig>(e => e.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Chunker -> ConversationProvider
            builder.Entity<Workflow>()
                .HasOne(w => w.ConversationProviderConfig)
                .WithOne(c => c.Workflow)
                .HasForeignKey<ConversationProviderConfig>(c => c.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: QueryEnhancer -> QueryEnhancerMetas
            builder.Entity<QueryEnhancerMeta>()
                        .HasOne(qm => qm.QueryEnhancer)
                        .WithMany(q => q.Metas)
                        .HasForeignKey(qm => qm.QueryEnhancerId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Workflow -> Filter
            builder.Entity<Workflow>()
                        .HasOne(w => w.Filter)
                        .WithOne(f => f.Workflow)
                        .HasForeignKey<Filter>(f => f.WorkflowId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Filter -> FilterMetas
            builder.Entity<FilterMeta>()
                        .HasOne(fm => fm.Filter)
                        .WithMany(f => f.Metas)
                        .HasForeignKey(fm => fm.FilterId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Ranker -> RankerMetas
            builder.Entity<RankerMeta>()
                        .HasOne(rm => rm.Ranker)
                        .WithMany(r => r.Metas)
                        .HasForeignKey(rm => rm.RankerId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Document -> Pages
            builder.Entity<Document>()
                        .HasMany(d => d.Pages)
                        .WithOne(p => p.Document)
                        .HasForeignKey(p => p.DocumentId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Page -> Chunks
            builder.Entity<Page>()
                        .HasMany(p => p.Chunks)
                        .WithOne(c => c.Page)
                        .HasForeignKey(c => c.PageId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
