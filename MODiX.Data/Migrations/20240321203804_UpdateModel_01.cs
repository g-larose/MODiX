using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MODiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerMembers_BankId",
                table: "ServerMembers");

            migrationBuilder.CreateIndex(
                name: "IX_ServerMembers_BankId",
                table: "ServerMembers",
                column: "BankId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerMembers_BankId",
                table: "ServerMembers");

            migrationBuilder.CreateIndex(
                name: "IX_ServerMembers_BankId",
                table: "ServerMembers",
                column: "BankId");
        }
    }
}
