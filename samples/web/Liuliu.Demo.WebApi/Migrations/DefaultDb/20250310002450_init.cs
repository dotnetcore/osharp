using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liuliu.Demo.Web.Migrations.DefaultDb
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auth_EntityInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "实体名称"),
                    TypeName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "实体类型名称"),
                    AuditEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "是否数据审计"),
                    PropertyJson = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "实体属性信息Json字符串")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_EntityInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Function",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
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
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Function", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Module",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "模块名称"),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "模块描述"),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "模块代码"),
                    OrderCode = table.Column<double>(type: "float", nullable: false, comment: "排序码"),
                    TreePathString = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "父节点树形路径"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true, comment: "父模块编号")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "名称"),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "编码"),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "描述"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true, comment: "父组织机构编号"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    FunctionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "执行的功能名"),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "当前用户标识"),
                    UserName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "当前用户名"),
                    NickName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "当前用户昵称"),
                    Ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "当前访问IP"),
                    OperationSystem = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "操作系统"),
                    Browser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "浏览器"),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "UserAgent"),
                    ResultType = table.Column<int>(type: "int", nullable: false, comment: "ResultType"),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "消息"),
                    Elapsed = table.Column<long>(type: "bigint", nullable: false, comment: "Elapsed"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "CreatedTime")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_AuditOperation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Systems_KeyValue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Key = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false, comment: "数据键名"),
                    ValueJson = table.Column<string>(type: "text", nullable: true, comment: "数据值JSON"),
                    ValueType = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "数据值类型名"),
                    Display = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "显示名称"),
                    Remark = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "显示名称"),
                    Order = table.Column<int>(type: "int", nullable: false, comment: "Order"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_KeyValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Systems_Menu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "名称"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "显示"),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "图标"),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "链接"),
                    Target = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "链接目标"),
                    Acl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "访问控制列表"),
                    OrderCode = table.Column<double>(type: "float", nullable: false, comment: "节点内排序"),
                    Data = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "菜单数据"),
                    TreePathString = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true, comment: "父节点树形路径"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "是否启用"),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, comment: "是否系统"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true, comment: "父菜单编号")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_Menu_Systems_Menu_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Systems_Menu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Auth_ModuleFunction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false, comment: "模块编号"),
                    FunctionId = table.Column<long>(type: "bigint", nullable: false, comment: "功能编号")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "实体名称"),
                    TypeName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "类型名称"),
                    EntityKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "类型名称"),
                    OperateType = table.Column<int>(type: "int", nullable: false, comment: "OperateType"),
                    OperationId = table.Column<long>(type: "bigint", nullable: false, comment: "OperationId")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "名称"),
                    FieldName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "字段"),
                    OriginalValue = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "旧值"),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "新值"),
                    DataType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "数据类型"),
                    AuditEntityId = table.Column<long>(type: "bigint", nullable: false, comment: "AuditEntityId")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false, comment: "角色编号"),
                    EntityId = table.Column<long>(type: "bigint", nullable: false, comment: "数据编号"),
                    Operation = table.Column<int>(type: "int", nullable: false, comment: "数据权限操作"),
                    FilterGroupJson = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true, comment: "过滤条件组Json字符串"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "用户编号"),
                    EntityId = table.Column<long>(type: "bigint", nullable: false, comment: "数据编号"),
                    FilterGroupJson = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true, comment: "过滤条件组Json字符串"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false, comment: "模块编号"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false, comment: "角色编号")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false, comment: "模块编号"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "用户编号"),
                    Disabled = table.Column<bool>(type: "bit", nullable: false, comment: "Disabled")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "登录IP"),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "用户代理头"),
                    LogoutTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "退出时间"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "用户编号")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_LoginLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    MessageId = table.Column<long>(type: "bigint", nullable: true),
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
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_RoleClaim",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false, comment: "角色编号"),
                    ClaimType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "声明类型"),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "声明值")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Remark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "备注"),
                    MessageId = table.Column<long>(type: "bigint", nullable: true),
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
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserClaim",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "用户编号"),
                    ClaimType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "声明类型"),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "声明值")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    RegisterIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "注册IP"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "用户编号")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    LoginProvider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "登录的登录提供程序"),
                    ProviderKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "第三方用户的唯一标识"),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "第三方用户昵称"),
                    Avatar = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "第三方用户头像"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "用户编号"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "用户编号"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false, comment: "角色编号"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "DeletedTime")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "用户编号"),
                    LoginProvider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "登录提供者"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "令牌名称"),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "令牌值")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "标题"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "内容"),
                    MessageType = table.Column<int>(type: "int", nullable: false, comment: "消息类型"),
                    NewReplyCount = table.Column<long>(type: "bigint", nullable: false, comment: "新回复数"),
                    IsSended = table.Column<bool>(type: "bit", nullable: false, comment: "是否发送"),
                    CanReply = table.Column<bool>(type: "bit", nullable: false, comment: "是否允许回复"),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "生效时间"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "过期时间"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "删除时间"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    SenderId = table.Column<long>(type: "bigint", nullable: false, comment: "发送人编号")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "接收时间"),
                    NewReplyCount = table.Column<long>(type: "bigint", nullable: false, comment: "新回复数，接收者使用"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    MessageId = table.Column<long>(type: "bigint", nullable: false, comment: "接收的主消息编号"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "消息接收人编号")
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
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "编号"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "消息内容"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, comment: "是否已读"),
                    ParentMessageId = table.Column<long>(type: "bigint", nullable: false, comment: "回复的主消息，当回复主消息时有效"),
                    ParentReplyId = table.Column<long>(type: "bigint", nullable: false, comment: "回复的回复消息，当回复回复消息时有效"),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, comment: "是否锁定"),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "删除时间"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: " 消息回复人编号"),
                    BelongMessageId = table.Column<long>(type: "bigint", nullable: false, comment: "回复所属主消息，用于避免递归查询")
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
                        name: "FK_Infos_MessageReply_Infos_MessageReply_ParentReplyId",
                        column: x => x.ParentReplyId,
                        principalTable: "Infos_MessageReply",
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

            migrationBuilder.CreateIndex(
                name: "IX_Systems_Menu_ParentId",
                table: "Systems_Menu",
                column: "ParentId");

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

        /// <inheritdoc />
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
                name: "Systems_Menu");

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
