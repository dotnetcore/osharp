using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OSharp.CodeGenerator.Migrations
{
    public partial class IsEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnum",
                table: "CodeGen_CodeProperty",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnum",
                table: "CodeGen_CodeProperty");
        }
    }
}
