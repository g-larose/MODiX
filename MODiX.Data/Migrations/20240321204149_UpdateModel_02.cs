using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MODiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerMembers_BankId",
                table: "ServerMembers");

            migrationBuilder.AddColumn<int>(
                name: "LocalServerMemberId",
                table: "Bank",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServerMembers_BankId",
                table: "ServerMembers",
                column: "BankId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerMembers_BankId",
                table: "ServerMembers");

            migrationBuilder.DropColumn(
                name: "LocalServerMemberId",
                table: "Bank");

            migrationBuilder.CreateIndex(
                name: "IX_ServerMembers_BankId",
                table: "ServerMembers",
                column: "BankId",
                unique: true);
        }
    }
}
