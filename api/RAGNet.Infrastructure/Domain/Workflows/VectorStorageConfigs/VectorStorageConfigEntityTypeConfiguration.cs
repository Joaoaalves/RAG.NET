using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RAGNET.Domain.Workflows.VectorStorageConfigs;

namespace RAGNET.Infrastructure.Domain.Workflows.VectorStorageConfigs
{
    internal sealed class VectorStorageConfigEntityTypeConfiguration : IEntityTypeConfiguration<VectorStorageConfig>
    {
        public void Configure(EntityTypeBuilder<VectorStorageConfig> builder)
        {
            builder.ToTable("VectorStorageConfigs");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.CollectionId).IsRequired();
            builder.Property(v => v.VectorDimension).IsRequired();

            builder.Property(v => v.WorkflowId)
                .HasColumnName("WorkflowId")
                .IsRequired();

            builder.Property(v => v.VectorStorageId)
                .HasColumnName("VectorStorageId")
                .IsRequired();

            builder.HasOne(v => v.VectorStorage)
                .WithOne()
                .HasForeignKey<VectorStorageConfig>(v => v.VectorStorageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}