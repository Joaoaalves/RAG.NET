using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChunksToWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
