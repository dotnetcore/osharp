using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liuliu.Demo.Web.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Auth_Auth_EntityInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuditEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PropertyJson = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Auth_EntityInfo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Auth_Auth_Function",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Area = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Controller = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Action = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsController = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsAjax = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessType = table.Column<int>(type: "int", nullable: false),
                    IsAccessTypeChanged = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AuditOperationEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AuditEntityEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CacheExpirationSeconds = table.Column<int>(type: "int", nullable: false),
                    IsCacheSliding = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSlaveDatabase = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Auth_Function", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Auth_Auth_Module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderCode = table.Column<double>(type: "double", nullable: false),
                    TreePathString = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Auth_Module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_Auth_Module_Auth_Auth_Module_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Auth_Auth_Module",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_Organization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Identity_Organization_Identity_Identity_Organizatio~",
                        column: x => x.ParentId,
                        principalTable: "Identity_Identity_Organization",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Systems_Systems_AuditOperation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FunctionName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NickName = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ip = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OperationSystem = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Browser = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAgent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResultType = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Elapsed = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_Systems_AuditOperation", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Systems_Systems_KeyValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Key = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValueJson = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValueType = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Display = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_Systems_KeyValue", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Systems_Systems_Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Text = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Target = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Acl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderCode = table.Column<double>(type: "double", nullable: false),
                    Data = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TreePathString = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSystem = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_Systems_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_Systems_Menu_Systems_Systems_Menu_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Systems_Systems_Menu",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Auth_Auth_ModuleFunction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    FunctionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Auth_ModuleFunction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_Auth_ModuleFunction_Auth_Auth_Function_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Auth_Auth_Function",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auth_Auth_ModuleFunction_Auth_Auth_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Auth_Auth_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Systems_Systems_AuditEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityKey = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OperateType = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_Systems_AuditEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_Systems_AuditEntity_Systems_Systems_AuditOperation_O~",
                        column: x => x.OperationId,
                        principalTable: "Systems_Systems_AuditOperation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Systems_Systems_AuditProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DisplayName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OriginalValue = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewValue = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataType = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuditEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_Systems_AuditProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_Systems_AuditProperty_Systems_Systems_AuditEntity_Au~",
                        column: x => x.AuditEntityId,
                        principalTable: "Systems_Systems_AuditEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Auth_Auth_EntityRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Operation = table.Column<int>(type: "int", nullable: false),
                    FilterGroupJson = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Auth_EntityRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_Auth_EntityRole_Auth_Auth_EntityInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Auth_Auth_EntityInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Auth_Auth_EntityUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FilterGroupJson = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Auth_EntityUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_Auth_EntityUser_Auth_Auth_EntityInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Auth_Auth_EntityInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Auth_Auth_ModuleRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Auth_ModuleRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_Auth_ModuleRole_Auth_Auth_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Auth_Auth_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Auth_Auth_ModuleUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Disabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Auth_ModuleUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_Auth_ModuleUser_Auth_Auth_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Auth_Auth_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_LoginLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Ip = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAgent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogoutTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_LoginLog", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MessageId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSystem = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_Role", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Identity_RoleClaim_Identity_Identity_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Identity_Identity_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Remark = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NickName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizeEmail = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeadImg = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    IsSystem = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_User", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Identity_UserClaim_Identity_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_UserDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RegisterIp = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_UserDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Identity_UserDetail_Identity_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_UserLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LoginProvider = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_UserLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Identity_UserLogin_Identity_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_UserRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Identity_UserRole_Identity_Identity_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Identity_Identity_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Identity_Identity_UserRole_Identity_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identity_Identity_UserToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Identity_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Identity_UserToken_Identity_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Infos_Infos_Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    NewReplyCount = table.Column<int>(type: "int", nullable: false),
                    IsSended = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanReply = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos_Infos_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infos_Infos_Message_Identity_Identity_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Identity_Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Infos_Infos_MessageReceive",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReadDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NewReplyCount = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MessageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos_Infos_MessageReceive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infos_Infos_MessageReceive_Identity_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Infos_Infos_MessageReceive_Infos_Infos_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Infos_Infos_Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Infos_Infos_MessageReply",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentMessageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ParentReplyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BelongMessageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos_Infos_MessageReply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infos_Infos_MessageReply_Identity_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Infos_Infos_MessageReply_Infos_Infos_Message_BelongMessageId",
                        column: x => x.BelongMessageId,
                        principalTable: "Infos_Infos_Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Infos_Infos_MessageReply_Infos_Infos_Message_ParentMessageId",
                        column: x => x.ParentMessageId,
                        principalTable: "Infos_Infos_Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Infos_Infos_MessageReply_Infos_Infos_MessageReply_ParentRepl~",
                        column: x => x.ParentReplyId,
                        principalTable: "Infos_Infos_MessageReply",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ClassFullNameIndex",
                table: "Auth_Auth_EntityInfo",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EntityRoleIndex",
                table: "Auth_Auth_EntityRole",
                columns: new[] { "EntityId", "RoleId", "Operation" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auth_Auth_EntityRole_RoleId",
                table: "Auth_Auth_EntityRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EntityUserIndex",
                table: "Auth_Auth_EntityUser",
                columns: new[] { "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Auth_Auth_EntityUser_UserId",
                table: "Auth_Auth_EntityUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "AreaControllerActionIndex",
                table: "Auth_Auth_Function",
                columns: new[] { "Area", "Controller", "Action" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auth_Auth_Module_ParentId",
                table: "Auth_Auth_Module",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Auth_Auth_ModuleFunction_FunctionId",
                table: "Auth_Auth_ModuleFunction",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "ModuleFunctionIndex",
                table: "Auth_Auth_ModuleFunction",
                columns: new[] { "ModuleId", "FunctionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auth_Auth_ModuleRole_RoleId",
                table: "Auth_Auth_ModuleRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "ModuleRoleIndex",
                table: "Auth_Auth_ModuleRole",
                columns: new[] { "ModuleId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auth_Auth_ModuleUser_UserId",
                table: "Auth_Auth_ModuleUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "ModuleUserIndex",
                table: "Auth_Auth_ModuleUser",
                columns: new[] { "ModuleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Identity_LoginLog_UserId",
                table: "Identity_Identity_LoginLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Identity_Organization_ParentId",
                table: "Identity_Identity_Organization",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Identity_Role_MessageId",
                table: "Identity_Identity_Role",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Identity_Identity_Role",
                columns: new[] { "NormalizedName", "DeletedTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Identity_RoleClaim_RoleId",
                table: "Identity_Identity_RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Identity_Identity_User",
                columns: new[] { "NormalizeEmail", "DeletedTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Identity_User_MessageId",
                table: "Identity_Identity_User",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Identity_Identity_User",
                columns: new[] { "NormalizedUserName", "DeletedTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Identity_UserClaim_UserId",
                table: "Identity_Identity_UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Identity_UserDetail_UserId",
                table: "Identity_Identity_UserDetail",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Identity_UserLogin_UserId",
                table: "Identity_Identity_UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UserLoginIndex",
                table: "Identity_Identity_UserLogin",
                columns: new[] { "LoginProvider", "ProviderKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Identity_UserRole_RoleId",
                table: "Identity_Identity_UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UserRoleIndex",
                table: "Identity_Identity_UserRole",
                columns: new[] { "UserId", "RoleId", "DeletedTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserTokenIndex",
                table: "Identity_Identity_UserToken",
                columns: new[] { "UserId", "LoginProvider", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Infos_Infos_Message_SenderId",
                table: "Infos_Infos_Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_Infos_MessageReceive_MessageId",
                table: "Infos_Infos_MessageReceive",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_Infos_MessageReceive_UserId",
                table: "Infos_Infos_MessageReceive",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_Infos_MessageReply_BelongMessageId",
                table: "Infos_Infos_MessageReply",
                column: "BelongMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_Infos_MessageReply_ParentMessageId",
                table: "Infos_Infos_MessageReply",
                column: "ParentMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_Infos_MessageReply_ParentReplyId",
                table: "Infos_Infos_MessageReply",
                column: "ParentReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_Infos_MessageReply_UserId",
                table: "Infos_Infos_MessageReply",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Systems_Systems_AuditEntity_OperationId",
                table: "Systems_Systems_AuditEntity",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Systems_Systems_AuditProperty_AuditEntityId",
                table: "Systems_Systems_AuditProperty",
                column: "AuditEntityId");

            migrationBuilder.CreateIndex(
                name: "KeyIndex",
                table: "Systems_Systems_KeyValue",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Systems_Systems_Menu_ParentId",
                table: "Systems_Systems_Menu",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_Auth_EntityRole_Identity_Identity_Role_RoleId",
                table: "Auth_Auth_EntityRole",
                column: "RoleId",
                principalTable: "Identity_Identity_Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_Auth_EntityUser_Identity_Identity_User_UserId",
                table: "Auth_Auth_EntityUser",
                column: "UserId",
                principalTable: "Identity_Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_Auth_ModuleRole_Identity_Identity_Role_RoleId",
                table: "Auth_Auth_ModuleRole",
                column: "RoleId",
                principalTable: "Identity_Identity_Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_Auth_ModuleUser_Identity_Identity_User_UserId",
                table: "Auth_Auth_ModuleUser",
                column: "UserId",
                principalTable: "Identity_Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_Identity_LoginLog_Identity_Identity_User_UserId",
                table: "Identity_Identity_LoginLog",
                column: "UserId",
                principalTable: "Identity_Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_Identity_Role_Infos_Infos_Message_MessageId",
                table: "Identity_Identity_Role",
                column: "MessageId",
                principalTable: "Infos_Infos_Message",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_Identity_User_Infos_Infos_Message_MessageId",
                table: "Identity_Identity_User",
                column: "MessageId",
                principalTable: "Infos_Infos_Message",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Infos_Infos_Message_Identity_Identity_User_SenderId",
                table: "Infos_Infos_Message");

            migrationBuilder.DropTable(
                name: "Auth_Auth_EntityRole");

            migrationBuilder.DropTable(
                name: "Auth_Auth_EntityUser");

            migrationBuilder.DropTable(
                name: "Auth_Auth_ModuleFunction");

            migrationBuilder.DropTable(
                name: "Auth_Auth_ModuleRole");

            migrationBuilder.DropTable(
                name: "Auth_Auth_ModuleUser");

            migrationBuilder.DropTable(
                name: "Identity_Identity_LoginLog");

            migrationBuilder.DropTable(
                name: "Identity_Identity_Organization");

            migrationBuilder.DropTable(
                name: "Identity_Identity_RoleClaim");

            migrationBuilder.DropTable(
                name: "Identity_Identity_UserClaim");

            migrationBuilder.DropTable(
                name: "Identity_Identity_UserDetail");

            migrationBuilder.DropTable(
                name: "Identity_Identity_UserLogin");

            migrationBuilder.DropTable(
                name: "Identity_Identity_UserRole");

            migrationBuilder.DropTable(
                name: "Identity_Identity_UserToken");

            migrationBuilder.DropTable(
                name: "Infos_Infos_MessageReceive");

            migrationBuilder.DropTable(
                name: "Infos_Infos_MessageReply");

            migrationBuilder.DropTable(
                name: "Systems_Systems_AuditProperty");

            migrationBuilder.DropTable(
                name: "Systems_Systems_KeyValue");

            migrationBuilder.DropTable(
                name: "Systems_Systems_Menu");

            migrationBuilder.DropTable(
                name: "Auth_Auth_EntityInfo");

            migrationBuilder.DropTable(
                name: "Auth_Auth_Function");

            migrationBuilder.DropTable(
                name: "Auth_Auth_Module");

            migrationBuilder.DropTable(
                name: "Identity_Identity_Role");

            migrationBuilder.DropTable(
                name: "Systems_Systems_AuditEntity");

            migrationBuilder.DropTable(
                name: "Systems_Systems_AuditOperation");

            migrationBuilder.DropTable(
                name: "Identity_Identity_User");

            migrationBuilder.DropTable(
                name: "Infos_Infos_Message");
        }
    }
}
