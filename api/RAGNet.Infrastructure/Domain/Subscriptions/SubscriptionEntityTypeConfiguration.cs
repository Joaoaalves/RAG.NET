using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.Users.Subscriptions;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Infrastructure.Domain.Subscriptions
{
    internal sealed class SubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.UserId).IsRequired();

            builder.Property(s => s.Plan)
                .IsRequired()
                .HasConversion(
                    plan => plan.Value.ToString(),
                    planString => SubscriptionPlan.FromType(
                        Enum.Parse<PlanType>(planString!)
                    )
                )
                .HasColumnName("Plan");

            builder.Property(s => s.PaymentId)
                .HasMaxLength(100)
                .IsRequired(false)
                .HasColumnName("PaymentId");

            builder.Property(s => s.SubscribedAt)
                .IsRequired()
                .HasColumnName("SubscribedAt");

            builder.Property(s => s.ExpiresAt)
                .IsRequired()
                .HasColumnName("ExpiresAt");
        }
    }
}
