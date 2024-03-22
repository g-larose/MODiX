using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MODiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocalServerMemberId",
                table: "Bank",
                newName: "ServerMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServerMemberId",
                table: "Bank",
                newName: "LocalServerMemberId");
        }
    }
}
