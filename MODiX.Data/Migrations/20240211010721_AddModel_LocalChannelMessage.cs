using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MODiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModel_LocalChannelMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalChannelMessage");

            migrationBuilder.AlterColumn<long[]>(
                name: "RoleIds",
                table: "ServerMembers",
                type: "bigint[]",
                nullable: false,
                oldClrType: typeof(int[]),
                oldType: "integer[]");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<string>(type: "text", nullable: true),
                    ServerId = table.Column<string>(type: "text", nullable: true),
                    MessageContent = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LocalServerMemberId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_ServerMembers_LocalServerMemberId",
                        column: x => x.LocalServerMemberId,
                        principalTable: "ServerMembers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_LocalServerMemberId",
                table: "Messages",
                column: "LocalServerMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.AlterColumn<int[]>(
                name: "RoleIds",
                table: "ServerMembers",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(long[]),
                oldType: "bigint[]");

            migrationBuilder.CreateTable(
                name: "LocalChannelMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<string>(type: "text", nullable: true),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LocalServerMemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    MessageContent = table.Column<string>(type: "text", nullable: true),
                    ServerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalChannelMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalChannelMessage_ServerMembers_LocalServerMemberId",
                        column: x => x.LocalServerMemberId,
                        principalTable: "ServerMembers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocalChannelMessage_LocalServerMemberId",
                table: "LocalChannelMessage",
                column: "LocalServerMemberId");
        }
    }
}
