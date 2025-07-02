using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Users;
using RAGNET.Domain.Users.Subscriptions;

namespace RAGNET.Infrastructure.Domain.Users
{
    internal sealed class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.CustomerId).IsRequired();

            builder.HasOne(u => u.TokenWallet)
                   .WithOne()
                   .HasForeignKey<TokenWallet>(w => w.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Subscription)
                    .WithOne()
                    .HasForeignKey<Subscription>(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.ApiKeys)
                    .WithOne()
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
