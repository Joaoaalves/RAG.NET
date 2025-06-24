using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAGNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenWallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FreeTokens = table.Column<long>(type: "bigint", nullable: false),
                    PaidTokens = table.Column<long>(type: "bigint", nullable: false),
                    LastFreeTokenResetAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenWallets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TokenTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    OperationName = table.Column<string>(type: "text", nullable: false),
                    ContextInfo = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<long>(type: "bigint", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    TokenWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenWalletId1 = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenTransactions_TokenWallets_TokenWalletId",
                        column: x => x.TokenWalletId,
                        principalTable: "TokenWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TokenTransactions_TokenWallets_TokenWalletId1",
                        column: x => x.TokenWalletId1,
                        principalTable: "TokenWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_TokenWalletId",
                table: "TokenTransactions",
                column: "TokenWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_TokenWalletId1",
                table: "TokenTransactions",
                column: "TokenWalletId1");

            migrationBuilder.CreateIndex(
                name: "IX_TokenWallets_UserId",
                table: "TokenWallets",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenTransactions");

            migrationBuilder.DropTable(
                name: "TokenWallets");
        }
    }
}
