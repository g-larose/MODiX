using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MODiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Model_LocalServerMember_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Xp",
                table: "ServerMembers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Xp",
                table: "ServerMembers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
