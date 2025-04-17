using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKeySuffixToApiKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeySuffix",
                table: "UserApiKeys",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeySuffix",
                table: "UserApiKeys");
        }
    }
}
