using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixChunkerMeta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChunkerMetas",
                table: "ChunkerMetas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ChunkerMetas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChunkerMetas",
                table: "ChunkerMetas",
                columns: new[] { "ChunkerId", "Key" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChunkerMetas",
                table: "ChunkerMetas");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ChunkerMetas",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChunkerMetas",
                table: "ChunkerMetas",
                columns: new[] { "ChunkerId", "Id" });
        }
    }
}
