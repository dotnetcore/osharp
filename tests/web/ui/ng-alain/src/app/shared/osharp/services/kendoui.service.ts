import { Injectable, NgZone, ElementRef, Injector } from '@angular/core';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { Group, Rule } from '@shared/osharp/osharp.model';
import { isFunction } from 'util';
import { List } from "linqts";

@Injectable()
export class KendouiService {

  osharp: OsharpService;

  constructor(private injector: Injector) {
    this.osharp = injector.get(OsharpService);
  }

  // #region 工具操作

  /** 获取osharp查询条件组 */
  getFilterGroup(filter, funcFieldReplace): Group {
    if (!funcFieldReplace) {
      funcFieldReplace = field => field;
    }
    if (!filter || !filter.filters || !filter.filters.length) {
      return null;
    }
    const group = new Group();
    filter.filters.forEach(item => {
      if (item.filters && item.filters.length) {
        group.groups.push(this.getFilterGroup(item, funcFieldReplace));
      } else {
        group.rules.push(this.getFilterRule(item, funcFieldReplace));
      }
    });
    group.operate = filter.logic;
    return group;
  }
  /** 获取osharp查询条件 */
  getFilterRule(filter, funcFieldReplace = null): Rule {
    if (!funcFieldReplace || !isFunction(funcFieldReplace)) {
      throw new Error("funcFieldReplace muse be function");
    }
    const field = funcFieldReplace(filter.field);
    const operate = this.renderRuleOperate(filter.operator);
    const rule = new Rule(field, filter.value, operate);
    return rule;
  }
  /** 转换查询操作 */
  renderRuleOperate(operate) {
    if (operate === "eq") return "equal";
    if (operate === "neq") return "notequal";
    if (operate === "gt") return "greater";
    if (operate === "gte") return "greaterorequal";
    if (operate === "lt") return "less";
    if (operate === "lte") return "lessorequal";
    if (operate === "doesnotcontain") return "notcontains";
    return operate;
  }
  /** 处理kendoui到osharp框架的查询参数 */
  readParameterMap(options, funcFieldReplace) {
    if (!funcFieldReplace) {
      funcFieldReplace = field => field;
    }
    const paramter = {
      pageIndex: options.page,
      pageSize: options.pageSize || 100000,
      sortField: null,
      sortOrder: null,
      filter_group: null
    };
    if (options.sort && options.sort.length) {
      const fields = [], orders = [];
      options.sort.forEach(item => {
        fields.push(funcFieldReplace(item.field));
        orders.push(item.dir);
      });
      paramter.sortField = this.osharp.expandAndToString(fields);
      paramter.sortOrder = this.osharp.expandAndToString(orders);
    }
    if (options.filter && options.filter.filters.length) {
      const group = this.getFilterGroup(options.filter, funcFieldReplace);
      paramter.filter_group = JSON.stringify(group);
    }
    return paramter;
  }


  /**给每个请求头设置 JWT-Token */
  setAuthToken(dataSource: kendo.data.DataSource) {
    const authToken = localStorage.getItem('id_token');
    if (!authToken) {
      return;
    }
    if (dataSource && dataSource.options && dataSource.options.transport) {
      const trans = dataSource.options.transport;
      if (trans.read) {
        (<any>trans.read).beforeSend = this.setAuthHeaderToken;
      }
      if (trans.create) {
        (<any>trans.create).beforeSend = this.setAuthHeaderToken;
      }
      if (trans.update) {
        (<any>trans.update).beforeSend = this.setAuthHeaderToken;
      }
      if (trans.destroy) {
        (<any>trans.destroy).beforeSend = this.setAuthHeaderToken;
      }
    }
  }
  private setAuthHeaderToken(xhr, opts) {
    const authToken = localStorage.getItem('id_token');
    xhr.setRequestHeader("Authorization", `Bearer ${authToken}`);
  }


  /**获取TreeView树数据源 */
  CreateHierarchicalDataSource(url: string): kendo.data.HierarchicalDataSource {
    return new kendo.data.HierarchicalDataSource({
      transport: { read: { url: url } },
      schema: { model: { children: "Items", hasChildren: "HasChildren" } },
      requestStart: e => this.OnRequestStart(e),
      requestEnd: e => {
        if (e.type == "read" && e.response.Type) {
          this.osharp.ajaxResult(e.response);
          return;
        }
        e.response = this.TreeDataInit(e.response);
      }
    });
  }

  /**初始化树数据 */
  TreeDataInit(nodes: Array<any>): any {
    if (!nodes.length) {
      return nodes;
    }
    for (let i = 0; i < nodes.length; i++) {
      const node = nodes[i];
      node.checked = node.IsChecked;
      node.expanded = node.HasChildren;
      nodes[i] = node;
      if (node.Items) {
        node.Items = this.TreeDataInit(node.Items);
      }
    }
    return nodes;
  }
  /**树节点被造反时选中复选框 */
  OnTreeNodeSelect(e) {
    const item = e.sender.dataItem(e.node);
    item.set("checked", !item.checked);
    e.preventDefault();
  }
  /**kendo请求开始时设置JWT-Token */
  OnRequestStart(e) {
    this.setAuthToken(e.sender);
  }

  queryOptions(source, options) {
    let sopts = source.options;
    let opts = {
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

  // #endregion

  // #region kendoui控件

  Boolean(value: boolean) {
    const html = value ? '<input type="checkbox" checked="checked"/>' : '<input type="checkbox"/>';
    return '<div class="checkbox c-checkbox"><label>' + html + '<span class="fa fa-check"></span></label></div>';
  }

  BooleanEditor(container, options) {
    const guid = kendo.guid();
    $('<input class="k-checkbox" type="checkbox" id="' + guid + '" name="' + options.field + '" data-type="boolean" data-bind="checked:' + options.field + '">').appendTo(container);
    $('<label class="k-checkbox-label" for="' + guid + '"></label>').appendTo(container);
  }

  NumberEditor(container, options, decimals, step?) {
    const input = $('<input/>');
    input.attr('name', options.field);
    input.appendTo(container);
    return new kendo.ui.NumericTextBox(input, {
      format: '{0:' + decimals + '}',
      step: step || 0.01
    });
  }

  DropDownList(element, dataSource, textField = 'text', valueField = 'id') {
    return new kendo.ui.DropDownList(element, {
      autoBind: true,
      dataTextField: textField || "text",
      dataValueField: valueField || "id",
      dataSource: dataSource
    });
  }

  DropDownListEditor(container, options, dataSource, textField = 'text', valueField = 'id') {
    const input = $('<input/>');
    input.attr('name', options.field);
    input.appendTo(container);
    return new kendo.ui.DropDownList(input, {
      autoBind: true,
      dataTextField: textField,
      dataValueField: valueField,
      dataSource: dataSource
    });
  }

  // #endregion
}

export abstract class GridComponentBase {

  protected moduleName: string = null;
  public gridOptions: kendo.ui.GridOptions = null;
  public grid: kendo.ui.Grid = null;

  protected zone: NgZone;
  protected element: ElementRef;
  protected osharp: OsharpService;
  protected kendoui: KendouiService;

  constructor(private injector: Injector) {
    this.zone = injector.get(NgZone);
    this.element = injector.get(ElementRef);
    this.osharp = injector.get(OsharpService);
    this.kendoui = injector.get(KendouiService);
  }

  protected InitBase() {
    const dataSourceOptions = this.GetDataSourceOptions();
    const dataSource = new kendo.data.DataSource(dataSourceOptions);
    this.gridOptions = this.GetGridOptions(dataSource);
  }

  protected ViewInitBase() {
    this.zone.runOutsideAngular(() => {
      this.GridInit();
      this.ToolbarInit();
    });
  }

  protected GridInit() {
    const $grid = $($(this.element.nativeElement).find("#grid-box"));
    this.grid = new kendo.ui.Grid($grid, this.gridOptions);
    this.ResizeGrid(true);
    window.addEventListener('keydown', e => this.KeyDownEvent(e));
    window.addEventListener('resize', e => this.ResizeGrid(false));
  }

  protected ToolbarInit() {
    const $toolbar = $(this.grid.element).find(".k-grid-toolbar");
    if (!$toolbar) {
      return;
    }
    // $($toolbar).on("click", ".toolbar-right .fullscreen", e => this.toggleGridFullScreen(e));
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    const options: kendo.ui.GridOptions = {
      dataSource: dataSource,
      toolbar: [{ name: 'create' }, { name: 'save' }, { name: 'cancel' }],
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
    const options: kendo.data.DataSourceOptions = {
      transport: {
        read: { url: "/api/admin/" + this.moduleName + "/read", type: 'post' },
        create: { url: "/api/admin/" + this.moduleName + "/create", type: 'post' },
        update: { url: "/api/admin/" + this.moduleName + "/update", type: 'post' },
        destroy: { url: "/api/admin/" + this.moduleName + "/delete", type: 'post' },
        parameterMap: (opts, operation) => {
          if (operation == 'read') {
            return this.kendoui.readParameterMap(opts, this.FieldReplace);
          }
          if (operation == 'create' || operation == 'update') {
            return { dtos: opts.models };
          }
          if (operation == 'destroy' && opts.models.length) {
            const ids = new List(opts.models).Select(m => m['Id']).ToArray();
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
      requestStart: e => this.kendoui.OnRequestStart(e),
      requestEnd: e => {
        if (e.type == "read" && !e.response.Type) {
          return;
        }
        this.osharp.ajaxResult(e.response, () => this.grid.options.dataSource.read(), null);
      },
      change: function () { },
      error: e => {
        if (e.status != "error") {
          return;
        }
        this.osharp.ajaxError(e.xhr);
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
    const winWidth = window.innerWidth, winHeight = window.innerHeight, diffHeight = winWidth >= 1114 ? 80 : winWidth >= 768 ? 64 : 145;
    const $content = $("#grid-box .k-grid-content");
    const otherHeight = $("#grid-box").height() - $content.height();
    $content.height(winHeight - diffHeight - otherHeight - (init ? 0 : 0));
  }

  private KeyDownEvent(e) {
    if (!this.grid) {
      return;
    }
    const key = e.keyCode;
    if (key === 83 && e.altKey) {
      this.grid.saveChanges();
    } else if (key === 65 && e.altKey) {
      this.grid.dataSource.read();
    }
  }
}


export abstract class TreeListComponentBase {

  protected moduleName: string = null;
  protected treeListOptions: kendo.ui.TreeListOptions = null;
  protected treeList: kendo.ui.TreeList = null;

  protected zone: NgZone;
  protected element: ElementRef;
  protected osharp: OsharpService;
  protected kendoui: KendouiService;

  constructor(private injector: Injector) {
    this.zone = injector.get(NgZone);
    this.element = injector.get(ElementRef);
    this.osharp = injector.get(OsharpService);
    this.kendoui = injector.get(KendouiService);
  }

  protected InitBase() {
    const dataSourceOptions = this.GetDataSourceOptions();
    const dataSource = new kendo.data.TreeListDataSource(dataSourceOptions);
    this.treeListOptions = this.GetTreeListOptions(dataSource);
  }

  protected ViewInitBase() {
    this.zone.runOutsideAngular(() => {
      const $tree = $($(this.element.nativeElement).find("#tree-list-box"));
      this.treeList = new kendo.ui.TreeList($tree, this.treeListOptions);
      // this.ResizeGrid(true);
      window.addEventListener('keydown', e => this.KeyDownEvent(e));
      // window.addEventListener('resize', e => this.ResizeGrid(false));
    });
  }

  protected GetTreeListOptions(dataSource: kendo.data.TreeListDataSource): kendo.ui.TreeListOptions {
    const options: kendo.ui.TreeListOptions = {
      dataSource: dataSource,
      columns: this.GetTreeListColumns(),
      selectable: true,
      resizable: true,
      editable: { move: true }
    };
    return options;
  }
  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    const options: kendo.data.DataSourceOptions = {
      transport: {
        read: { url: "/api/admin/" + this.moduleName + "/read", type: 'post' },
        create: { url: "/api/admin/" + this.moduleName + "/create", type: 'post' },
        update: { url: "/api/admin/" + this.moduleName + "/update", type: 'post' },
        destroy: { url: "/api/admin/" + this.moduleName + "/delete", type: 'post' },
      },
      schema: {
        model: this.GetModel()
      },
      requestStart: e => this.kendoui.OnRequestStart(e),
      requestEnd: e => this.osharp.ajaxResult(e.response, () => this.treeList.dataSource.read())
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
    const winWidth = window.innerWidth, winHeight = window.innerHeight, diffHeight = winWidth >= 1114 ? 80 : winWidth >= 768 ? 64 : 145;
    const $content = $("#tree-list-box .k-grid-content");
    const otherHeight = $("#tree-list-box").height() - $content.height();
    $content.height(winHeight - diffHeight - otherHeight - (init ? 0 : 0));
  }

  private KeyDownEvent(e) {
    if (!this.treeList) {
      return;
    }
    const key = e.keyCode;
    if (key === 83 && e.altKey) {
      this.treeList.saveRow();
    } else if (key === 65 && e.altKey) {
      this.treeList.dataSource.read();
    }
  }
}
