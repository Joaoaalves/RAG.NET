using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.ProvidersApiKeys;

namespace RAGNET.Infrastructure.Domain.ProviderApiKeys
{
    internal sealed class ProviderApiKeyEntityTypeConfiguration : IEntityTypeConfiguration<ProviderApiKey>
    {
        public void Configure(EntityTypeBuilder<ProviderApiKey> builder)
        {
            builder.ToTable("ProviderApiKeys");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.OwnsOne(x => x.Provider, provider =>
            {
                provider.WithOwner().HasForeignKey("ProviderApiKeyId");
                provider.Ignore(p => p.Policy);

                provider.Property(p => p.ProviderType)
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
}

