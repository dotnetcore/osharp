using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OSharp.Demo.Web.Migrations
{
    public partial class AddIdentityIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserToken_UserId_LoginProvider_Name",
                table: "UserToken");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserRole_UserId_RoleId",
                table: "UserRole");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserLogin_LoginProvider_ProviderKey",
                table: "UserLogin");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserToken",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserToken",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "UserLogin",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserLogin",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "UserTokenIndex",
                table: "UserToken",
                columns: new[] { "UserId", "LoginProvider", "Name" },
                unique: true,
                filter: "[LoginProvider] IS NOT NULL AND [Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserRoleIndex",
                table: "UserRole",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserLoginIndex",
                table: "UserLogin",
                columns: new[] { "LoginProvider", "ProviderKey" },
                unique: true,
                filter: "[LoginProvider] IS NOT NULL AND [ProviderKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserTokenIndex",
                table: "UserToken");

            migrationBuilder.DropIndex(
                name: "UserRoleIndex",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "UserLoginIndex",
                table: "UserLogin");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserToken",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserToken",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "UserLogin",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserLogin",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserToken_UserId_LoginProvider_Name",
                table: "UserToken",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserRole_UserId_RoleId",
                table: "UserRole",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserLogin_LoginProvider_ProviderKey",
                table: "UserLogin",
                columns: new[] { "LoginProvider", "ProviderKey" });
        }
    }
}
