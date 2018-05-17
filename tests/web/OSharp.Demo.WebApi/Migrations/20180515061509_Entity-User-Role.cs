using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OSharp.Demo.WebApi.Migrations
{
    public partial class EntityUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    FilterGroupJson = table.Column<string>(nullable: true),
                    IsLocked = table.Column<bool>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityRole_EntityInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "EntityInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    FilterGroupJson = table.Column<string>(nullable: true),
                    IsLocked = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityUser_EntityInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "EntityInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityRole_RoleId",
                table: "EntityRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EntityRoleIndex",
                table: "EntityRole",
                columns: new[] { "EntityId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntityUser_UserId",
                table: "EntityUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EntityUserIndex",
                table: "EntityUser",
                columns: new[] { "EntityId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityRole");

            migrationBuilder.DropTable(
                name: "EntityUser");
        }
    }
}
