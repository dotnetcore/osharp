using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liuliu.Demo.Web.Migrations
{
    /// <inheritdoc />
    public partial class Comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Systems_Menu",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                comment: "链接",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TreePathString",
                table: "Systems_Menu",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true,
                comment: "父节点树形路径",
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Systems_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "显示",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Target",
                table: "Systems_Menu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "链接目标",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Systems_Menu",
                type: "int",
                nullable: true,
                comment: "父菜单编号",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "OrderCode",
                table: "Systems_Menu",
                type: "float",
                nullable: false,
                comment: "节点内排序",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Systems_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSystem",
                table: "Systems_Menu",
                type: "bit",
                nullable: false,
                comment: "是否系统",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabled",
                table: "Systems_Menu",
                type: "bit",
                nullable: false,
                comment: "是否启用",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Systems_Menu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "图标",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Systems_Menu",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                comment: "菜单数据",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Acl",
                table: "Systems_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "访问控制列表",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Systems_Menu",
                type: "int",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "ValueType",
                table: "Systems_KeyValue",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "数据值类型名",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ValueJson",
                table: "Systems_KeyValue",
                type: "text",
                nullable: true,
                comment: "数据值JSON",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Systems_KeyValue",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "显示名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "Systems_KeyValue",
                type: "int",
                nullable: false,
                comment: "Order",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Systems_KeyValue",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                comment: "数据键名",
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Systems_KeyValue",
                type: "bit",
                nullable: false,
                comment: "是否锁定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Display",
                table: "Systems_KeyValue",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "显示名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Systems_KeyValue",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalValue",
                table: "Systems_AuditProperty",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                comment: "旧值",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NewValue",
                table: "Systems_AuditProperty",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                comment: "新值",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "Systems_AuditProperty",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "字段",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Systems_AuditProperty",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "Systems_AuditProperty",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "数据类型",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AuditEntityId",
                table: "Systems_AuditProperty",
                type: "uniqueidentifier",
                nullable: false,
                comment: "AuditEntityId",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Systems_AuditProperty",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Systems_AuditOperation",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                comment: "当前用户名",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Systems_AuditOperation",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "当前用户标识",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "Systems_AuditOperation",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "UserAgent",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResultType",
                table: "Systems_AuditOperation",
                type: "int",
                nullable: false,
                comment: "ResultType",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "OperationSystem",
                table: "Systems_AuditOperation",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                comment: "操作系统",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "Systems_AuditOperation",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                comment: "当前用户昵称",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Systems_AuditOperation",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "消息",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "Systems_AuditOperation",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "当前访问IP",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FunctionName",
                table: "Systems_AuditOperation",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "执行的功能名",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Elapsed",
                table: "Systems_AuditOperation",
                type: "int",
                nullable: false,
                comment: "Elapsed",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Systems_AuditOperation",
                type: "datetime2",
                nullable: false,
                comment: "CreatedTime",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Browser",
                table: "Systems_AuditOperation",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "浏览器",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Systems_AuditOperation",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Systems_AuditEntity",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "类型名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<Guid>(
                name: "OperationId",
                table: "Systems_AuditEntity",
                type: "uniqueidentifier",
                nullable: false,
                comment: "OperationId",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "OperateType",
                table: "Systems_AuditEntity",
                type: "int",
                nullable: false,
                comment: "OperateType",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Systems_AuditEntity",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "实体名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "EntityKey",
                table: "Systems_AuditEntity",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "类型名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Systems_AuditEntity",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Infos_MessageReply",
                type: "int",
                nullable: false,
                comment: " 消息回复人编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentReplyId",
                table: "Infos_MessageReply",
                type: "uniqueidentifier",
                nullable: false,
                comment: "回复的回复消息，当回复回复消息时有效",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentMessageId",
                table: "Infos_MessageReply",
                type: "uniqueidentifier",
                nullable: false,
                comment: "回复的主消息，当回复主消息时有效",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "Infos_MessageReply",
                type: "bit",
                nullable: false,
                comment: "是否已读",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Infos_MessageReply",
                type: "bit",
                nullable: false,
                comment: "是否锁定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Infos_MessageReply",
                type: "datetime2",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Infos_MessageReply",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Infos_MessageReply",
                type: "nvarchar(max)",
                nullable: false,
                comment: "消息内容",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "BelongMessageId",
                table: "Infos_MessageReply",
                type: "uniqueidentifier",
                nullable: false,
                comment: "回复所属主消息，用于避免递归查询",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Infos_MessageReply",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Infos_MessageReceive",
                type: "int",
                nullable: false,
                comment: "消息接收人编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadDate",
                table: "Infos_MessageReceive",
                type: "datetime2",
                nullable: false,
                comment: "接收时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "NewReplyCount",
                table: "Infos_MessageReceive",
                type: "int",
                nullable: false,
                comment: "新回复数，接收者使用",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "MessageId",
                table: "Infos_MessageReceive",
                type: "uniqueidentifier",
                nullable: false,
                comment: "接收的主消息编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Infos_MessageReceive",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Infos_MessageReceive",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Infos_Message",
                type: "nvarchar(max)",
                nullable: false,
                comment: "标题",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                table: "Infos_Message",
                type: "int",
                nullable: false,
                comment: "发送人编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NewReplyCount",
                table: "Infos_Message",
                type: "int",
                nullable: false,
                comment: "新回复数",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MessageType",
                table: "Infos_Message",
                type: "int",
                nullable: false,
                comment: "消息类型",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSended",
                table: "Infos_Message",
                type: "bit",
                nullable: false,
                comment: "是否发送",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Infos_Message",
                type: "bit",
                nullable: false,
                comment: "是否锁定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Infos_Message",
                type: "datetime2",
                nullable: true,
                comment: "过期时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Infos_Message",
                type: "datetime2",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Infos_Message",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Infos_Message",
                type: "nvarchar(max)",
                nullable: false,
                comment: "内容",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "CanReply",
                table: "Infos_Message",
                type: "bit",
                nullable: false,
                comment: "是否允许回复",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BeginDate",
                table: "Infos_Message",
                type: "datetime2",
                nullable: true,
                comment: "生效时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Infos_Message",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Identity_UserToken",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                comment: "令牌值",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserToken",
                type: "int",
                nullable: false,
                comment: "用户编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Identity_UserToken",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "令牌名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "Identity_UserToken",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "登录提供者",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identity_UserToken",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserRole",
                type: "int",
                nullable: false,
                comment: "用户编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Identity_UserRole",
                type: "int",
                nullable: false,
                comment: "角色编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Identity_UserRole",
                type: "bit",
                nullable: false,
                comment: "是否锁定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Identity_UserRole",
                type: "datetime2",
                nullable: true,
                comment: "DeletedTime",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_UserRole",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identity_UserRole",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserLogin",
                type: "int",
                nullable: false,
                comment: "用户编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "Identity_UserLogin",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "第三方用户的唯一标识",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "Identity_UserLogin",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "第三方用户昵称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "Identity_UserLogin",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "登录的登录提供程序",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_UserLogin",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "Identity_UserLogin",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "第三方用户头像",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identity_UserLogin",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserDetail",
                type: "int",
                nullable: false,
                comment: "用户编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "RegisterIp",
                table: "Identity_UserDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "注册IP",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_UserDetail",
                type: "int",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserClaim",
                type: "int",
                nullable: false,
                comment: "用户编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "Identity_UserClaim",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "声明值",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "Identity_UserClaim",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "声明类型",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_UserClaim",
                type: "int",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "用户名",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                comment: "双因子身份验证",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "Identity_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "安全标识",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Identity_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "备注",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                comment: "手机号码确定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Identity_User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "手机号码",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Identity_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "密码哈希值",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "标准化的用户名",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizeEmail",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "标准化的电子邮箱",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "用户昵称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Identity_User",
                type: "datetimeoffset",
                nullable: true,
                comment: "锁定时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "LockoutEnabled",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                comment: "是否登录锁",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSystem",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                comment: "是否系统用户",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                comment: "是否锁定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "HeadImg",
                table: "Identity_User",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "用户头像",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EmailConfirmed",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                comment: "电子邮箱确认",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "电子邮箱",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Identity_User",
                type: "datetime2",
                nullable: true,
                comment: "DeletedTime",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_User",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Identity_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "版本标识",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccessFailedCount",
                table: "Identity_User",
                type: "int",
                nullable: false,
                comment: "登录失败次数",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_User",
                type: "int",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Identity_RoleClaim",
                type: "int",
                nullable: false,
                comment: "角色编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "Identity_RoleClaim",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "声明值",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "Identity_RoleClaim",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "声明类型",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_RoleClaim",
                type: "int",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Identity_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "角色描述",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "Identity_Role",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "标准化角色名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Identity_Role",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "角色名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSystem",
                table: "Identity_Role",
                type: "bit",
                nullable: false,
                comment: "是否系统角色",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Identity_Role",
                type: "bit",
                nullable: false,
                comment: "是否锁定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "Identity_Role",
                type: "bit",
                nullable: false,
                comment: "是否默认角色",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAdmin",
                table: "Identity_Role",
                type: "bit",
                nullable: false,
                comment: "是否管理员角色",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Identity_Role",
                type: "datetime2",
                nullable: true,
                comment: "DeletedTime",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_Role",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Identity_Role",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "版本标识",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_Role",
                type: "int",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Identity_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "描述",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Identity_Organization",
                type: "int",
                nullable: true,
                comment: "父组织机构编号",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Identity_Organization",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_Organization",
                type: "int",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_LoginLog",
                type: "int",
                nullable: false,
                comment: "用户编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "Identity_LoginLog",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "用户代理头",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LogoutTime",
                table: "Identity_LoginLog",
                type: "datetime2",
                nullable: true,
                comment: "退出时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "Identity_LoginLog",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "登录IP",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_LoginLog",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identity_LoginLog",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Auth_ModuleUser",
                type: "int",
                nullable: false,
                comment: "用户编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Auth_ModuleUser",
                type: "int",
                nullable: false,
                comment: "模块编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Disabled",
                table: "Auth_ModuleUser",
                type: "bit",
                nullable: false,
                comment: "Disabled",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_ModuleUser",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Auth_ModuleRole",
                type: "int",
                nullable: false,
                comment: "角色编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Auth_ModuleRole",
                type: "int",
                nullable: false,
                comment: "模块编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_ModuleRole",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Auth_ModuleFunction",
                type: "int",
                nullable: false,
                comment: "模块编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "FunctionId",
                table: "Auth_ModuleFunction",
                type: "uniqueidentifier",
                nullable: false,
                comment: "功能编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_ModuleFunction",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "TreePathString",
                table: "Auth_Module",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                comment: "父节点树形路径",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Auth_Module",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "模块描述",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Auth_Module",
                type: "int",
                nullable: true,
                comment: "父模块编号",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "OrderCode",
                table: "Auth_Module",
                type: "float",
                nullable: false,
                comment: "排序码",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Auth_Module",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "模块名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Auth_Module",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "模块代码",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Auth_Module",
                type: "int",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Auth_Function",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSlaveDatabase",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                comment: "是否从库",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                comment: "是否锁定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsController",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                comment: "是否控制器",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCacheSliding",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                comment: "是否相对过期时间",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAjax",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                comment: "是否Ajax功能",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAccessTypeChanged",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                comment: "访问类型是否更改",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "Auth_Function",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "控制器",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CacheExpirationSeconds",
                table: "Auth_Function",
                type: "int",
                nullable: false,
                comment: "数据缓存秒数",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "AuditOperationEnabled",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                comment: "是否操作审计",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "AuditEntityEnabled",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                comment: "是否数据审计",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "Auth_Function",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "区域",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "Auth_Function",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "功能",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccessType",
                table: "Auth_Function",
                type: "int",
                nullable: false,
                comment: "访问类型",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_Function",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Auth_EntityUser",
                type: "int",
                nullable: false,
                comment: "用户编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Auth_EntityUser",
                type: "bit",
                nullable: false,
                comment: "是否锁定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FilterGroupJson",
                table: "Auth_EntityUser",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                comment: "过滤条件组Json字符串",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "Auth_EntityUser",
                type: "uniqueidentifier",
                nullable: false,
                comment: "数据编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Auth_EntityUser",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_EntityUser",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Auth_EntityRole",
                type: "int",
                nullable: false,
                comment: "角色编号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Operation",
                table: "Auth_EntityRole",
                type: "int",
                nullable: false,
                comment: "数据权限操作",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Auth_EntityRole",
                type: "bit",
                nullable: false,
                comment: "是否锁定",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FilterGroupJson",
                table: "Auth_EntityRole",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                comment: "过滤条件组Json字符串",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "Auth_EntityRole",
                type: "uniqueidentifier",
                nullable: false,
                comment: "数据编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Auth_EntityRole",
                type: "datetime2",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_EntityRole",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Auth_EntityInfo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "实体类型名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyJson",
                table: "Auth_EntityInfo",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                comment: "实体属性信息Json字符串",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Auth_EntityInfo",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "实体名称",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "AuditEnabled",
                table: "Auth_EntityInfo",
                type: "bit",
                nullable: false,
                comment: "是否数据审计",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_EntityInfo",
                type: "uniqueidentifier",
                nullable: false,
                comment: "编号",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Systems_Menu",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true,
                oldComment: "链接");

            migrationBuilder.AlterColumn<string>(
                name: "TreePathString",
                table: "Systems_Menu",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000,
                oldNullable: true,
                oldComment: "父节点树形路径");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Systems_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "显示");

            migrationBuilder.AlterColumn<string>(
                name: "Target",
                table: "Systems_Menu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "链接目标");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Systems_Menu",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "父菜单编号");

            migrationBuilder.AlterColumn<double>(
                name: "OrderCode",
                table: "Systems_Menu",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "节点内排序");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Systems_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSystem",
                table: "Systems_Menu",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否系统");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabled",
                table: "Systems_Menu",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否启用");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Systems_Menu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "图标");

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Systems_Menu",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true,
                oldComment: "菜单数据");

            migrationBuilder.AlterColumn<string>(
                name: "Acl",
                table: "Systems_Menu",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "访问控制列表");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Systems_Menu",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "编号")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "ValueType",
                table: "Systems_KeyValue",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "数据值类型名");

            migrationBuilder.AlterColumn<string>(
                name: "ValueJson",
                table: "Systems_KeyValue",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "数据值JSON");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Systems_KeyValue",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "显示名称");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "Systems_KeyValue",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Systems_KeyValue",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldComment: "数据键名");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Systems_KeyValue",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否锁定");

            migrationBuilder.AlterColumn<string>(
                name: "Display",
                table: "Systems_KeyValue",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "显示名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Systems_KeyValue",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalValue",
                table: "Systems_AuditProperty",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true,
                oldComment: "旧值");

            migrationBuilder.AlterColumn<string>(
                name: "NewValue",
                table: "Systems_AuditProperty",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true,
                oldComment: "新值");

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "Systems_AuditProperty",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "字段");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Systems_AuditProperty",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "名称");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "Systems_AuditProperty",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "数据类型");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuditEntityId",
                table: "Systems_AuditProperty",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "AuditEntityId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Systems_AuditProperty",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Systems_AuditOperation",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true,
                oldComment: "当前用户名");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Systems_AuditOperation",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "当前用户标识");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "Systems_AuditOperation",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "UserAgent");

            migrationBuilder.AlterColumn<int>(
                name: "ResultType",
                table: "Systems_AuditOperation",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "ResultType");

            migrationBuilder.AlterColumn<string>(
                name: "OperationSystem",
                table: "Systems_AuditOperation",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true,
                oldComment: "操作系统");

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "Systems_AuditOperation",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true,
                oldComment: "当前用户昵称");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Systems_AuditOperation",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "消息");

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "Systems_AuditOperation",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "当前访问IP");

            migrationBuilder.AlterColumn<string>(
                name: "FunctionName",
                table: "Systems_AuditOperation",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "执行的功能名");

            migrationBuilder.AlterColumn<int>(
                name: "Elapsed",
                table: "Systems_AuditOperation",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Elapsed");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Systems_AuditOperation",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "CreatedTime");

            migrationBuilder.AlterColumn<string>(
                name: "Browser",
                table: "Systems_AuditOperation",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "浏览器");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Systems_AuditOperation",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Systems_AuditEntity",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "类型名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "OperationId",
                table: "Systems_AuditEntity",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "OperationId");

            migrationBuilder.AlterColumn<int>(
                name: "OperateType",
                table: "Systems_AuditEntity",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "OperateType");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Systems_AuditEntity",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "实体名称");

            migrationBuilder.AlterColumn<string>(
                name: "EntityKey",
                table: "Systems_AuditEntity",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "类型名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Systems_AuditEntity",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Infos_MessageReply",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: " 消息回复人编号");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentReplyId",
                table: "Infos_MessageReply",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "回复的回复消息，当回复回复消息时有效");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentMessageId",
                table: "Infos_MessageReply",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "回复的主消息，当回复主消息时有效");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "Infos_MessageReply",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否已读");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Infos_MessageReply",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否锁定");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Infos_MessageReply",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Infos_MessageReply",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Infos_MessageReply",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "消息内容");

            migrationBuilder.AlterColumn<Guid>(
                name: "BelongMessageId",
                table: "Infos_MessageReply",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "回复所属主消息，用于避免递归查询");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Infos_MessageReply",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Infos_MessageReceive",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "消息接收人编号");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadDate",
                table: "Infos_MessageReceive",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "接收时间");

            migrationBuilder.AlterColumn<int>(
                name: "NewReplyCount",
                table: "Infos_MessageReceive",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "新回复数，接收者使用");

            migrationBuilder.AlterColumn<Guid>(
                name: "MessageId",
                table: "Infos_MessageReceive",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "接收的主消息编号");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Infos_MessageReceive",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Infos_MessageReceive",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Infos_Message",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "标题");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                table: "Infos_Message",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "发送人编号");

            migrationBuilder.AlterColumn<int>(
                name: "NewReplyCount",
                table: "Infos_Message",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "新回复数");

            migrationBuilder.AlterColumn<int>(
                name: "MessageType",
                table: "Infos_Message",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "消息类型");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSended",
                table: "Infos_Message",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否发送");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Infos_Message",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否锁定");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Infos_Message",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "过期时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Infos_Message",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Infos_Message",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Infos_Message",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "内容");

            migrationBuilder.AlterColumn<bool>(
                name: "CanReply",
                table: "Infos_Message",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否允许回复");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BeginDate",
                table: "Infos_Message",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "生效时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Infos_Message",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Identity_UserToken",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true,
                oldComment: "令牌值");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserToken",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "用户编号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Identity_UserToken",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "令牌名称");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "Identity_UserToken",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "登录提供者");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identity_UserToken",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "用户编号");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Identity_UserRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "角色编号");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Identity_UserRole",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否锁定");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Identity_UserRole",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "DeletedTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_UserRole",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identity_UserRole",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserLogin",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "用户编号");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "Identity_UserLogin",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "第三方用户的唯一标识");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "Identity_UserLogin",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "第三方用户昵称");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "Identity_UserLogin",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "登录的登录提供程序");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_UserLogin",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "Identity_UserLogin",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "第三方用户头像");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identity_UserLogin",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserDetail",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "用户编号");

            migrationBuilder.AlterColumn<string>(
                name: "RegisterIp",
                table: "Identity_UserDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "注册IP");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_UserDetail",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "编号")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_UserClaim",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "用户编号");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "Identity_UserClaim",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "声明值");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "Identity_UserClaim",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "声明类型");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_UserClaim",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "编号")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "用户名");

            migrationBuilder.AlterColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "双因子身份验证");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "Identity_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "安全标识");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Identity_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "备注");

            migrationBuilder.AlterColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "手机号码确定");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Identity_User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "手机号码");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Identity_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "密码哈希值");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "标准化的用户名");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizeEmail",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "标准化的电子邮箱");

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "用户昵称");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Identity_User",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "锁定时间");

            migrationBuilder.AlterColumn<bool>(
                name: "LockoutEnabled",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否登录锁");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSystem",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否系统用户");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否锁定");

            migrationBuilder.AlterColumn<string>(
                name: "HeadImg",
                table: "Identity_User",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "用户头像");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailConfirmed",
                table: "Identity_User",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "电子邮箱确认");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Identity_User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "电子邮箱");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Identity_User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "DeletedTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_User",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Identity_User",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "版本标识");

            migrationBuilder.AlterColumn<int>(
                name: "AccessFailedCount",
                table: "Identity_User",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "登录失败次数");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_User",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "编号")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Identity_RoleClaim",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "角色编号");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "Identity_RoleClaim",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "声明值");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "Identity_RoleClaim",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "声明类型");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_RoleClaim",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "编号")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Identity_Role",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "角色描述");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "Identity_Role",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "标准化角色名称");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Identity_Role",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "角色名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSystem",
                table: "Identity_Role",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否系统角色");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Identity_Role",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否锁定");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "Identity_Role",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否默认角色");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAdmin",
                table: "Identity_Role",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否管理员角色");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedTime",
                table: "Identity_Role",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "DeletedTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_Role",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Identity_Role",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "版本标识");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_Role",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "编号")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Identity_Organization",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "描述");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Identity_Organization",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "父组织机构编号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Identity_Organization",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "名称");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Identity_Organization",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "编号")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Identity_LoginLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "用户编号");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "Identity_LoginLog",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "用户代理头");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LogoutTime",
                table: "Identity_LoginLog",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "退出时间");

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "Identity_LoginLog",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "登录IP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Identity_LoginLog",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identity_LoginLog",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Auth_ModuleUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "用户编号");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Auth_ModuleUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "模块编号");

            migrationBuilder.AlterColumn<bool>(
                name: "Disabled",
                table: "Auth_ModuleUser",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Disabled");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_ModuleUser",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Auth_ModuleRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "角色编号");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Auth_ModuleRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "模块编号");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_ModuleRole",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Auth_ModuleFunction",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "模块编号");

            migrationBuilder.AlterColumn<Guid>(
                name: "FunctionId",
                table: "Auth_ModuleFunction",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "功能编号");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_ModuleFunction",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<string>(
                name: "TreePathString",
                table: "Auth_Module",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true,
                oldComment: "父节点树形路径");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "Auth_Module",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "模块描述");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Auth_Module",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "父模块编号");

            migrationBuilder.AlterColumn<double>(
                name: "OrderCode",
                table: "Auth_Module",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "排序码");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Auth_Module",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "模块名称");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Auth_Module",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "模块代码");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Auth_Module",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "编号")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Auth_Function",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSlaveDatabase",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否从库");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否锁定");

            migrationBuilder.AlterColumn<bool>(
                name: "IsController",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否控制器");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCacheSliding",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否相对过期时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAjax",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否Ajax功能");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAccessTypeChanged",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "访问类型是否更改");

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "Auth_Function",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "控制器");

            migrationBuilder.AlterColumn<int>(
                name: "CacheExpirationSeconds",
                table: "Auth_Function",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "数据缓存秒数");

            migrationBuilder.AlterColumn<bool>(
                name: "AuditOperationEnabled",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否操作审计");

            migrationBuilder.AlterColumn<bool>(
                name: "AuditEntityEnabled",
                table: "Auth_Function",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否数据审计");

            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "Auth_Function",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "区域");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "Auth_Function",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "功能");

            migrationBuilder.AlterColumn<int>(
                name: "AccessType",
                table: "Auth_Function",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "访问类型");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_Function",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Auth_EntityUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "用户编号");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Auth_EntityUser",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否锁定");

            migrationBuilder.AlterColumn<string>(
                name: "FilterGroupJson",
                table: "Auth_EntityUser",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true,
                oldComment: "过滤条件组Json字符串");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "Auth_EntityUser",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "数据编号");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Auth_EntityUser",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_EntityUser",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Auth_EntityRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "角色编号");

            migrationBuilder.AlterColumn<int>(
                name: "Operation",
                table: "Auth_EntityRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "数据权限操作");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Auth_EntityRole",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否锁定");

            migrationBuilder.AlterColumn<string>(
                name: "FilterGroupJson",
                table: "Auth_EntityRole",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true,
                oldComment: "过滤条件组Json字符串");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "Auth_EntityRole",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "数据编号");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Auth_EntityRole",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_EntityRole",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Auth_EntityInfo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "实体类型名称");

            migrationBuilder.AlterColumn<string>(
                name: "PropertyJson",
                table: "Auth_EntityInfo",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldComment: "实体属性信息Json字符串");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Auth_EntityInfo",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComment: "实体名称");

            migrationBuilder.AlterColumn<bool>(
                name: "AuditEnabled",
                table: "Auth_EntityInfo",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否数据审计");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Auth_EntityInfo",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "编号");
        }
    }
}
