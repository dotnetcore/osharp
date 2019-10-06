using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Liuliu.Demo.Web.Migrations
{
    public partial class Message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "Role",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Message",
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
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageReceive",
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
                    table.PrimaryKey("PK_MessageReceive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageReceive_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageReceive_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageReply",
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
                    table.PrimaryKey("PK_MessageReply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageReply_Message_BelongMessageId",
                        column: x => x.BelongMessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageReply_Message_ParentMessageId",
                        column: x => x.ParentMessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageReply_MessageReply_ParentReplyId",
                        column: x => x.ParentReplyId,
                        principalTable: "MessageReply",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageReply_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_MessageId",
                table: "User",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_MessageId",
                table: "Role",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReceive_MessageId",
                table: "MessageReceive",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReceive_UserId",
                table: "MessageReceive",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReply_BelongMessageId",
                table: "MessageReply",
                column: "BelongMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReply_ParentMessageId",
                table: "MessageReply",
                column: "ParentMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReply_ParentReplyId",
                table: "MessageReply",
                column: "ParentReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReply_UserId",
                table: "MessageReply",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Message_MessageId",
                table: "Role",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Message_MessageId",
                table: "User",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Message_MessageId",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Message_MessageId",
                table: "User");

            migrationBuilder.DropTable(
                name: "MessageReceive");

            migrationBuilder.DropTable(
                name: "MessageReply");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropIndex(
                name: "IX_User_MessageId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Role_MessageId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Role");
        }
    }
}
