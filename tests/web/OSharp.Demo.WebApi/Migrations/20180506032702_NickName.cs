using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OSharp.Demo.WebApi.Migrations
{
    public partial class NickName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "UserDetail",
                newName: "RegisterIp");

            migrationBuilder.AddColumn<string>(
                name: "HeadImg",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    LogoutTime = table.Column<DateTime>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginLog_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginLog_UserId",
                table: "LoginLog",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginLog");

            migrationBuilder.DropColumn(
                name: "HeadImg",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "RegisterIp",
                table: "UserDetail",
                newName: "Address");
        }
    }
}
