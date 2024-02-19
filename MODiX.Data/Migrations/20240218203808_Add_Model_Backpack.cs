using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MODiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Model_Backpack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BackpackId",
                table: "ServerMembers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Backpack",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backpack", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BackpackId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BackpackId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Backpack_BackpackId1",
                        column: x => x.BackpackId1,
                        principalTable: "Backpack",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerMembers_BackpackId",
                table: "ServerMembers",
                column: "BackpackId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_BackpackId1",
                table: "Item",
                column: "BackpackId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerMembers_Backpack_BackpackId",
                table: "ServerMembers",
                column: "BackpackId",
                principalTable: "Backpack",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
