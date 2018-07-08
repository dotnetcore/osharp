import { Component, AfterViewInit, Injector, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { List } from 'linqts';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';


@Component({
  selector: 'admin-identity-user',
  templateUrl: './user.component.html'
})
export class UserComponent extends GridComponentBase implements AfterViewInit {

  roleWindow: kendo.ui.Window;
  moduleWindow: kendo.ui.Window;
  windowOptions: kendo.ui.WindowOptions;
  tabstripOptions: kendo.ui.TabStripOptions;
  roleTreeOptions: kendo.ui.TreeViewOptions;
  roleTree: kendo.ui.TreeView;
  moduleTreeOptions: kendo.ui.TreeViewOptions;
  moduleTree: kendo.ui.TreeView;

  http: HttpClient;

  constructor(injector: Injector) {
    super(injector);
    this.http = injector.get(HttpClient);
    this.moduleName = "user";
    this.windowOptions = {
      visible: false, width: 500, height: 620, modal: true, title: "用户权限设置", actions: ["Pin", "Minimize", "Maximize", "Close"],
      resize: e => this.onWinResize(e)
    };
    this.roleTreeOptions = { checkboxes: { checkChildren: true, }, dataTextField: "Name", select: e => this.kendoui.OnTreeNodeSelect(e) };
    this.moduleTreeOptions = { checkboxes: { checkChildren: true }, dataTextField: "Name", select: e => this.kendoui.OnTreeNodeSelect(e) };
  }

  async ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    } else {
      this.osharp.error("无权查看该页面");
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Identity.User", ["Read", "Create", "Update", "Delete", "SetRoles", "SetModules"]);
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
        LockoutEnabled: { type: "boolean" },
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
          { name: "setRoles", text: "", iconClass: "k-icon k-i-link-horizontal", click: e => this.roleWindowOpen(e) },
          { name: "setModules", text: "", iconClass: "k-icon k-i-unlink-horizontal", click: e => this.moduleWindowOpen(e) },
          { name: "destroy", iconClass: "k-icon k-i-delete", text: "" },
        ],
        width: 100
      },
      { field: "Id", title: "编号", width: 70 },
      {
        field: "UserName",
        title: "用户名",
        width: 150,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "NickName",
        title: "昵称",
        width: 130,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "Email",
        title: "邮箱",
        width: 180,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "EmailConfirmed",
        title: "邮箱确认",
        width: 95,
        template: d => this.kendoui.Boolean(d.EmailConfirmed),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "PhoneNumber",
        title: "手机号",
        width: 105,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "PhoneNumberConfirmed",
        title: "手机确认",
        width: 95,
        template: d => this.kendoui.Boolean(d.PhoneNumberConfirmed),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "Roles",
        title: "角色",
        width: 180,
        template: d => this.osharp.expandAndToString(d.Roles)
      }, {
        field: "Locked",
        title: "是否锁定",
        width: 95,
        template: d => this.kendoui.Boolean(d.Locked),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "LockoutEnabled",
        title: "登录锁",
        width: 90,
        template: d => this.kendoui.Boolean(d.LockoutEnabled),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "LockoutEnd",
        title: "锁时间",
        width: 120,
        format: "{0:yy-MM-dd HH:mm}"
      }, {
        field: "AccessFailedCount",
        title: "登录错误",
        width: 95
      }, {
        field: "CreatedTime",
        title: "注册时间",
        width: 120,
        format: "{0:yy-MM-dd HH:mm}"
      }
    ];
  }

  protected FilterGridAuth(options: kendo.ui.GridOptions) {
    // 命令列
    let cmdColumn = options.columns && options.columns.find(m => m.command != null);
    let cmds = cmdColumn && cmdColumn.command as kendo.ui.GridColumnCommandItem[];
    if (cmds) {
      if (!this.auth.SetRoles) {
        this.osharp.remove(cmds, m => m.name == "setRoles");
      }
      if (!this.auth.SetModules) {
        this.osharp.remove(cmds, m => m.name == "setModules");
      }
    }
    options = super.FilterGridAuth(options);
    return options;
  }
  //#endregion

  //#region Window

  private winUser: any;

  //#region RoleWindow

  onRoleWinInit(win) {
    this.roleWindow = win;
  }
  private roleWindowOpen(e) {
    e.preventDefault();
    let tr = $(e.target).closest("tr");
    this.winUser = this.grid.dataItem(tr);
    this.roleWindow.title("用户角色设置-" + this.winUser.UserName).open().center().resize();
    this.roleTree.setDataSource(this.kendoui.CreateHierarchicalDataSource("api/admin/role/ReadUserRoles?userId=" + this.winUser.Id));
  }
  onRoleWinSubmit() {
    let roles = this.roleTree.dataSource.data();
    let checkRoleIds = new List(roles.slice(0)).Where(m => m.checked).Select(m => m.Id).ToArray();

    let params = { userId: this.winUser.Id, roleIds: checkRoleIds };

    this.http.post("api/admin/user/setroles", params).subscribe(res => {
      this.osharp.ajaxResult(res, () => {
        this.grid.dataSource.read();
        this.roleWindow.close();
      });
    });
  }

  //#endregion

  //#region ModuleWindow

  onModuleWinInit(win) {
    this.moduleWindow = win;
  }
  private moduleWindowOpen(e) {
    e.preventDefault();
    let tr = $(e.target).closest("tr");
    this.winUser = this.grid.dataItem(tr);
    this.moduleWindow.title("用户模块设置-" + this.winUser.UserName).open().center().resize();
    this.moduleTree.setDataSource(this.kendoui.CreateHierarchicalDataSource("api/admin/module/ReadUserModules?userId=" + this.winUser.Id));
  }
  onModuleWinSubmit() {
    let moduleRoot = this.moduleTree.dataSource.data()[0];
    let modules = [];
    this.osharp.getTreeNodes(moduleRoot, modules);
    let checkModuleIds = new List(modules).Where(m => m.checked).Select(m => m.Id).ToArray();
    let params = { userId: this.winUser.Id, moduleIds: checkModuleIds };

    this.http.post("api/admin/user/setmodules", params).subscribe(res => {
      this.osharp.ajaxResult(res, () => {
        this.grid.dataSource.read();
        this.moduleWindow.close();
      });
    });
  }

  //#endregion

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
