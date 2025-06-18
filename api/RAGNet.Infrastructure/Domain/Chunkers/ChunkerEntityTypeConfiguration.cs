using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.Chunkers;

namespace RAGNET.Infrastructure.Domain.Chunkers
{
    internal sealed class ChunkerEntityTypeConfiguration : IEntityTypeConfiguration<Chunker>
    {
        public void Configure(EntityTypeBuilder<Chunker> builder)
        {
            builder.ToTable("Chunkers");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.StrategyType)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<ChunkerStrategy>(v));

            builder.Property(c => c.UserId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.WorkflowId)
                .HasColumnName("WorkflowId")
                .IsRequired();

            builder.OwnsMany(c => c.Metas, meta =>
            {
                meta.WithOwner().HasForeignKey("ChunkerId");
                meta.ToTable("ChunkerMetas");

                meta.Property(m => m.Key)
                    .IsRequired()
                    .HasMaxLength(100);

                meta.Property(m => m.Value)
                    .HasMaxLength(1000);
            });
        }
    }
}