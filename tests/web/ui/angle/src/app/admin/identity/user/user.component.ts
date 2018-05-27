import { Component, OnInit, OnDestroy, AfterViewInit, NgZone, ElementRef } from '@angular/core';
import { List } from "linqts";
import { HttpClient } from '@angular/common/http';

import { kendoui } from '../../../shared/kendoui';
import { osharp } from '../../../shared/osharp';
import { AuthService } from '../../../shared/osharp/services/auth.service';

@Component({
  selector: 'identity-user',
  templateUrl: './user.component.html'
})
export class UserComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

  window: kendo.ui.Window;
  windowOptions: kendo.ui.WindowOptions;
  tabstripOptions: kendo.ui.TabStripOptions;
  roleTreeOptions: kendo.ui.TreeViewOptions;
  roleTree: kendo.ui.TreeView;
  moduleTreeOptions: kendo.ui.TreeViewOptions;
  moduleTree: kendo.ui.TreeView;

  constructor(protected zone: NgZone, protected el: ElementRef, private http: HttpClient, private auth: AuthService) {
    super(zone, el);
    this.moduleName = "user";
    this.windowOptions = {
      visible: false, width: 500, height: 620, modal: true, title: "用户权限设置", actions: ["Pin", "Minimize", "Maximize", "Close"],
      resize: e => this.onWinResize(e)
    };
    this.roleTreeOptions = { checkboxes: { checkChildren: true, }, dataTextField: "Name", select: e => kendoui.Tools.OnTreeNodeSelect(e) };
    this.moduleTreeOptions = { checkboxes: { checkChildren: true }, dataTextField: "Name", select: e => kendoui.Tools.OnTreeNodeSelect(e) };
  }

  ngOnInit() {
    super.InitBase();
  }

  ngAfterViewInit() {
    super.ViewInitBase();
    console.log(this.auth.loggedIn());
  }

  //#region GridBase

  protected GetModel() {
    return {
      id: "Id",
      fields: {
        Id: { type: "number", editable: false },
        UserName: { type: "string", validation: { required: true } },
        NickName: { type: "string", validation: { required: true } },
        Email: { type: "string", validation: { required: true } },
        EmailConfirmed: { type: "boolean" },
        PhoneNumber: { type: "string" },
        PhoneNumberConfirmed: { type: "boolean" },
        LockoutEnabled: { type: "boolean", editable: false },
        LockoutEnd: { type: "date", editable: false },
        AccessFailedCount: { type: "number", editable: false },
        CreatedTime: { type: "date", editable: false },
        Roles: { editable: false }
      }
    };
  }
  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        command: [
          { name: "permission", text: "", iconClass: "k-icon k-i-unlink-horizontal", click: e => this.windowOpen(e) },
          { name: "destroy", iconClass: "k-icon k-i-delete", text: "" }
        ],
        width: 100
      },
      { field: "Id", title: "编号", width: 70 },
      {
        field: "UserName",
        title: "用户名",
        width: 150,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "NickName",
        title: "昵称",
        width: 150,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "Email",
        title: "邮箱",
        width: 200,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "EmailConfirmed",
        title: "邮箱确认",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.EmailConfirmed),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "PhoneNumber",
        title: "手机号",
        width: 105,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "PhoneNumberConfirmed",
        title: "手机确认",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.PhoneNumberConfirmed),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "Roles",
        title: "角色",
        width: 180,
        template: d => osharp.Tools.expandAndToString(d.Roles)
      }, {
        field: "Locked",
        title: "是否锁定",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.Locked),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "LockoutEnabled",
        title: "是否登录锁",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.LockoutEnabled),
      }, {
        field: "LockoutEnd",
        title: "锁时间",
        width: 115,
        format: "{0:yy-MM-dd HH:mm}"
      }, {
        field: "AccessFailedCount",
        title: "登录错误",
        width: 95
      }, {
        field: "CreatedTime",
        title: "注册时间",
        width: 115,
        format: "{0:yy-MM-dd HH:mm}"
      }
    ];
  }

  //#endregion

  //#region Window

  private winUser: any;

  private windowOpen(e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    this.winUser = this.grid.dataItem(tr);
    this.window.title("用户权限设置-" + this.winUser.UserName).open().center().resize();
    //设置树数据
    this.roleTree.setDataSource(kendoui.Tools.CreateHierarchicalDataSource("/api/admin/role/ReadUserRoles?userId=" + this.winUser.Id));
    this.moduleTree.setDataSource(kendoui.Tools.CreateHierarchicalDataSource("/api/admin/module/ReadUserModules?userId=" + this.winUser.Id));
  }
  onWinInit(win) {
    this.window = win;
  }
  onWinClose(win) {

  }
  onWinSubmit(win) {
    var roles = this.roleTree.dataSource.data();
    var checkRoleIds = new List(roles.slice(0)).Where(m => m.checked).Select(m => m.Id).ToArray();

    var moduleRoot = this.moduleTree.dataSource.data()[0];
    var modules = [];
    osharp.Tools.getTreeNodes(moduleRoot, modules);
    var checkModuleIds = new List(modules).Where(m => m.checked).Select(m => m.Id).ToArray();
    var params = { userId: this.winUser.Id, roleIds: checkRoleIds, moduleIds: checkModuleIds };

    this.http.post("/api/admin/user/setpermission", params).subscribe(res => {
      osharp.Tools.ajaxResult(res, () => {
        this.grid.dataSource.read();
        this.window.close();
      });
    });
  }

  private onWinResize(e) {
    $(".win-content .k-tabstrip .k-content").height(e.height - 140);
  }

  //#endregion

  //#region Tree

  onRoleTreeInit(tree) {
    this.roleTree = tree;
  }
  onModuleTreeInit(tree) {
    this.moduleTree = tree;
  }

  //#endregion

}
