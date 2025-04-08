using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentsAndPagesForParentChildQueryRetrieval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chunks_Workflows_WorkflowId",
                table: "Chunks");

            migrationBuilder.DropIndex(
                name: "IX_Chunks_WorkflowId",
                table: "Chunks");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Chunks");

            migrationBuilder.RenameColumn(
                name: "Documents",
                table: "Workflows",
                newName: "DocumentsCount");

            migrationBuilder.AddColumn<Guid>(
                name: "PageId",
                table: "Chunks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "SourceDocument",
                table: "Chunks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chunks_PageId",
                table: "Chunks",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_WorkflowId",
                table: "Documents",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_DocumentId",
                table: "Pages",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chunks_Pages_PageId",
                table: "Chunks",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chunks_Pages_PageId",
                table: "Chunks");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Chunks_PageId",
                table: "Chunks");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "Chunks");

            migrationBuilder.DropColumn(
                name: "SourceDocument",
                table: "Chunks");

            migrationBuilder.RenameColumn(
                name: "DocumentsCount",
                table: "Workflows",
                newName: "Documents");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowId",
                table: "Chunks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chunks_WorkflowId",
                table: "Chunks",
                column: "WorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chunks_Workflows_WorkflowId",
                table: "Chunks",
                column: "WorkflowId",
                principalTable: "Workflows",
                principalColumn: "Id");
        }
    }
}
