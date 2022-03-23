using Microsoft.EntityFrameworkCore.Migrations;

namespace OSharp.CodeGenerator.Migrations
{
    public partial class Icon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValueJson",
                table: "Systems_KeyValue",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Display",
                table: "Systems_KeyValue",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Systems_KeyValue",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Systems_KeyValue",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "CodeGen_CodeModule",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "CodeGen_CodeEntity",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Display",
                table: "Systems_KeyValue");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Systems_KeyValue");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Systems_KeyValue");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "CodeGen_CodeModule");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "CodeGen_CodeEntity");

            migrationBuilder.AlterColumn<string>(
                name: "ValueJson",
                table: "Systems_KeyValue",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
