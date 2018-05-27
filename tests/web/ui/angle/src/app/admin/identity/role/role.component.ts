import { Component, OnInit, OnDestroy, NgZone, ElementRef, AfterViewInit, } from '@angular/core';

import { kendoui } from '../../../shared/kendoui';
import { osharp } from '../../../shared/osharp';
import { ToasterService } from 'angular2-toaster/src/toaster.service';
import { HttpClient } from '@angular/common/http';
import { List } from 'linqts';

@Component({
  selector: 'identity-role',
  templateUrl: `./role.component.html`
})
export class RoleComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

  windowOptions: kendo.ui.WindowOptions;
  window: kendo.ui.Window;
  moduleTreeOptions: kendo.ui.TreeViewOptions;
  moduleTree: kendo.ui.TreeView;

  constructor(protected zone: NgZone, protected el: ElementRef, private http: HttpClient) {
    super(zone, el);
    this.moduleName = "role";
    this.windowOptions = {
      visible: false, width: 500, height: 620, modal: true, title: "角色权限设置", actions: ["Pin", "Minimize", "Maximize", "Close"],
      resize: e => this.onWinResize(e)
    };
    this.moduleTreeOptions = { autoBind: true, checkboxes: { checkChildren: true }, dataTextField: "Name", select: e => kendoui.Tools.OnTreeNodeSelect(e) };
  }

  ngOnInit() {
    super.InitBase();
  }

  ngAfterViewInit() {
    super.ViewInitBase();
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
        filterable: osharp.Data.stringFilterable
      }, {
        field: "Remark", title: "备注", width: 250,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "IsAdmin", title: "管理", width: 95,
        template: d => kendoui.Controls.Boolean(d.IsAdmin),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "IsDefault", title: "默认", width: 95,
        template: d => kendoui.Controls.Boolean(d.IsDefault),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "IsLocked", title: "锁定", width: 95,
        template: d => kendoui.Controls.Boolean(d.IsLocked),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      },
      { field: "CreatedTime", title: "注册时间", width: 115, format: "{0:yy-MM-dd HH:mm}" }
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
    var tr = $(e.target).closest("tr");
    this.winRole = this.grid.dataItem(tr);
    this.window.title("角色权限设置-" + this.winRole.Name).open().center().resize();
    //设置树数据
    this.moduleTree.setDataSource(kendoui.Tools.CreateHierarchicalDataSource("/api/admin/module/ReadRoleModules?roleId=" + this.winRole.Id));
  }
  private onWinResize(e) {
    $(".win-content .k-tabstrip .k-content").height(e.height - 140);
  }
  onWinSubmit(win) {
    var moduleRoot = this.moduleTree.dataSource.data()[0];
    var modules = [];
    osharp.Tools.getTreeNodes(moduleRoot, modules);
    var checkModuleIds = new List(modules).Where(m => m.checked).Select(m => m.Id).ToArray();
    var body = { roleId: this.winRole.Id, moduleIds: checkModuleIds };
    this.http.post("/api/admin/role/setpermission", body).subscribe(res => {
      osharp.Tools.ajaxResult(res, () => {
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
