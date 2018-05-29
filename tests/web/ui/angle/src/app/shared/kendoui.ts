import { NgZone, ElementRef } from "@angular/core";
import { osharp } from "./osharp";
import { List } from "linqts";
import { AjaxResult } from "./osharp/osharp.model";
declare var $: any;

export namespace kendoui {
  export abstract class GridComponentBase {

    protected moduleName: string = null;
    public gridOptions: kendo.ui.GridOptions = null;
    public grid: kendo.ui.Grid = null;

    constructor(protected zone: NgZone, protected element: ElementRef) { }

    protected InitBase() {
      let dataSourceOptions = this.GetDataSourceOptions();
      let dataSource = new kendo.data.DataSource(dataSourceOptions);
      this.gridOptions = this.GetGridOptions(dataSource);
    }

    protected ViewInitBase() {
      this.zone.runOutsideAngular(() => {
        this.GridInit();
        this.ToolbarInit();
      });
    }

    protected GridInit() {
      let $grid = $($(this.element.nativeElement).find("#grid-box"));
      this.grid = new kendo.ui.Grid($grid, this.gridOptions);
      this.ResizeGrid(true);
      window.addEventListener('keydown', e => this.KeyDownEvent(e));
      window.addEventListener('resize', e => this.ResizeGrid(false));
    }

    protected ToolbarInit() {
      let $toolbar = $(this.grid.element).find(".k-grid-toolbar");
      if (!$toolbar) {
        return;
      }
      $($toolbar).on("click", ".toolbar-right .fullscreen", e => this.toggleGridFullScreen(e));
    }

    protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
      var options: kendo.ui.GridOptions = {
        dataSource: dataSource,
        toolbar: [{ name: 'create' }, { name: 'save' }, { name: 'cancel' }, {
          name: 'toolbarRight', template: `
        <ul class="toolbar-right">
          <li><a class="fullscreen"><em class="fa fa-expand"></em></a></li>
        </ul>
        `}],
        columns: this.GetGridColumns(),
        navigatable: true,
        filterable: true,
        resizable: true,
        scrollable: true,
        selectable: false,
        reorderable: true,
        columnMenu: false,
        sortable: { allowUnsort: true, showIndexes: true, mode: 'multiple' },
        pageable: { refresh: true, pageSizes: [10, 15, 20, 50, 'all'], buttonCount: 5 },
        editable: { mode: "incell", confirmation: true },
        saveChanges: e => {
          if (!confirm('是否提交对表格的更改？')) {
            e.preventDefault();
          }
        }
      };
      return options;
    }

    protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
      var options: kendo.data.DataSourceOptions = {
        transport: {
          read: { url: "/api/admin/" + this.moduleName + "/read", type: 'post' },
          create: { url: "/api/admin/" + this.moduleName + "/create", type: 'post' },
          update: { url: "/api/admin/" + this.moduleName + "/update", type: 'post' },
          destroy: { url: "/api/admin/" + this.moduleName + "/delete", type: 'post' },
          parameterMap: (opts, operation) => {
            if (operation == 'read') {
              return osharp.kendoui.Grid.readParameterMap(opts, this.FieldReplace);
            }
            if (operation == 'create' || operation == 'update') {
              return { dtos: opts.models };
            }
            if (operation == 'destroy' && opts.models.length) {
              var ids = new List(opts.models).Select(m => m['Id']).ToArray();
              return { ids: ids };
            }
            return {};
          }
        },
        group: [],
        schema: {
          model: this.GetModel(),
          data: d => d.Rows,
          total: d => d.Total
        },
        aggregate: [],
        batch: true,
        pageSize: 24,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        requestStart: e => kendoui.Tools.OnRequestStart(e),
        requestEnd: e => {
          if (e.type == "read" && !e.response.Type) {
            return;
          }
          osharp.Tools.ajaxResult(e.response, () => this.grid.options.dataSource.read(), null);
        },
        change: function () { },
        error: e => {
          if (e.status != "error") {
            return;
          }
          osharp.Tools.ajaxError(e.xhr);
        }
      };

      return options;
    }

    protected FieldReplace(field: string): string {
      return field;
    }

    /**重写以获取Grid的模型设置Model */
    protected abstract GetModel(): any;
    /**重写以获取Grid的列设置Columns */
    protected abstract GetGridColumns(): kendo.ui.GridColumn[];

    /**重置Grid高度 */
    protected ResizeGrid(init: boolean) {
      var winWidth = window.innerWidth, winHeight = window.innerHeight, diffHeight = winWidth >= 1114 ? 80 : winWidth >= 768 ? 64 : 145;
      var $content = $("#grid-box .k-grid-content");
      var otherHeight = $("#grid-box").height() - $content.height();
      $content.height(winHeight - diffHeight - otherHeight - (init ? 0 : 0));
    }

    private KeyDownEvent(e) {
      if (!this.grid) {
        return;
      }
      var key = e.keyCode;
      if (key === 83 && e.altKey) {
        this.grid.saveChanges();
      } else if (key === 65 && e.altKey) {
        this.grid.dataSource.read();
      }
    }

    private toggleGridFullScreen(e) {
      var $em = $(e.currentTarget).find(".fa");
      if ($em.hasClass("fa-expand")) {
        $em.removeClass("fa-expand").addClass("fa-compress");
        osharp.Tools.fullscreen(this.grid.element[0]);
      } else {
        $em.removeClass("fa-compress").addClass("fa-expand");
        osharp.Tools.exitFullscreen();
      }
    }
  }

  export abstract class TreeListComponentBase {

    protected moduleName: string = null;
    protected treeListOptions: kendo.ui.TreeListOptions = null;
    protected treeList: kendo.ui.TreeList = null;

    constructor(protected zone: NgZone, protected element: ElementRef) { }

    protected InitBase() {
      var dataSourceOptions = this.GetDataSourceOptions();
      var dataSource = new kendo.data.TreeListDataSource(dataSourceOptions);
      this.treeListOptions = this.GetTreeListOptions(dataSource);
    }

    protected ViewInitBase() {
      this.zone.runOutsideAngular(() => {
        let $tree = $($(this.element.nativeElement).find("#tree-list-box"));
        this.treeList = new kendo.ui.TreeList($tree, this.treeListOptions);
        //this.ResizeGrid(true);
        window.addEventListener('keydown', e => this.KeyDownEvent(e));
        // window.addEventListener('resize', e => this.ResizeGrid(false));
      });
    }

    protected GetTreeListOptions(dataSource: kendo.data.TreeListDataSource): kendo.ui.TreeListOptions {
      var options: kendo.ui.TreeListOptions = {
        dataSource: dataSource,
        columns: this.GetTreeListColumns(),
        selectable: true,
        resizable: true,
        editable: { move: true }
      };
      return options;
    }
    protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
      var options: kendo.data.DataSourceOptions = {
        transport: {
          read: { url: "/api/admin/" + this.moduleName + "/read", type: 'post' },
          create: { url: "/api/admin/" + this.moduleName + "/create", type: 'post' },
          update: { url: "/api/admin/" + this.moduleName + "/update", type: 'post' },
          destroy: { url: "/api/admin/" + this.moduleName + "/delete", type: 'post' },
        },
        schema: {
          model: this.GetModel()
        },
        requestStart: e => kendoui.Tools.OnRequestStart(e),
        requestEnd: e => osharp.Tools.ajaxResult(e.response, () => this.treeList.dataSource.read())
      };

      return options;
    }
    protected FieldReplace(field: string): string {
      return field;
    }

    /**重写以获取Model */
    protected abstract GetModel(): any;
    protected abstract GetTreeListColumns(): kendo.ui.TreeListColumn[];

    /**重置Grid高度 */
    protected ResizeGrid(init: boolean) {
      var winWidth = window.innerWidth, winHeight = window.innerHeight, diffHeight = winWidth >= 1114 ? 80 : winWidth >= 768 ? 64 : 145;
      var $content = $("#tree-list-box .k-grid-content");
      var otherHeight = $("#tree-list-box").height() - $content.height();
      $content.height(winHeight - diffHeight - otherHeight - (init ? 0 : 0));
    }

    private KeyDownEvent(e) {
      if (!this.treeList) {
        return;
      }
      var key = e.keyCode;
      if (key === 83 && e.altKey) {
        this.treeList.saveRow();
      } else if (key === 65 && e.altKey) {
        this.treeList.dataSource.read();
      }
    }
  }

  export class Controls {
    static Boolean(value: boolean) {
      let html = value ? '<input type="checkbox" checked="checked"/>' : '<input type="checkbox"/>';
      return '<div class="checkbox c-checkbox"><label>' + html + '<span class="fa fa-check"></span></label></div>';
    }

    static BooleanEditor(container, options) {
      var guid = kendo.guid();
      $('<input class="k-checkbox" type="checkbox" id="' + guid + '" name="' + options.field + '" data-type="boolean" data-bind="checked:' + options.field + '">').appendTo(container);
      $('<label class="k-checkbox-label" for="' + guid + '"></label>').appendTo(container);
    }

    static NumberEditor(container, options, decimals, step?) {
      var input = $('<input/>');
      input.attr('name', options.field);
      input.appendTo(container);
      new kendo.ui.NumericTextBox(input, {
        format: '{0:' + decimals + '}',
        step: step || 0.01
      });
    }

    static DropDownList(element, dataSource, textField = 'text', valueField = 'id') {
      element.kendoDropDownList({
        autoBind: true,
        dataTextField: textField || "text",
        dataValueField: valueField || "id",
        dataSource: dataSource
      });
    }

    static DropDownListEditor(container, options, dataSource, textField = 'text', valueField = 'id') {
      var input = $('<input/>');
      input.attr('name', options.field);
      input.appendTo(container);
      new kendo.ui.DropDownList(input, {
        autoBind: true,
        dataTextField: textField,
        dataValueField: valueField,
        dataSource: dataSource
      });
    }
  }

  export class Tools {

    /**获取TreeView树数据源 */
    static CreateHierarchicalDataSource(url: string): kendo.data.HierarchicalDataSource {
      return new kendo.data.HierarchicalDataSource({
        transport: { read: { url: url } },
        schema: { model: { children: "Items", hasChildren: "HasChildren" } },
        requestStart: e => kendoui.Tools.OnRequestStart(e),
        requestEnd: e => {
          if (e.type == "read" && e.response.Type) {
            osharp.Tools.ajaxResult(e.response);
            return;
          }
          e.response = kendoui.Tools.TreeDataInit(e.response);
        }
      });
    }

    /**初始化树数据 */
    static TreeDataInit(nodes: Array<any>): any {
      if (!nodes.length) {
        return nodes;
      }
      for (let i = 0; i < nodes.length; i++) {
        const node = nodes[i];
        node.checked = node.IsChecked;
        node.expanded = node.HasChildren;
        nodes[i] = node;
        if (node.Items) {
          node.Items = kendoui.Tools.TreeDataInit(node.Items);
        }
      }
      return nodes;
    }

    static queryOptions(source, options) {
      var sopts = source.options;
      var opts = {
        filter: options.filter || null,
        aggregate: sopts.aggregate || [],
        group: sopts.group || [],
        page: sopts.page || 1,
        pageSize: sopts.pageSize || 20,
        sort: options.sort || sopts.sort,
        take: sopts.pageSize,
        skip: sopts.pageSize * (sopts.page - 1)
      };
      return opts;
    }

    static OnTreeNodeSelect(e) {
      var item = e.sender.dataItem(e.node);
      item.set("checked", !item.checked);
      e.preventDefault();
    }

    static OnRequestStart(e) {
      osharp.Kendo.setAuthToken(e.sender);
    }
  }
}
