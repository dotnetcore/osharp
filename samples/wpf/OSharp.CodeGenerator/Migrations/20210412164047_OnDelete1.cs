using Microsoft.EntityFrameworkCore.Migrations;

namespace OSharp.CodeGenerator.Migrations
{
    public partial class OnDelete1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeGen_CodeProjectTemplate_CodeGen_CodeTemplate_TemplateId",
                table: "CodeGen_CodeProjectTemplate");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeGen_CodeProjectTemplate_CodeGen_CodeTemplate_TemplateId",
                table: "CodeGen_CodeProjectTemplate",
                column: "TemplateId",
                principalTable: "CodeGen_CodeTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeGen_CodeProjectTemplate_CodeGen_CodeTemplate_TemplateId",
                table: "CodeGen_CodeProjectTemplate");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeGen_CodeProjectTemplate_CodeGen_CodeTemplate_TemplateId",
                table: "CodeGen_CodeProjectTemplate",
                column: "TemplateId",
                principalTable: "CodeGen_CodeTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
