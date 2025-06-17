using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Filters;
using RAGNET.Domain.Workflows;

namespace RAGNET.Infrastructure.Domain.Workflows
{
    internal sealed class WorkflowTypeConfiguration : IEntityTypeConfiguration<Workflow>
    {
        public void Configure(EntityTypeBuilder<Workflow> builder)
        {
            builder.ToTable("Workflows");

            builder.HasKey(w => w.Id);
            builder.Property(w => w.Name).IsRequired();
            builder.Property(w => w.Description);
            builder.Property(w => w.IsActive);
            builder.Property(w => w.UserId).IsRequired();
            builder.Property(w => w.ApiKey).IsRequired();
            builder.Property(w => w.CollectionId).IsRequired();

            builder.Navigation(w => w.CallbackUrls)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(w => w.CallbackUrls)
                .WithOne()
                .HasForeignKey("WorkflowId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Chunker)
                .WithOne(c => c.Workflow)
                .HasForeignKey<Chunker>(c => c.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(w => w.QueryEnhancers)
                .WithOne(q => q.Workflow)
                .HasForeignKey(q => q.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(w => w.Rankers)
                .WithOne(r => r.Workflow)
                .HasForeignKey(r => r.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(w => w.Documents)
                .WithOne(d => d.Workflow)
                .HasForeignKey(d => d.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(w => w.ConversationProviderConfig, cfg =>
            {
                cfg.Property(c => c.Provider).HasColumnName("ConversationProvider").IsRequired();
                cfg.Property(c => c.Model).HasColumnName("ConversationModel").IsRequired();
            });

            builder.OwnsOne(w => w.EmbeddingProviderConfig, cfg =>
            {
                cfg.Property(c => c.Provider).HasColumnName("EmbeddingProvider").IsRequired();
                cfg.Property(c => c.Model).HasColumnName("EmbeddingModel").IsRequired();
                cfg.Property(c => c.VectorSize).HasColumnName("VectorSize").IsRequired();
            });

            builder.HasOne(w => w.Filter)
                .WithOne(f => f.Workflow)
                .HasForeignKey<Filter>(f => f.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

