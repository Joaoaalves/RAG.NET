using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Infrastructure.Domain.QueryEnhancers
{
    internal sealed class QueryEnhancerEntityTypeConfiguration : IEntityTypeConfiguration<QueryEnhancer>
    {
        public void Configure(EntityTypeBuilder<QueryEnhancer> builder)
        {
            builder.ToTable("QueryEnhancers");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Type).IsRequired();
            builder.Property(q => q.WorkflowId)
            .HasColumnName("WorkflowId")
            .IsRequired();

            builder.Property(q => q.UserId).HasMaxLength(100).IsRequired();
            builder.Property(q => q.Prompt).HasMaxLength(2000);
            builder.Property(q => q.MaxQueries).IsRequired();
            builder.Property(q => q.IsEnabled).IsRequired();

            builder.OwnsMany(q => q.Metas, meta =>
            {
                meta.WithOwner().HasForeignKey("QueryEnhancerId");
                meta.ToTable("QueryEnhancerMetas");

                meta.Property(m => m.Key).IsRequired().HasMaxLength(100);
                meta.Property(m => m.Value).HasMaxLength(1000);
            });
        }
    }

}