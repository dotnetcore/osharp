using Microsoft.EntityFrameworkCore.Migrations;

namespace Liuliu.Demo.Web.Migrations
{
    public partial class Menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Systems_Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Target = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Acl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OrderCode = table.Column<double>(type: "REAL", nullable: false),
                    Data = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TreePathString = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSystem = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_Menu_Systems_Menu_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Systems_Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Systems_Menu_ParentId",
                table: "Systems_Menu",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Systems_Menu");
        }
    }
}
