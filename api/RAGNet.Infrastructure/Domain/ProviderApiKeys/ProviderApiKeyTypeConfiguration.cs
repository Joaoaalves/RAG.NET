using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.Entities;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.Domain.ProviderApiKeys;

internal sealed class ProviderApiKeyEntityTypeConfiguration : IEntityTypeConfiguration<ProviderApiKey>
{
    public void Configure(EntityTypeBuilder<ProviderApiKey> builder)
    {
        builder.ToTable("ProviderApiKeys", "AspNetUsers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.OwnsOne(x => x.Provider, provider =>
        {
            provider.Property(p => p.Type)
                .HasColumnName("Provider")
                .HasConversion<string>()
                .IsRequired();

            provider.OwnsOne(p => p.ApiKey, apiKey =>
            {
                apiKey.Property(a => a.Value)
                    .HasColumnName("ApiKey")
                    .HasMaxLength(300)
                    .IsRequired();
            });
        });
    }
}
