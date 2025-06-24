using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenUsageToEachWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastEmbeddedDocumentDate",
                table: "Workflows",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastQueryDate",
                table: "Workflows",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TokenUsage",
                table: "Workflows",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEmbeddedDocumentDate",
                table: "Workflows");

            migrationBuilder.DropColumn(
                name: "LastQueryDate",
                table: "Workflows");

            migrationBuilder.DropColumn(
                name: "TokenUsage",
                table: "Workflows");
        }
    }
}
