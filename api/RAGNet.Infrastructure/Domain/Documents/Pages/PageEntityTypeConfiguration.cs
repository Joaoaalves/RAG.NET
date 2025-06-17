using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Infrastructure.SeedWork;

namespace RAGNET.Infrastructure.Domain.Documents.Pages
{
    internal sealed class PageEntityTypeConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.ToTable("Pages");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(p => p.DocumentId)
                .HasColumnName("DocumentId")
                .IsRequired();

            builder.Property(p => p.Text)
                .HasColumnName("Text")
                .HasConversion(new TextValueConverter())
                .IsRequired()
                .HasMaxLength(10000);


            builder.HasMany(p => p.Chunks)
                .WithOne(c => c.Page)
                .HasForeignKey(c => c.PageId);
        }
    }
}