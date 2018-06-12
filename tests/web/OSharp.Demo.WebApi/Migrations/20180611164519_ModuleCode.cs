using Microsoft.EntityFrameworkCore.Migrations;

namespace OSharp.Demo.WebApi.Migrations
{
    public partial class ModuleCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Module",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Module");
        }
    }
}
