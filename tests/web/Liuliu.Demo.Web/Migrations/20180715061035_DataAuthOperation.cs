using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Liuliu.Demo.Web.Migrations
{
    public partial class DataAuthOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EntityRoleIndex",
                table: "EntityRole");

            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("1a972119-cf36-4314-8ddd-a91300538f04"));

            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("4b2f3f7f-874e-4f85-82a1-a91300538f00"));

            migrationBuilder.AddColumn<int>(
                name: "Operation",
                table: "EntityRole",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("426b3368-9841-4e17-85c8-a91e00e99f43"), false, "Site.Name", "\"OSHARP\"", "System.String" });

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("9d176145-6058-4b7f-868d-a91e00e99f47"), false, "Site.Description", "\"Osharp with .NetStandard2.0 & Angular6\"", "System.String" });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime" },
                values: new object[] { "5f8e54eb-15bf-4c6b-b5b7-ce51c926bf60", new DateTime(2018, 7, 15, 14, 10, 35, 578, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "EntityRoleIndex",
                table: "EntityRole",
                columns: new[] { "EntityId", "RoleId", "Operation" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EntityRoleIndex",
                table: "EntityRole");

            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("426b3368-9841-4e17-85c8-a91e00e99f43"));

            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("9d176145-6058-4b7f-868d-a91e00e99f47"));

            migrationBuilder.DropColumn(
                name: "Operation",
                table: "EntityRole");

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("4b2f3f7f-874e-4f85-82a1-a91300538f00"), false, "Site.Name", "\"OSHARP\"", "System.String" });

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("1a972119-cf36-4314-8ddd-a91300538f04"), false, "Site.Description", "\"Osharp with .NetStandard2.0 & Angular6\"", "System.String" });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime" },
                values: new object[] { "b2194acf-2bfd-46e4-b2ea-1be822ac3052", new DateTime(2018, 7, 4, 5, 4, 13, 700, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "EntityRoleIndex",
                table: "EntityRole",
                columns: new[] { "EntityId", "RoleId" },
                unique: true);
        }
    }
}
