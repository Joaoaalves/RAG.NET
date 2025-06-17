using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Infrastructure.SeedWork;

namespace RAGNET.Infrastructure.Domain.Documents.Pages.Chunks
{
    internal sealed class ChunkEntityTypeConfiguration : IEntityTypeConfiguration<Chunk>
    {
        public void Configure(EntityTypeBuilder<Chunk> builder)
        {
            builder.ToTable("Chunks");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(c => c.Text)
                .HasConversion(new TextValueConverter())
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(c => c.VectorId)
                .HasColumnName("VectorId")
                .IsRequired();

            builder.Property(c => c.PageId)
                .HasColumnName("PageId")
                .IsRequired();

            builder.HasOne(c => c.Page)
                .WithMany(p => p.Chunks)
                .HasForeignKey(c => c.PageId);
        }
    }
}