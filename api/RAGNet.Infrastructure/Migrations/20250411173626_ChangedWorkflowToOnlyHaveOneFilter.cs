using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedWorkflowToOnlyHaveOneFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Filters_WorkflowId",
                table: "Filters");

            migrationBuilder.CreateIndex(
                name: "IX_Filters_WorkflowId",
                table: "Filters",
                column: "WorkflowId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Filters_WorkflowId",
                table: "Filters");

            migrationBuilder.CreateIndex(
                name: "IX_Filters_WorkflowId",
                table: "Filters",
                column: "WorkflowId");
        }
    }
}
