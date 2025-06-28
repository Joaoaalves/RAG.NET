using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionIdToSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScheduledPlan",
                table: "Subscriptions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                table: "Subscriptions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduledPlan",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Subscriptions");
        }
    }
}
