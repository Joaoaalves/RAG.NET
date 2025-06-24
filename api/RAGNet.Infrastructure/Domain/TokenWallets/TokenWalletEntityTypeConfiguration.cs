using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.TokenWallets;

namespace RAGNET.Infrastructure.Domain.TokenWallets
{
    internal sealed class TokenWalletTypeConfiguration : IEntityTypeConfiguration<TokenWallet>
    {
        public void Configure(EntityTypeBuilder<TokenWallet> builder)
        {
            builder.ToTable("TokenWallets");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.UserId).IsRequired();

            builder.Property(w => w.LastFreeTokenResetAt).IsRequired();

            builder.OwnsOne(w => w.FreeTokens, tokens =>
            {
                tokens.Property(t => t.Value)
                      .HasColumnName("FreeTokens")
                      .IsRequired();
            });

            builder.OwnsOne(w => w.PaidTokens, tokens =>
            {
                tokens.Property(t => t.Value)
                      .HasColumnName("PaidTokens")
                      .IsRequired();
            });

            builder.Navigation(w => w.Transactions)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(w => w.Transactions)
                .WithOne()
                .HasForeignKey("TokenWalletId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
