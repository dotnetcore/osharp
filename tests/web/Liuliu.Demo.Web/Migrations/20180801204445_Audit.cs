using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Liuliu.Demo.Web.Migrations
{
    public partial class Audit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("a2b1305e-525f-4c6d-bc36-a92e0175a928"));

            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("cd9e1efe-8cc3-40fa-a781-a92e0175a924"));

            migrationBuilder.CreateTable(
                name: "AuditOperation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FunctionName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditOperation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TypeName = table.Column<string>(nullable: true),
                    EntityKey = table.Column<string>(nullable: true),
                    OperateType = table.Column<int>(nullable: false),
                    OperationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditEntity_AuditOperation_OperationId",
                        column: x => x.OperationId,
                        principalTable: "AuditOperation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    FieldName = table.Column<string>(nullable: true),
                    OriginalValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    AuditEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditProperty_AuditEntity_AuditEntityId",
                        column: x => x.AuditEntityId,
                        principalTable: "AuditEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntity_OperationId",
                table: "AuditEntity",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProperty_AuditEntityId",
                table: "AuditProperty",
                column: "AuditEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditProperty");

            migrationBuilder.DropTable(
                name: "AuditEntity");

            migrationBuilder.DropTable(
                name: "AuditOperation");

            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("214c9d18-f463-4c0b-87ea-a930004e35ff"));

            migrationBuilder.DeleteData(
                table: "KeyValueCouple",
                keyColumn: "Id",
                keyValue: new Guid("50ae37f0-244e-431b-ad8f-a930004e3604"));

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("cd9e1efe-8cc3-40fa-a781-a92e0175a924"), false, "Site.Name", "\"OSHARP\"", "System.String" });

            migrationBuilder.InsertData(
                table: "KeyValueCouple",
                columns: new[] { "Id", "IsLocked", "Key", "ValueJson", "ValueType" },
                values: new object[] { new Guid("a2b1305e-525f-4c6d-bc36-a92e0175a928"), false, "Site.Description", "\"Osharp with .NetStandard2.0 & Angular6\"", "System.String" });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime" },
                values: new object[] { "c853f82f-009d-4fa8-bf61-2af5763b29b6", new DateTime(2018, 7, 31, 22, 40, 27, 469, DateTimeKind.Local) });
        }
    }
}
