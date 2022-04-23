using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingAppAPI.Data.Migrations
{
    public partial class renamepropertydatareadtodateread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataRead",
                table: "Messages",
                newName: "DateRead");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateRead",
                table: "Messages",
                newName: "DataRead");
        }
    }
}
