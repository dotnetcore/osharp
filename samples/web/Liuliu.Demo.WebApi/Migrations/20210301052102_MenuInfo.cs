using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Liuliu.Demo.Web.Migrations
{
    public partial class MenuInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Systems_MenuInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Target = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Acl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OrderCode = table.Column<double>(type: "REAL", nullable: false),
                    TreePathString = table.Column<string>(type: "TEXT", nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_MenuInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_MenuInfo_Systems_MenuInfo_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Systems_MenuInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Systems_MenuInfo_ParentId",
                table: "Systems_MenuInfo",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Systems_MenuInfo");
        }
    }
}
