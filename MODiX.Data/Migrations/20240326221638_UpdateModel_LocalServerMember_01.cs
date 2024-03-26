using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MODiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel_LocalServerMember_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServerId",
                table: "Errors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServerName",
                table: "Errors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastDaily",
                table: "Bank",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "ServerName",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "LastDaily",
                table: "Bank");
        }
    }
}
