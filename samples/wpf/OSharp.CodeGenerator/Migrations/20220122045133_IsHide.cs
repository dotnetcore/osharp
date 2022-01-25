using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OSharp.CodeGenerator.Migrations
{
    public partial class IsHide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHide",
                table: "CodeGen_CodeProperty",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHide",
                table: "CodeGen_CodeProperty");
        }
    }
}
