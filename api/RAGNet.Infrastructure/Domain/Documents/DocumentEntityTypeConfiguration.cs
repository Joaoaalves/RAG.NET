using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RAGNET.Domain.Documents;

using RAGNET.Infrastructure.SeedWork;

namespace RAGNET.Infrastructure.Domain.Documents
{
    internal sealed class DocumentTypeConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            builder.ToTable("Documents");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.Title)
                .HasConversion(new TextValueConverter())
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(d => d.WorkflowId)
                .IsRequired();

            builder.HasMany(d => d.Pages)
                .WithOne(p => p.Document)
                .HasForeignKey(p => p.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}