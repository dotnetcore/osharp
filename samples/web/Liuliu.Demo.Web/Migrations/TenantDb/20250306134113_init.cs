using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liuliu.Demo.Web.Migrations.TenantDb
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MultiTenancy_Tenant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    TenantKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "租户标识"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "租户名称"),
                    ShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "租户简称"),
                    Host = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "租户主机"),
                    ConnectionString = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "连接字符串"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "是否启用"),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "到期时间"),
                    CustomJson = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "自定义配置数据"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "DeletedTime"),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiTenancy_Tenant", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultiTenancy_Tenant");
        }
    }
}
