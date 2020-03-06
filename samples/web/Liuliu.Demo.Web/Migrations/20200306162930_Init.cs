using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Liuliu.Demo.Web.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auth_EntityInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    TypeName = table.Column<string>(nullable: false),
                    AuditEnabled = table.Column<bool>(nullable: false),
                    PropertyJson = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_EntityInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Function",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    IsController = table.Column<bool>(nullable: false),
                    IsAjax = table.Column<bool>(nullable: false),
                    AccessType = table.Column<int>(nullable: false),
                    IsAccessTypeChanged = table.Column<bool>(nullable: false),
                    AuditOperationEnabled = table.Column<bool>(nullable: false),
                    AuditEntityEnabled = table.Column<bool>(nullable: false),
                    CacheExpirationSeconds = table.Column<int>(nullable: false),
                    IsCacheSliding = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Function", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Module",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    OrderCode = table.Column<double>(nullable: false),
                    TreePathString = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_Module_Auth_Module_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Auth_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ApiResource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    AllowedAccessTokenSigningAlgorithms = table.Column<string>(maxLength: 100, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    LastAccessed = table.Column<DateTime>(nullable: true),
                    NonEditable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ApiResource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Id4_Client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>(nullable: false),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    RequireClientSecret = table.Column<bool>(nullable: false),
                    ProtocolType = table.Column<string>(maxLength: 200, nullable: false),
                    RequirePkce = table.Column<bool>(nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(nullable: false),
                    RequireRequestObject = table.Column<bool>(nullable: false),
                    AllowOfflineAccess = table.Column<bool>(nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(nullable: false),
                    ClientName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    RequireConsent = table.Column<bool>(nullable: false),
                    AllowRememberConsent = table.Column<bool>(nullable: false),
                    ConsentLifetime = table.Column<int>(nullable: true),
                    ClientUri = table.Column<string>(maxLength: 2000, nullable: true),
                    LogoUri = table.Column<string>(maxLength: 2000, nullable: true),
                    FrontChannelLogoutUri = table.Column<string>(maxLength: 2000, nullable: true),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(nullable: false),
                    BackChannelLogoutUri = table.Column<string>(maxLength: 2000, nullable: true),
                    BackChannelLogoutSessionRequired = table.Column<bool>(nullable: false),
                    EnableLocalLogin = table.Column<bool>(nullable: false),
                    UserSsoLifetime = table.Column<int>(nullable: true),
                    AllowedIdentityTokenSigningAlgorithms = table.Column<string>(maxLength: 100, nullable: true),
                    IdentityTokenLifetime = table.Column<int>(nullable: false),
                    AccessTokenLifetime = table.Column<int>(nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(nullable: false),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(nullable: false),
                    RefreshTokenUsage = table.Column<int>(nullable: false),
                    RefreshTokenExpiration = table.Column<int>(nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(nullable: false),
                    AccessTokenType = table.Column<int>(nullable: false),
                    IncludeJwtId = table.Column<bool>(nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(nullable: false),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(nullable: false),
                    ClientClaimsPrefix = table.Column<string>(maxLength: 200, nullable: true),
                    PairWiseSubjectSalt = table.Column<string>(maxLength: 200, nullable: true),
                    UserCodeType = table.Column<string>(maxLength: 100, nullable: true),
                    DeviceCodeLifetime = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    LastAccessed = table.Column<DateTime>(nullable: true),
                    NonEditable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Id4_DeviceFlowCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceCode = table.Column<string>(maxLength: 200, nullable: false),
                    UserCode = table.Column<string>(maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_DeviceFlowCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Id4_IdentityResource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    Emphasize = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    NonEditable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_IdentityResource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Id4_PersistedGrant",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 200, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_PersistedGrant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_Organization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Organization_Identity_Organization_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Identity_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Systems_AuditOperation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FunctionName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    OperationSystem = table.Column<string>(nullable: true),
                    Browser = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    ResultType = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Elapsed = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_AuditOperation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Systems_KeyValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ValueJson = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_KeyValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_ModuleFunction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    FunctionId = table.Column<Guid>(nullable: false)
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
                name: "Id4_ApiResourceClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    ApiResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ApiResourceClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ApiResourceClaim_Id4_ApiResource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "Id4_ApiResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ApiResourceProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    ApiResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ApiResourceProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ApiResourceProperty_Id4_ApiResource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "Id4_ApiResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ApiScope",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Required = table.Column<bool>(nullable: false),
                    Emphasize = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false),
                    ApiResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ApiScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ApiScope_Id4_ApiResource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "Id4_ApiResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ApiSecret",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Value = table.Column<string>(maxLength: 4000, nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    ApiResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ApiSecret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ApiSecret_Id4_ApiResource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "Id4_ApiResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ClientClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 1000, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ClientClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ClientClaim_Id4_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Id4_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ClientCorsOrigin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Origin = table.Column<string>(maxLength: 200, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ClientCorsOrigin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ClientCorsOrigin_Id4_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Id4_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ClientGrantType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GrantType = table.Column<string>(maxLength: 250, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ClientGrantType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ClientGrantType_Id4_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Id4_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ClientIdPRestriction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Provider = table.Column<string>(maxLength: 200, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ClientIdPRestriction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ClientIdPRestriction_Id4_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Id4_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ClientPostLogoutRedirectUri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PostLogoutRedirectUri = table.Column<string>(maxLength: 2000, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ClientPostLogoutRedirectUri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ClientPostLogoutRedirectUri_Id4_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Id4_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ClientProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ClientProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ClientProperty_Id4_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Id4_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ClientRedirectUri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RedirectUri = table.Column<string>(maxLength: 2000, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ClientRedirectUri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ClientRedirectUri_Id4_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Id4_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ClientScope",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Scope = table.Column<string>(maxLength: 200, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ClientScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ClientScope_Id4_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Id4_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ClientSecret",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Value = table.Column<string>(maxLength: 4000, nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ClientSecret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ClientSecret_Id4_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Id4_Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_IdentityClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    IdentityResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_IdentityClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_IdentityClaim_Id4_IdentityResource_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalTable: "Id4_IdentityResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_IdentityResourceProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    IdentityResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_IdentityResourceProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_IdentityResourceProperty_Id4_IdentityResource_IdentityRe~",
                        column: x => x.IdentityResourceId,
                        principalTable: "Id4_IdentityResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Systems_AuditEntity",
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
                    table.PrimaryKey("PK_Systems_AuditEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_AuditEntity_Systems_AuditOperation_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Systems_AuditOperation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Id4_ApiScopeClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    ApiScopeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Id4_ApiScopeClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id4_ApiScopeClaim_Id4_ApiScope_ApiScopeId",
                        column: x => x.ApiScopeId,
                        principalTable: "Id4_ApiScope",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Systems_AuditProperty",
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
                    Id = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    Operation = table.Column<int>(nullable: false),
                    FilterGroupJson = table.Column<string>(nullable: true),
                    IsLocked = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    FilterGroupJson = table.Column<string>(nullable: true),
                    IsLocked = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Disabled = table.Column<bool>(nullable: false)
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
                name: "Identity_RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_RoleClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false),
                    DeletedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_LoginLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    LogoutTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_LoginLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: false),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RegisterIp = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserLogin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_UserToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_UserToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Infos_Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    MessageType = table.Column<int>(nullable: false),
                    NewReplyCount = table.Column<int>(nullable: false),
                    IsSended = table.Column<bool>(nullable: false),
                    CanReply = table.Column<bool>(nullable: false),
                    BeginDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    IsLocked = table.Column<bool>(nullable: false),
                    DeletedTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    SenderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity_Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    NormalizedName = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(maxLength: 512, nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    IsSystem = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    DeletedTime = table.Column<DateTime>(nullable: true),
                    MessageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Role_Infos_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Infos_Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Identity_User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    NormalizedUserName = table.Column<string>(nullable: false),
                    NickName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizeEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    HeadImg = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    IsSystem = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    DeletedTime = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    MessageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_User_Infos_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Infos_Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Infos_MessageReceive",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReadDate = table.Column<DateTime>(nullable: false),
                    NewReplyCount = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    MessageId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos_MessageReceive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infos_MessageReceive_Infos_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Infos_Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Infos_MessageReceive_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Infos_MessageReply",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    ParentMessageId = table.Column<Guid>(nullable: false),
                    ParentReplyId = table.Column<Guid>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false),
                    DeletedTime = table.Column<DateTime>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    BelongMessageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos_MessageReply", x => x.Id);
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
                    table.ForeignKey(
                        name: "FK_Infos_MessageReply_Identity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Identity_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ClassFullNameIndex",
                table: "Auth_EntityInfo",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auth_EntityRole_RoleId",
                table: "Auth_EntityRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EntityRoleIndex",
                table: "Auth_EntityRole",
                columns: new[] { "EntityId", "RoleId", "Operation" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auth_EntityUser_UserId",
                table: "Auth_EntityUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EntityUserIndex",
                table: "Auth_EntityUser",
                columns: new[] { "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "AreaControllerActionIndex",
                table: "Auth_Function",
                columns: new[] { "Area", "Controller", "Action" },
                unique: true);

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
                name: "IX_Id4_ApiResource_Name",
                table: "Id4_ApiResource",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ApiResourceClaim_ApiResourceId",
                table: "Id4_ApiResourceClaim",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ApiResourceProperty_ApiResourceId",
                table: "Id4_ApiResourceProperty",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ApiScope_ApiResourceId",
                table: "Id4_ApiScope",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ApiScope_Name",
                table: "Id4_ApiScope",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ApiScopeClaim_ApiScopeId",
                table: "Id4_ApiScopeClaim",
                column: "ApiScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ApiSecret_ApiResourceId",
                table: "Id4_ApiSecret",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_Client_ClientId",
                table: "Id4_Client",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ClientClaim_ClientId",
                table: "Id4_ClientClaim",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ClientCorsOrigin_ClientId",
                table: "Id4_ClientCorsOrigin",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ClientGrantType_ClientId",
                table: "Id4_ClientGrantType",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ClientIdPRestriction_ClientId",
                table: "Id4_ClientIdPRestriction",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ClientPostLogoutRedirectUri_ClientId",
                table: "Id4_ClientPostLogoutRedirectUri",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ClientProperty_ClientId",
                table: "Id4_ClientProperty",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ClientRedirectUri_ClientId",
                table: "Id4_ClientRedirectUri",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ClientScope_ClientId",
                table: "Id4_ClientScope",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_ClientSecret_ClientId",
                table: "Id4_ClientSecret",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_DeviceFlowCodes_DeviceCode",
                table: "Id4_DeviceFlowCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Id4_DeviceFlowCodes_Expiration",
                table: "Id4_DeviceFlowCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_DeviceFlowCodes_UserCode",
                table: "Id4_DeviceFlowCodes",
                column: "UserCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Id4_IdentityClaim_IdentityResourceId",
                table: "Id4_IdentityClaim",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_IdentityResource_Name",
                table: "Id4_IdentityResource",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Id4_IdentityResourceProperty_IdentityResourceId",
                table: "Id4_IdentityResourceProperty",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Id4_PersistedGrant_Key",
                table: "Id4_PersistedGrant",
                column: "Key",
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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_RoleClaim_RoleId",
                table: "Identity_RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_User_MessageId",
                table: "Identity_User",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Identity_User",
                columns: new[] { "NormalizeEmail", "DeletedTime" });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Identity_User",
                columns: new[] { "NormalizedUserName", "DeletedTime" },
                unique: true);

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_UserRole_RoleId",
                table: "Identity_UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UserRoleIndex",
                table: "Identity_UserRole",
                columns: new[] { "UserId", "RoleId", "DeletedTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserTokenIndex",
                table: "Identity_UserToken",
                columns: new[] { "UserId", "LoginProvider", "Name" },
                unique: true);

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
                name: "IX_Systems_KeyValue_Key",
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
                name: "FK_Identity_RoleClaim_Identity_Role_RoleId",
                table: "Identity_RoleClaim",
                column: "RoleId",
                principalTable: "Identity_Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_UserRole_Identity_Role_RoleId",
                table: "Identity_UserRole",
                column: "RoleId",
                principalTable: "Identity_Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_UserRole_Identity_User_UserId",
                table: "Identity_UserRole",
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
                name: "FK_Identity_UserClaim_Identity_User_UserId",
                table: "Identity_UserClaim",
                column: "UserId",
                principalTable: "Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_UserDetail_Identity_User_UserId",
                table: "Identity_UserDetail",
                column: "UserId",
                principalTable: "Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_UserLogin_Identity_User_UserId",
                table: "Identity_UserLogin",
                column: "UserId",
                principalTable: "Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_UserToken_Identity_User_UserId",
                table: "Identity_UserToken",
                column: "UserId",
                principalTable: "Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Infos_Message_Identity_User_SenderId",
                table: "Infos_Message",
                column: "SenderId",
                principalTable: "Identity_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
                name: "Id4_ApiResourceClaim");

            migrationBuilder.DropTable(
                name: "Id4_ApiResourceProperty");

            migrationBuilder.DropTable(
                name: "Id4_ApiScopeClaim");

            migrationBuilder.DropTable(
                name: "Id4_ApiSecret");

            migrationBuilder.DropTable(
                name: "Id4_ClientClaim");

            migrationBuilder.DropTable(
                name: "Id4_ClientCorsOrigin");

            migrationBuilder.DropTable(
                name: "Id4_ClientGrantType");

            migrationBuilder.DropTable(
                name: "Id4_ClientIdPRestriction");

            migrationBuilder.DropTable(
                name: "Id4_ClientPostLogoutRedirectUri");

            migrationBuilder.DropTable(
                name: "Id4_ClientProperty");

            migrationBuilder.DropTable(
                name: "Id4_ClientRedirectUri");

            migrationBuilder.DropTable(
                name: "Id4_ClientScope");

            migrationBuilder.DropTable(
                name: "Id4_ClientSecret");

            migrationBuilder.DropTable(
                name: "Id4_DeviceFlowCodes");

            migrationBuilder.DropTable(
                name: "Id4_IdentityClaim");

            migrationBuilder.DropTable(
                name: "Id4_IdentityResourceProperty");

            migrationBuilder.DropTable(
                name: "Id4_PersistedGrant");

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
                name: "Id4_ApiScope");

            migrationBuilder.DropTable(
                name: "Id4_Client");

            migrationBuilder.DropTable(
                name: "Id4_IdentityResource");

            migrationBuilder.DropTable(
                name: "Identity_Role");

            migrationBuilder.DropTable(
                name: "Systems_AuditEntity");

            migrationBuilder.DropTable(
                name: "Id4_ApiResource");

            migrationBuilder.DropTable(
                name: "Systems_AuditOperation");

            migrationBuilder.DropTable(
                name: "Identity_User");

            migrationBuilder.DropTable(
                name: "Infos_Message");
        }
    }
}
