using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.TokenWallets.TokenTransactions;

namespace RAGNET.Infrastructure.Domain.TokenWallets.TokenTransactions
{
    internal sealed class TokenTransactionTypeConfiguration : IEntityTypeConfiguration<TokenTransaction>
    {
        public void Configure(EntityTypeBuilder<TokenTransaction> builder)
        {
            builder.ToTable("TokenTransactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId).IsRequired();
            builder.Property(t => t.OperationName).IsRequired();
            builder.Property(t => t.ContextInfo).IsRequired();
            builder.Property(t => t.TimeStamp).IsRequired();
            builder.Property(t => t.Source).IsRequired();

            builder.HasOne(t => t.TokenWallet)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(t => t.TokenWalletId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(t => t.Cost, cost =>
            {
                cost.Property(c => c.Value)
                    .HasColumnName("Cost")
                    .IsRequired();
            });
        }
    }
}
