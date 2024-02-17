using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MODiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Model_Wallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WalletId",
                table: "ServerMembers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<string>(type: "text", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerMembers_WalletId",
                table: "ServerMembers",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerMembers_Wallet_WalletId",
                table: "ServerMembers",
                column: "WalletId",
                principalTable: "Wallet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerMembers_Wallet_WalletId",
                table: "ServerMembers");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_ServerMembers_WalletId",
                table: "ServerMembers");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "ServerMembers");
        }
    }
}
