using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.Rankers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Infrastructure.Domain.Rankers
{
    internal sealed class RankerTypeConfiguration : IEntityTypeConfiguration<Ranker>
    {
        public void Configure(EntityTypeBuilder<Ranker> builder)
        {
            builder.ToTable("Rankers");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.WorkflowId)
            .HasColumnName("WorkflowId")
            .IsRequired();

            builder.Property(r => r.UserId).HasMaxLength(100).IsRequired();
            builder.Property(r => r.IsEnabled).IsRequired();

            builder.OwnsMany(r => r.Metas, meta =>
            {
                meta.WithOwner().HasForeignKey("RankerId");
                meta.ToTable("RankerMetas");

                meta.Property(m => m.Key).IsRequired().HasMaxLength(100);
                meta.Property(m => m.Value).HasMaxLength(1000);
            });
        }
    }
}