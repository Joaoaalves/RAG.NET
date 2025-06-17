using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.Filters;
using RAGNET.Domain.Workflows;

namespace RAGNET.Infrastructure.Domain.Filters
{
    internal sealed class FilterEntityTypeConfiguration : IEntityTypeConfiguration<Filter>
    {
        public void Configure(EntityTypeBuilder<Filter> builder)
        {
            builder.ToTable("Filters");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder.Property(f => f.Strategy).IsRequired();
            builder.Property(f => f.WorkflowId)
            .HasColumnName("WorkflowId")
            .IsRequired();

            builder.Property(f => f.UserId)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(f => f.MaxItems)
                .IsRequired()
                .HasDefaultValue(5);

            builder.Property(f => f.IsEnabled)
                .IsRequired()
                .HasDefaultValue(true);

            builder.OwnsMany(f => f.Metas, meta =>
            {
                meta.WithOwner().HasForeignKey("FilterId");
                meta.ToTable("FilterMetas");

                meta.Property(m => m.Key)
                    .IsRequired()
                    .HasMaxLength(100);
                meta.Property(m => m.Value)
                    .HasMaxLength(1000);
            });
        }
    }
}