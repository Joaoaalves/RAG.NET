using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEnabledToQueryEnhancer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "QueryEnhancers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "QueryEnhancers");
        }
    }
}
