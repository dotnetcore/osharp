using Microsoft.EntityFrameworkCore.Migrations;

namespace OSharp.CodeGenerator.Migrations
{
    public partial class RootPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RootPath",
                table: "CodeGen_CodeProject",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RootPath",
                table: "CodeGen_CodeProject");
        }
    }
}
