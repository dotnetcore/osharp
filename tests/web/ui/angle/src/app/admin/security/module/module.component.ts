import { Component, OnInit, ElementRef, AfterViewInit, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';


import { kendoui } from '../../../shared/kendoui';
import { osharp } from "../../../shared/osharp";
import { List } from 'linqts';

@Component({
  selector: 'security-module',
  templateUrl: './module.component.html'
})
export class ModuleComponent extends kendoui.TreeListComponentBase implements OnInit, AfterViewInit {

  splitterOptions: kendo.ui.SplitterOptions = null;
  selectedModuleId: number = 0;
  windowOptions: kendo.ui.WindowOptions;
  window: kendo.ui.Window;
  functionTreeOptions: kendo.ui.TreeViewOptions;
  functionTree: kendo.ui.TreeView;

  constructor(protected zone: NgZone, protected element: ElementRef, private http: HttpClient) {
    super(zone, element);
    this.moduleName = "module";
    this.splitterOptions = {
      panes: [{ size: "60%" }, { collapsible: true, collapsed: false }]
    };
    this.windowOptions = {
      visible: false, width: 500, height: 620, modal: true, title: "模块功能设置", actions: ["Pin", "Minimize", "Maximize", "Close"],
      resize: e => this.onWindowResize(e)
    };
    this.functionTreeOptions = { autoBind: true, checkboxes: { checkChildren: true }, dataTextField: "Name", select: e => kendoui.Tools.OnTreeNodeSelect(e) };
  }

  ngOnInit() {
    super.InitBase();
  }
  ngAfterViewInit(): void {
    super.ViewInitBase();
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
      {
        field: "OrderCode", title: "排序", width: 70,
        editor: (container, options) => kendoui.Controls.NumberEditor(container, options, 3)
      }, {
        title: "操作",
        command: [
          { name: "setFuncs", imageClass: "k-i-categorize", text: " ", click: e => this.windowOpen(e) },
          { name: "createChild", text: " " },
          { name: "edit", text: " " },
          { name: "destroy", imageClass: "k-i-delete", text: " " }
        ],
        width: 180
      }
    ];
  }
  protected GetTreeListOptions(dataSource: kendo.data.TreeListDataSource): kendo.ui.TreeListOptions {
    var options = super.GetTreeListOptions(dataSource);
    options.toolbar = [{ name: "refresh", text: "刷新", imageClass: "k-icon k-i-refresh", click: e => this.treeList.dataSource.read() }];
    options.change = e => {
      var row = this.treeList.select();
      if (row) {
        var data: any = this.treeList.dataItem(row);
        this.selectedModuleId = data.Id;
      }
    };
    options.remove = e => {
      var model: any = e.model;
      if (!model.ParentId) {
        osharp.Tip.error('“' + model.Name + '”是根结点，禁止删除');
        e.preventDefault();
        return;
      }
      if (model.hasChildren) {
        osharp.Tip.error('“' + model.Name + '”包含子节点，不能删除');
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

  onModuleSelectedChange(functionGrid: kendo.ui.Grid) {
    var options = {
      filter: {
        logic: "and",
        filters: [
          { field: "TreePathString", operator: "contains", value: "$" + this.selectedModuleId + "$" }
        ]
      }
    };
    options = kendoui.Tools.queryOptions(functionGrid.dataSource, options);
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
    var tr = $(e.target).closest("tr");
    this.winModule = this.treeList.dataItem(tr);
    this.window.title("模块功能设置-" + this.winModule.Name).open().center().resize();
    this.treeList.select(tr);
    //设置树数据
    this.functionTree.setDataSource(kendoui.Tools.CreateHierarchicalDataSource("/api/admin/function/ReadTreeNode?moduleId=" + this.winModule.Id));
  }
  private onWindowResize(e) {
    $(".win-content .k-tabstrip .k-content").height(e.height - 140);
  }
  onWindowSubmit(win) {
    var root = this.functionTree.dataSource.data()[0];
    var functions = [];
    osharp.Tools.getTreeNodes(root, functions);
    var checkFuncIds = new List(functions).Where(m => m.checked).Select(m => m.Id).ToArray();
    var body = { moduleId: this.winModule.Id, functionIds: checkFuncIds };
    this.http.post("/api/admin/module/setfunctions", body).subscribe(res => {
      osharp.Tools.ajaxResult(res, () => {
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
