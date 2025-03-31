using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionAndDocumentCounterToWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Workflows",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Documents",
                table: "Workflows",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Workflows");

            migrationBuilder.DropColumn(
                name: "Documents",
                table: "Workflows");
        }
    }
}
