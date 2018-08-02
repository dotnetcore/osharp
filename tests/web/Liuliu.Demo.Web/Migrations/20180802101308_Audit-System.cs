using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Liuliu.Demo.Web.Migrations
{
    public partial class AuditSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("214c9d18-f463-4c0b-87ea-a930004e35ff"));

            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("50ae37f0-244e-431b-ad8f-a930004e3604"));

            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "AuditOperation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperationSystem",
                table: "AuditOperation",
                nullable: true);

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("6747ea95-b2f6-4fed-ba6f-a930012c3cef"), false, "Site.Name", "\"OSHARP\"", "System.String" });

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("b8e53ff4-4ac8-4153-a7e3-a930012c3cf3"), false, "Site.Description", "\"Osharp with .NetStandard2.0 & Angular6\"", "System.String" });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime" },
                values: new object[] { "acdfd9b8-1f6a-4f24-9f08-fe2537edf5d2", new DateTime(2018, 8, 2, 18, 13, 8, 34, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("6747ea95-b2f6-4fed-ba6f-a930012c3cef"));

            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("b8e53ff4-4ac8-4153-a7e3-a930012c3cf3"));

            migrationBuilder.DropColumn(
                name: "Browser",
                table: "AuditOperation");

            migrationBuilder.DropColumn(
                name: "OperationSystem",
                table: "AuditOperation");

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("214c9d18-f463-4c0b-87ea-a930004e35ff"), false, "Site.Name", "\"OSHARP\"", "System.String" });

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("50ae37f0-244e-431b-ad8f-a930004e3604"), false, "Site.Description", "\"Osharp with .NetStandard2.0 & Angular6\"", "System.String" });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime" },
                values: new object[] { "983e86bf-81a0-4f4c-afc5-c3085b995a32", new DateTime(2018, 8, 2, 4, 44, 45, 481, DateTimeKind.Local) });
        }
    }
}
