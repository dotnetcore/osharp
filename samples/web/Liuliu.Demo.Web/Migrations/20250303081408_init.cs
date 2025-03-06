using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liuliu.Demo.Web.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auth_EntityInfo",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AuditEnabled = table.Column<bool>(type: "bit", nullable: false),
                    PropertyJson = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "实体名称"),
                    TypeName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "实体类型名称"),
                    AuditEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "是否数据审计"),
                    PropertyJson = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false, comment: "实体属性信息Json字符串")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_EntityInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Function",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Area = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsController = table.Column<bool>(type: "bit", nullable: false),
                    IsAjax = table.Column<bool>(type: "bit", nullable: false),
                    AccessType = table.Column<int>(type: "int", nullable: false),
                    IsAccessTypeChanged = table.Column<bool>(type: "bit", nullable: false),
                    AuditOperationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AuditEntityEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CacheExpirationSeconds = table.Column<int>(type: "int", nullable: false),
                    IsCacheSliding = table.Column<bool>(type: "bit", nullable: false),
                    IsSlaveDatabase = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "名称"),
                    Area = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "区域"),
                    Controller = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "控制器"),
                    Action = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "功能"),
                    IsController = table.Column<bool>(type: "bit", nullable: false, comment: "是否控制器"),
                    IsAjax = table.Column<bool>(type: "bit", nullable: false, comment: "是否Ajax功能"),
                    AccessType = table.Column<int>(type: "int", nullable: false, comment: "访问类型"),
                    IsAccessTypeChanged = table.Column<bool>(type: "bit", nullable: false, comment: "访问类型是否更改"),
                    AuditOperationEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "是否操作审计"),
                    AuditEntityEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "是否数据审计"),
                    CacheExpirationSeconds = table.Column<int>(type: "int", nullable: false, comment: "数据缓存秒数"),
                    IsCacheSliding = table.Column<bool>(type: "bit", nullable: false, comment: "是否相对过期时间"),
                    IsSlaveDatabase = table.Column<bool>(type: "bit", nullable: false, comment: "是否从库"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Function", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Module",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrderCode = table.Column<double>(type: "float", nullable: false),
                    TreePathString = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
========
                    Id = table.Column<int>(type: "int", nullable: false, comment: "编号")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "模块名称"),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "模块描述"),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "模块代码"),
                    OrderCode = table.Column<double>(type: "float", nullable: false, comment: "排序码"),
                    TreePathString = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "父节点树形路径"),
                    ParentId = table.Column<int>(type: "int", nullable: true, comment: "父模块编号")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_Module_Auth_Module_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Auth_Module",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Identity_Organization",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
========
                    Id = table.Column<int>(type: "int", nullable: false, comment: "编号")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "名称"),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "描述"),
                    ParentId = table.Column<int>(type: "int", nullable: true, comment: "父组织机构编号")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Organization_Identity_Organization_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Identity_Organization",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Systems_AuditOperation",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FunctionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NickName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperationSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultType = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Elapsed = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    FunctionName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "FunctionName"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "UserId"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "UserName"),
                    NickName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "NickName"),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Ip"),
                    OperationSystem = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "OperationSystem"),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Browser"),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "UserAgent"),
                    ResultType = table.Column<int>(type: "int", nullable: false, comment: "ResultType"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Message"),
                    Elapsed = table.Column<int>(type: "int", nullable: false, comment: "Elapsed"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "CreatedTime")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_AuditOperation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Systems_KeyValue",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ValueJson = table.Column<string>(type: "text", nullable: true),
                    ValueType = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Display = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    Key = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false, comment: "数据键名"),
                    ValueJson = table.Column<string>(type: "text", nullable: true, comment: "数据值JSON"),
                    ValueType = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "数据值类型名"),
                    Display = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "显示名称"),
                    Remark = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "显示名称"),
                    Order = table.Column<int>(type: "int", nullable: false, comment: "Order"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_KeyValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_ModuleFunction",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    FunctionId = table.Column<long>(type: "bigint", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    ModuleId = table.Column<int>(type: "int", nullable: false, comment: "模块编号"),
                    FunctionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "功能编号")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_ModuleFunction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_ModuleFunction_Auth_Function_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Auth_Function",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auth_ModuleFunction_Auth_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Auth_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Systems_AuditEntity",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperateType = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<long>(type: "bigint", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Name"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "TypeName"),
                    EntityKey = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "EntityKey"),
                    OperateType = table.Column<int>(type: "int", nullable: false, comment: "OperateType"),
                    OperationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "OperationId")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_AuditEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_AuditEntity_Systems_AuditOperation_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Systems_AuditOperation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Systems_AuditProperty",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditEntityId = table.Column<long>(type: "bigint", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "DisplayName"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "FieldName"),
                    OriginalValue = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "OriginalValue"),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "NewValue"),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "DataType"),
                    AuditEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "AuditEntityId")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_AuditProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_AuditProperty_Systems_AuditEntity_AuditEntityId",
                        column: x => x.AuditEntityId,
                        principalTable: "Systems_AuditEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auth_EntityRole",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    Operation = table.Column<int>(type: "int", nullable: false),
                    FilterGroupJson = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "角色编号"),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "数据编号"),
                    Operation = table.Column<int>(type: "int", nullable: false, comment: "数据权限操作"),
                    FilterGroupJson = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true, comment: "过滤条件组Json字符串"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_EntityRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_EntityRole_Auth_EntityInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Auth_EntityInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auth_EntityUser",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    FilterGroupJson = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户编号"),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "数据编号"),
                    FilterGroupJson = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true, comment: "过滤条件组Json字符串"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_EntityUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_EntityUser_Auth_EntityInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Auth_EntityInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auth_ModuleRole",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    ModuleId = table.Column<int>(type: "int", nullable: false, comment: "模块编号"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "角色编号")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_ModuleRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_ModuleRole_Auth_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Auth_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auth_ModuleUser",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Disabled = table.Column<bool>(type: "bit", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    ModuleId = table.Column<int>(type: "int", nullable: false, comment: "模块编号"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户编号"),
                    Disabled = table.Column<bool>(type: "bit", nullable: false, comment: "Disabled")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_ModuleUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_ModuleUser_Auth_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Auth_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identity_LoginLog",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "登录IP"),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "用户代理头"),
                    LogoutTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "退出时间"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户编号")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_LoginLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_Role",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MessageId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
========
                    Id = table.Column<int>(type: "int", nullable: false, comment: "编号")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "角色名称"),
                    NormalizedName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "标准化角色名称"),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "版本标识"),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "角色描述"),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false, comment: "是否管理员角色"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, comment: "是否默认角色"),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, comment: "是否系统角色"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "DeletedTime")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_RoleClaim",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
========
                    Id = table.Column<int>(type: "int", nullable: false, comment: "编号")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "角色编号"),
                    ClaimType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "声明类型"),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "声明值")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_RoleClaim_Identity_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Identity_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identity_User",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NormalizeEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HeadImg = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
========
                    Id = table.Column<int>(type: "int", nullable: false, comment: "编号")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "备注"),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "用户名"),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "标准化的用户名"),
                    NickName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "用户昵称"),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "电子邮箱"),
                    NormalizeEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "标准化的电子邮箱"),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, comment: "电子邮箱确认"),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "密码哈希值"),
                    HeadImg = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "用户头像"),
                    SecurityStamp = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "安全标识"),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "版本标识"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "手机号码"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false, comment: "手机号码确定"),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "双因子身份验证"),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, comment: "锁定时间"),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "是否登录锁"),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false, comment: "登录失败次数"),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, comment: "是否系统用户"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "DeletedTime")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserClaim",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
========
                    Id = table.Column<int>(type: "int", nullable: false, comment: "编号")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户编号"),
                    ClaimType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "声明类型"),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "声明值")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_UserClaim_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserDetail",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RegisterIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
========
                    Id = table.Column<int>(type: "int", nullable: false, comment: "编号")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterIp = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "注册IP"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户编号")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_UserDetail_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserLogin",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    LoginProvider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "登录的登录提供程序"),
                    ProviderKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "第三方用户的唯一标识"),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "第三方用户昵称"),
                    Avatar = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "第三方用户头像"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户编号"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_UserLogin_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserRole",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户编号"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "角色编号"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "DeletedTime")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_UserRole_Identity_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Identity_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Identity_UserRole_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserToken",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户编号"),
                    LoginProvider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "登录提供者"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "令牌名称"),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "令牌值")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_UserToken_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Infos_Message",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    NewReplyCount = table.Column<int>(type: "int", nullable: false),
                    IsSended = table.Column<bool>(type: "bit", nullable: false),
                    CanReply = table.Column<bool>(type: "bit", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<long>(type: "bigint", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "标题"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "内容"),
                    MessageType = table.Column<int>(type: "int", nullable: false, comment: "消息类型"),
                    NewReplyCount = table.Column<int>(type: "int", nullable: false, comment: "新回复数"),
                    IsSended = table.Column<bool>(type: "bit", nullable: false, comment: "是否发送"),
                    CanReply = table.Column<bool>(type: "bit", nullable: false, comment: "是否允许回复"),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "生效时间"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "过期时间"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "删除时间"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    SenderId = table.Column<int>(type: "int", nullable: false, comment: "发送人编号")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infos_Message_Identity_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Infos_MessageReceive",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewReplyCount = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "接收时间"),
                    NewReplyCount = table.Column<int>(type: "int", nullable: false, comment: "新回复数，接收者使用"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "接收的主消息编号"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "消息接收人编号")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos_MessageReceive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infos_MessageReceive_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Infos_MessageReceive_Infos_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Infos_Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Infos_MessageReply",
                columns: table => new
                {
<<<<<<<< HEAD:samples/web/Liuliu.Demo.Web/Migrations/20221116015548_Init.cs
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ParentMessageId = table.Column<long>(type: "bigint", nullable: false),
                    ParentReplyId = table.Column<long>(type: "bigint", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BelongMessageId = table.Column<long>(type: "bigint", nullable: false)
========
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "编号"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "消息内容"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, comment: "是否已读"),
                    ParentMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "回复的主消息，当回复主消息时有效"),
                    ParentReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "回复的回复消息，当回复回复消息时有效"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "删除时间"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: " 消息回复人编号"),
                    BelongMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "回复所属主消息，用于避免递归查询")
>>>>>>>> master:samples/web/Liuliu.Demo.Web/Migrations/20250303081408_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos_MessageReply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infos_MessageReply_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Infos_MessageReply_Infos_Message_BelongMessageId",
                        column: x => x.BelongMessageId,
                        principalTable: "Infos_Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Infos_MessageReply_Infos_Message_ParentMessageId",
                        column: x => x.ParentMessageId,
                        principalTable: "Infos_Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Infos_MessageReply_Infos_MessageReply_ParentReplyId",
                        column: x => x.ParentReplyId,
                        principalTable: "Infos_MessageReply",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ClassFullNameIndex",
                table: "Auth_EntityInfo",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EntityRoleIndex",
                table: "Auth_EntityRole",
                columns: new[] { "EntityId", "RoleId", "Operation" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auth_EntityRole_RoleId",
                table: "Auth_EntityRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EntityUserIndex",
                table: "Auth_EntityUser",
                columns: new[] { "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Auth_EntityUser_UserId",
                table: "Auth_EntityUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "AreaControllerActionIndex",
                table: "Auth_Function",
                columns: new[] { "Area", "Controller", "Action" },
                unique: true,
                filter: "[Area] IS NOT NULL AND [Controller] IS NOT NULL AND [Action] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auth_Module_ParentId",
                table: "Auth_Module",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Auth_ModuleFunction_FunctionId",
                table: "Auth_ModuleFunction",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "ModuleFunctionIndex",
                table: "Auth_ModuleFunction",
                columns: new[] { "ModuleId", "FunctionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auth_ModuleRole_RoleId",
                table: "Auth_ModuleRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "ModuleRoleIndex",
                table: "Auth_ModuleRole",
                columns: new[] { "ModuleId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auth_ModuleUser_UserId",
                table: "Auth_ModuleUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "ModuleUserIndex",
                table: "Auth_ModuleUser",
                columns: new[] { "ModuleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_LoginLog_UserId",
                table: "Identity_LoginLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Organization_ParentId",
                table: "Identity_Organization",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Role_MessageId",
                table: "Identity_Role",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Identity_Role",
                columns: new[] { "NormalizedName", "DeletedTime" },
                unique: true,
                filter: "[DeletedTime] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_RoleClaim_RoleId",
                table: "Identity_RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Identity_User",
                columns: new[] { "NormalizeEmail", "DeletedTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Identity_User_MessageId",
                table: "Identity_User",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Identity_User",
                columns: new[] { "NormalizedUserName", "DeletedTime" },
                unique: true,
                filter: "[DeletedTime] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_UserClaim_UserId",
                table: "Identity_UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_UserDetail_UserId",
                table: "Identity_UserDetail",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_UserLogin_UserId",
                table: "Identity_UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UserLoginIndex",
                table: "Identity_UserLogin",
                columns: new[] { "LoginProvider", "ProviderKey" },
                unique: true,
                filter: "[LoginProvider] IS NOT NULL AND [ProviderKey] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_UserRole_RoleId",
                table: "Identity_UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UserRoleIndex",
                table: "Identity_UserRole",
                columns: new[] { "UserId", "RoleId", "DeletedTime" },
                unique: true,
                filter: "[DeletedTime] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserTokenIndex",
                table: "Identity_UserToken",
                columns: new[] { "UserId", "LoginProvider", "Name" },
                unique: true,
                filter: "[LoginProvider] IS NOT NULL AND [Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_Message_SenderId",
                table: "Infos_Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_MessageReceive_MessageId",
                table: "Infos_MessageReceive",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_MessageReceive_UserId",
                table: "Infos_MessageReceive",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_MessageReply_BelongMessageId",
                table: "Infos_MessageReply",
                column: "BelongMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_MessageReply_ParentMessageId",
                table: "Infos_MessageReply",
                column: "ParentMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_MessageReply_ParentReplyId",
                table: "Infos_MessageReply",
                column: "ParentReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_MessageReply_UserId",
                table: "Infos_MessageReply",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Systems_AuditEntity_OperationId",
                table: "Systems_AuditEntity",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Systems_AuditProperty_AuditEntityId",
                table: "Systems_AuditProperty",
                column: "AuditEntityId");

            migrationBuilder.CreateIndex(
                name: "KeyIndex",
                table: "Systems_KeyValue",
                column: "Key",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_EntityRole_Identity_Role_RoleId",
                table: "Auth_EntityRole",
                column: "RoleId",
                principalTable: "Identity_Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_EntityUser_Identity_User_UserId",
                table: "Auth_EntityUser",
                column: "UserId",
                principalTable: "Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_ModuleRole_Identity_Role_RoleId",
                table: "Auth_ModuleRole",
                column: "RoleId",
                principalTable: "Identity_Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_ModuleUser_Identity_User_UserId",
                table: "Auth_ModuleUser",
                column: "UserId",
                principalTable: "Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_LoginLog_Identity_User_UserId",
                table: "Identity_LoginLog",
                column: "UserId",
                principalTable: "Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_Role_Infos_Message_MessageId",
                table: "Identity_Role",
                column: "MessageId",
                principalTable: "Infos_Message",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_User_Infos_Message_MessageId",
                table: "Identity_User",
                column: "MessageId",
                principalTable: "Infos_Message",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Infos_Message_Identity_User_SenderId",
                table: "Infos_Message");

            migrationBuilder.DropTable(
                name: "Auth_EntityRole");

            migrationBuilder.DropTable(
                name: "Auth_EntityUser");

            migrationBuilder.DropTable(
                name: "Auth_ModuleFunction");

            migrationBuilder.DropTable(
                name: "Auth_ModuleRole");

            migrationBuilder.DropTable(
                name: "Auth_ModuleUser");

            migrationBuilder.DropTable(
                name: "Identity_LoginLog");

            migrationBuilder.DropTable(
                name: "Identity_Organization");

            migrationBuilder.DropTable(
                name: "Identity_RoleClaim");

            migrationBuilder.DropTable(
                name: "Identity_UserClaim");

            migrationBuilder.DropTable(
                name: "Identity_UserDetail");

            migrationBuilder.DropTable(
                name: "Identity_UserLogin");

            migrationBuilder.DropTable(
                name: "Identity_UserRole");

            migrationBuilder.DropTable(
                name: "Identity_UserToken");

            migrationBuilder.DropTable(
                name: "Infos_MessageReceive");

            migrationBuilder.DropTable(
                name: "Infos_MessageReply");

            migrationBuilder.DropTable(
                name: "Systems_AuditProperty");

            migrationBuilder.DropTable(
                name: "Systems_KeyValue");

            migrationBuilder.DropTable(
                name: "Auth_EntityInfo");

            migrationBuilder.DropTable(
                name: "Auth_Function");

            migrationBuilder.DropTable(
                name: "Auth_Module");

            migrationBuilder.DropTable(
                name: "Identity_Role");

            migrationBuilder.DropTable(
                name: "Systems_AuditEntity");

            migrationBuilder.DropTable(
                name: "Systems_AuditOperation");

            migrationBuilder.DropTable(
                name: "Identity_User");

            migrationBuilder.DropTable(
                name: "Infos_Message");
        }
    }
}
