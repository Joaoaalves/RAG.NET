using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.Entities;

namespace RAGNET.Infrastructure.Domain.ProviderApiKeys;

internal sealed class ProviderApiKeyEntityTypeConfiguration : IEntityTypeConfiguration<ProviderApiKey>
{
    public void Configure(EntityTypeBuilder<ProviderApiKey> builder)
    {
        builder.ToTable("ProviderApiKeys", "Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Provider)
            .HasConversion<string>()
            .HasColumnName("Provider")
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.OwnsOne(x => x.ApiKey, apiKey =>
        {
            apiKey.Property(p => p.Value)
                .HasColumnName("ApiKey")
                .HasMaxLength(300)
                .IsRequired();
        });
    }
}
