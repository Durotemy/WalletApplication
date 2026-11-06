using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApi.Migrations
{
    /// <inheritdoc />
    public partial class LedgerEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_userId",
                table: "Wallet");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Wallet",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "balance",
                table: "Wallet",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Wallet",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Wallet_userId",
                table: "Wallet",
                newName: "IX_Wallet_UserId");

            migrationBuilder.CreateTable(
                name: "LedgersEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    EntryType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgersEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LedgersEntries_Wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LedgersEntries_WalletId",
                table: "LedgersEntries",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_UserId",
                table: "Wallet",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_UserId",
                table: "Wallet");

            migrationBuilder.DropTable(
                name: "LedgersEntries");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Wallet",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Wallet",
                newName: "balance");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Wallet",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet",
                newName: "IX_Wallet_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_userId",
                table: "Wallet",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
