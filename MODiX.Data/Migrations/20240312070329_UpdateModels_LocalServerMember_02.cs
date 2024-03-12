using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MODiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels_LocalServerMember_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerMembers_Backpack_BackpackId",
                table: "ServerMembers");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Backpack");

            migrationBuilder.DropIndex(
                name: "IX_ServerMembers_BackpackId",
                table: "ServerMembers");

            migrationBuilder.DropColumn(
                name: "BackpackId",
                table: "ServerMembers");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BackpackId",
                table: "ServerMembers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "ServerMembers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Backpack",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    ServerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backpack", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountTotal = table.Column<double>(type: "double precision", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    ServerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BackpackId = table.Column<Guid>(type: "uuid", nullable: true),
                    Cost = table.Column<double>(type: "double precision", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Backpack_BackpackId",
                        column: x => x.BackpackId,
                        principalTable: "Backpack",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerMembers_BackpackId",
                table: "ServerMembers",
                column: "BackpackId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerMembers_BankId",
                table: "ServerMembers",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_BackpackId",
                table: "Item",
                column: "BackpackId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerMembers_Backpack_BackpackId",
                table: "ServerMembers",
                column: "BackpackId",
                principalTable: "Backpack",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerMembers_Bank_BankId",
                table: "ServerMembers",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id");
        }
    }
}
