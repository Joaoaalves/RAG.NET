using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGNET.Domain.SharedKernel.URLs;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Infrastructure.Domain.Workflows.CallbackUrls
{
    internal sealed class CallbackUrlEntityTypeConfiguration : IEntityTypeConfiguration<CallbackUrl>
    {
        public void Configure(EntityTypeBuilder<CallbackUrl> builder)
        {
            builder.ToTable("CallbackUrls");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(c => c.Url)
                .HasConversion(
                    url => url.Value,
                    value => URL.Create(value))
                .HasColumnName("url")
                .IsRequired();

            builder.Property(c => c.WorkflowId)
                .HasColumnName("workflow_id")
                .IsRequired();

            builder.HasOne<Workflow>()
                .WithMany(w => w.CallbackUrls)
                .HasForeignKey(c => c.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}