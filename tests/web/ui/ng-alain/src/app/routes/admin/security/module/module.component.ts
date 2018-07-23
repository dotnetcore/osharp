import { Component, AfterViewInit, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { List } from 'linqts';
import { TreeListComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-security-module',
  templateUrl: './module.component.html'
})
export class ModuleComponent extends TreeListComponentBase implements AfterViewInit {

  splitterOptions: kendo.ui.SplitterOptions = null;
  selectedModuleId = 0;
  windowOptions: kendo.ui.WindowOptions;
  window: kendo.ui.Window;
  functionTreeOptions: kendo.ui.TreeViewOptions;
  functionTree: kendo.ui.TreeView;

  constructor(injector: Injector, private http: HttpClient) {
    super(injector);
    this.moduleName = "module";
    this.splitterOptions = {
      panes: [{ size: "60%" }, { collapsible: true, collapsed: false }]
    };
    this.windowOptions = {
      visible: false, width: 500, height: 620, modal: true, title: "模块功能设置", actions: ["Pin", "Minimize", "Maximize", "Close"],
      resize: e => this.onWindowResize(e)
    };
    this.functionTreeOptions = { autoBind: true, checkboxes: { checkChildren: true }, dataTextField: "Name", select: e => this.kendoui.OnTreeNodeSelect(e) };
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
    return new AuthConfig("Root.Admin.Security.Module", ["Read", "ReadFunctions"]);
  }

  //#region Grid
  protected GetModel() {
    return {
      id: "Id",
      parentId: "ParentId",
      fields: {
        Id: { type: "number", nullable: false, editable: false },
        ParentId: { type: "number", nullable: true },
        Name: { type: "string" },
        Code: { type: 'string' },
        OrderCode: { type: "number" },
        Remark: { type: "string" }
      },
      expanded: true
    };
  }
  protected GetTreeListColumns(): kendo.ui.TreeListColumn[] {
    return [
      {
        field: "Name", title: "名称", width: 200,
        template: d => "<span>" + d.Id + ". " + d.Name + "</span>"
      },
      { field: "Remark", title: "备注", width: 200 },
      { field: "Code", title: "代码", width: 130 },
      {
        field: "OrderCode", title: "排序", width: 70,
        editor: (container, options) => this.kendoui.NumberEditor(container, options, 3)
      }, {
        title: "操作",
        command: [
          // { name: "setFuncs", imageClass: "k-i-categorize", text: " ", click: e => this.windowOpen(e) },
          // { name: "createChild", text: " " },
          // { name: "edit", text: " " },
          // { name: "destroy", imageClass: "k-i-delete", text: " " }
        ],
        width: 180
      }
    ];
  }
  protected GetTreeListOptions(dataSource: kendo.data.TreeListDataSource): kendo.ui.TreeListOptions {
    let options = super.GetTreeListOptions(dataSource);
    options.toolbar = [{ name: "refresh", text: "刷新", imageClass: "k-icon k-i-refresh", click: e => this.treeList.dataSource.read() }];
    options.change = e => {
      let row = this.treeList.select();
      if (row) {
        let data: any = this.treeList.dataItem(row);
        this.selectedModuleId = data.Id;
      }
    };
    options.remove = e => {
      let model: any = e.model;
      if (!model.ParentId) {
        this.osharp.error('“' + model.Name + '”是根结点，禁止删除');
        e.preventDefault();
        return;
      }
      if (model.hasChildren) {
        this.osharp.error('“' + model.Name + '”包含子节点，不能删除');
        e.preventDefault();
        return;
      }
      if (!confirm('是否删除模块“' + model.Name + '”？')) {
        e.preventDefault();
        return;
      }
    };
    return options;
  }
  protected FilterTreeListAuth(options: kendo.ui.TreeListOptions) {
    //命令列
    let cmdColumn = options.columns && options.columns.find(m => m.command != null);
    let cmds = cmdColumn && cmdColumn.command as kendo.ui.TreeListColumnCommandItem[];
    if (cmds) {
      if (!this.auth.SetFunctions) {
        this.osharp.remove(cmds, m => m.name == "setFuncs");
      }
    }
    options = super.FilterTreeListAuth(options);
    return options;
  }
  onModuleSelectedChange(functionGrid: kendo.ui.Grid) {
    let options = {
      filter: {
        logic: "and",
        filters: [
          { field: "TreePathString", operator: "contains", value: "$" + this.selectedModuleId + "$" }
        ]
      }
    };
    options = this.kendoui.queryOptions(functionGrid.dataSource, options);
    console.log(options);
    functionGrid.dataSource.query(options);
  }

  //#endregion

  //#region Window

  private winModule;
  onWindowInit(win) {
    this.window = win;
  }
  windowOpen(e) {
    e.preventDefault();
    let tr = $(e.target).closest("tr");
    this.winModule = this.treeList.dataItem(tr);
    this.window.title("模块功能设置-" + this.winModule.Name).open().center().resize();
    this.treeList.select(tr);
    // 设置树数据
    this.functionTree.setDataSource(this.kendoui.CreateHierarchicalDataSource("api/admin/function/ReadTreeNode?moduleId=" + this.winModule.Id));
  }
  private onWindowResize(e) {
    $(".win-content .k-tabstrip .k-content").height(e.height - 140);
  }
  onWindowSubmit(win) {
    let root = this.functionTree.dataSource.data()[0];
    let functions = [];
    this.osharp.getTreeNodes(root, functions);
    let checkFuncIds = new List(functions).Where(m => m.checked).Select(m => m.Id).ToArray();
    let body = { moduleId: this.winModule.Id, functionIds: checkFuncIds };
    this.http.post("api/admin/module/setfunctions", body).subscribe(res => {
      this.osharp.ajaxResult(res, () => {
        $("#btn-refresh-function").click();
        this.window.close();
      });
    });
  }
  //#endregion

  //#region TreeView
  onFunctionTreeInit(tree) {
    this.functionTree = tree;
  }
  //#endregion
}
