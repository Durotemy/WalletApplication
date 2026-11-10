using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTransfers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TransferId",
                table: "LedgerEntries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.CheckConstraint("CK_Transfer_Amount_Positive", "\"Amount\" > 0");
                    table.CheckConstraint("CK_Transfer_NotSelf", "\"FromWalletId\" <> \"ToWalletId\"");
                    table.ForeignKey(
                        name: "FK_Transfers_Wallets_FromWalletId",
                        column: x => x.FromWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Wallets_ToWalletId",
                        column: x => x.ToWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Wallet_Balance_NonNegative",
                table: "Wallets",
                sql: "\"Balance\" >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_TransferId",
                table: "LedgerEntries",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_CreatedAt",
                table: "Transfers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_FromWalletId",
                table: "Transfers",
                column: "FromWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ToWalletId",
                table: "Transfers",
                column: "ToWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Transfers_TransferId",
                table: "LedgerEntries",
                column: "TransferId",
                principalTable: "Transfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Transfers_TransferId",
                table: "LedgerEntries");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Wallet_Balance_NonNegative",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntries_TransferId",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "TransferId",
                table: "LedgerEntries");
        }
    }
}
