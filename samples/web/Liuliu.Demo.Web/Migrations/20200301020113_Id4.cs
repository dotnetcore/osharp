using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Liuliu.Demo.Web.Migrations
{
    public partial class Id4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Id4_ApiScope");

            migrationBuilder.DropTable(
                name: "Id4_Client");

            migrationBuilder.DropTable(
                name: "Id4_IdentityResource");

            migrationBuilder.DropTable(
                name: "Id4_ApiResource");
        }
    }
}
