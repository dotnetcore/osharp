import { Component, AfterViewInit, Injector, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { List } from 'linqts';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';


@Component({
  selector: 'admin-identity-role',
  templateUrl: './role.component.html'
})
export class RoleComponent extends GridComponentBase implements AfterViewInit {

  windowOptions: kendo.ui.WindowOptions;
  window: kendo.ui.Window;
  moduleTreeOptions: kendo.ui.TreeViewOptions;
  moduleTree: kendo.ui.TreeView;
  http: HttpClient;

  constructor(injector: Injector) {
    super(injector);
    this.http = injector.get(HttpClient);
    this.moduleName = 'role';
    this.windowOptions = {
      visible: false, width: 500, height: 620, modal: true, title: "角色权限设置", actions: ["Pin", "Minimize", "Maximize", "Close"],
      resize: e => this.onWinResize(e)
    };
    this.moduleTreeOptions = { autoBind: true, checkboxes: { checkChildren: true }, dataTextField: "Name", select: e => this.kendoui.OnTreeNodeSelect(e) };
  }

  async  ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    } else {
      this.osharp.error("无权查看该页面");
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Identity.Role", ["Read", "Create", "Update", "Delete", "SetPermission"]);
  }

  //#region GridBase

  protected GetModel() {
    return {
      id: "Id",
      fields: {
        Id: { type: "number", editable: false },
        Name: { type: "string", validation: { required: true } },
        Remark: { type: "string" },
        IsAdmin: { type: "boolean" },
        IsDefault: { type: "boolean" },
        IsLocked: { type: "boolean" },
        CreatedTime: { type: "date", editable: false }
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
        field: "Name", title: "角色名", width: 150,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "Remark", title: "备注", width: 250,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "IsAdmin", title: "管理", width: 95,
        template: d => this.kendoui.Boolean(d.IsAdmin),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "IsDefault", title: "默认", width: 95,
        template: d => this.kendoui.Boolean(d.IsDefault),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "IsLocked", title: "锁定", width: 95,
        template: d => this.kendoui.Boolean(d.IsLocked),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      },
      { field: "CreatedTime", title: "注册时间", width: 120, format: "{0:yy-MM-dd HH:mm}" }
    ];
  }

  //#endregion

  //#region Window

  private winRole;
  onWinInit(win) {
    this.window = win;
  }
  windowOpen(e) {
    e.preventDefault();
    let tr = $(e.target).closest("tr");
    this.winRole = this.grid.dataItem(tr);
    this.window.title("角色权限设置-" + this.winRole.Name).open().center().resize();
    // 设置树数据
    this.moduleTree.setDataSource(this.kendoui.CreateHierarchicalDataSource("api/admin/module/ReadRoleModules?roleId=" + this.winRole.Id));
  }
  private onWinResize(e) {
    $(".win-content .k-tabstrip .k-content").height(e.height - 140);
  }
  onWinSubmit(win) {
    let moduleRoot = this.moduleTree.dataSource.data()[0];
    let modules = [];
    this.osharp.getTreeNodes(moduleRoot, modules);
    let checkModuleIds = new List(modules).Where(m => m.checked).Select(m => m.Id).ToArray();
    let body = { roleId: this.winRole.Id, moduleIds: checkModuleIds };
    this.http.post("api/admin/role/setpermission", body).subscribe(res => {
      this.osharp.ajaxResult(res, () => {
        this.grid.dataSource.read();
        this.window.close();
      });
    });
  }
  //#endregion

  //#region Tree

  onModuleTreeInit(tree) {
    this.moduleTree = tree;
  }

  //#endregion

}
