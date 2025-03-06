using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OSharp.CodeGenerator.Migrations
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TypeName = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    AuditEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    PropertyJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_EntityInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Function",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Area = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Controller = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Action = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsController = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAjax = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAccessTypeChanged = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditOperationEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditEntityEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CacheExpirationSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCacheSliding = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSlaveDatabase = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Function", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeGen_CodeProject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    NamespacePrefix = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Company = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    SiteUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Creator = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Copyright = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RootPath = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGen_CodeProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeGen_CodeTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    MetadataType = table.Column<int>(type: "INTEGER", nullable: false),
                    TemplateFile = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    OutputFileFormat = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    IsOnce = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSystem = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGen_CodeTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Systems_KeyValue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    ValueJson = table.Column<string>(type: "text", nullable: true),
                    ValueType = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Display = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Remark = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems_KeyValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeGen_CodeModule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Display = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGen_CodeModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeGen_CodeModule_CodeGen_CodeProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "CodeGen_CodeProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeGen_CodeProjectTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TemplateId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGen_CodeProjectTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeGen_CodeProjectTemplate_CodeGen_CodeProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "CodeGen_CodeProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeGen_CodeProjectTemplate_CodeGen_CodeTemplate_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "CodeGen_CodeTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CodeGen_CodeEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Display = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PrimaryKeyTypeFullName = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Listable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Addable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Updatable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Deletable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDataAuth = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasCreatedTime = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasSoftDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasCreationAudited = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasUpdateAudited = table.Column<bool>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModuleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGen_CodeEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeGen_CodeEntity_CodeGen_CodeModule_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "CodeGen_CodeModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeGen_CodeForeign",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SelfNavigation = table.Column<string>(type: "TEXT", nullable: true),
                    SelfForeignKey = table.Column<string>(type: "TEXT", nullable: true),
                    OtherEntity = table.Column<string>(type: "TEXT", nullable: true),
                    OtherNavigation = table.Column<string>(type: "TEXT", nullable: true),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeleteBehavior = table.Column<int>(type: "INTEGER", nullable: true),
                    ForeignRelation = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGen_CodeForeign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeGen_CodeForeign_CodeGen_CodeEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "CodeGen_CodeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeGen_CodeProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Display = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    TypeName = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Listable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Updatable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sortable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Filterable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: true),
                    MinLength = table.Column<int>(type: "INTEGER", nullable: true),
                    MaxLength = table.Column<int>(type: "INTEGER", nullable: true),
                    IsNullable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEnum = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsReadonly = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsHide = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsVirtual = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsForeignKey = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsNavigation = table.Column<bool>(type: "INTEGER", nullable: false),
                    RelateEntity = table.Column<string>(type: "TEXT", nullable: true),
                    DataAuthFlag = table.Column<string>(type: "TEXT", nullable: true),
                    IsInputDto = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsOutputDto = table.Column<bool>(type: "INTEGER", nullable: false),
                    DefaultValue = table.Column<string>(type: "TEXT", nullable: true),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    EntityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGen_CodeProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeGen_CodeProperty_CodeGen_CodeEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "CodeGen_CodeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ClassFullNameIndex",
                table: "Auth_EntityInfo",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AreaControllerActionIndex",
                table: "Auth_Function",
                columns: new[] { "Area", "Controller", "Action" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeGen_CodeEntity_ModuleId",
                table: "CodeGen_CodeEntity",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGen_CodeForeign_EntityId",
                table: "CodeGen_CodeForeign",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGen_CodeModule_ProjectId",
                table: "CodeGen_CodeModule",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGen_CodeProjectTemplate_ProjectId",
                table: "CodeGen_CodeProjectTemplate",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGen_CodeProjectTemplate_TemplateId",
                table: "CodeGen_CodeProjectTemplate",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGen_CodeProperty_EntityId",
                table: "CodeGen_CodeProperty",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "KeyIndex",
                table: "Systems_KeyValue",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auth_EntityInfo");

            migrationBuilder.DropTable(
                name: "Auth_Function");

            migrationBuilder.DropTable(
                name: "CodeGen_CodeForeign");

            migrationBuilder.DropTable(
                name: "CodeGen_CodeProjectTemplate");

            migrationBuilder.DropTable(
                name: "CodeGen_CodeProperty");

            migrationBuilder.DropTable(
                name: "Systems_KeyValue");

            migrationBuilder.DropTable(
                name: "CodeGen_CodeTemplate");

            migrationBuilder.DropTable(
                name: "CodeGen_CodeEntity");

            migrationBuilder.DropTable(
                name: "CodeGen_CodeModule");

            migrationBuilder.DropTable(
                name: "CodeGen_CodeProject");
        }
    }
}
