using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.VectorStorages;

using RAGNET.Infrastructure.SeedWork;

namespace RAGNET.Infrastructure.Domain.VectorStorages
{
    internal sealed class VectorStorageEntityTypeConfiguration : IEntityTypeConfiguration<VectorStorage>
    {
        public void Configure(EntityTypeBuilder<VectorStorage> builder)
        {
            builder.ToTable("VectorStorages");

            builder.HasKey(v => v.Id);
            builder.Property(v => v.UserId)
                .IsRequired();

            builder.Property(v => v.IsActive).IsRequired();
            builder.Property(v => v.Provider).IsRequired();

            builder.Property(v => v.ApiKey)
                .HasConversion(new ApiKeyConverter())
                .IsRequired();

            builder.OwnsMany(c => c.Metas, meta =>
            {
                meta.WithOwner().HasForeignKey("VectorStorageId");
                meta.ToTable("VectorStorageMetas");

                meta.HasKey("VectorStorageId", "Key");

                meta.Property(m => m.Key)
                    .IsRequired()
                    .HasMaxLength(100);

                meta.Property(m => m.Value)
                    .HasMaxLength(1000);
            });
        }
    }
}