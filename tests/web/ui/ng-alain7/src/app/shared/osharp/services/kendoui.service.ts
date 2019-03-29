import { Injectable, NgZone, ElementRef, Injector, Inject } from '@angular/core';
import { OsharpService, ComponentBase } from '@shared/osharp/services/osharp.service';
import { FilterGroup, FilterRule, FilterOperate, PageRequest, SortCondition, ListSortDirection } from '@shared/osharp/osharp.model';
import { isFunction } from 'util';
import { List } from "linqts";
import { JWTTokenModel, ITokenService, DA_SERVICE_TOKEN } from '@delon/auth';
import { NzModalService } from 'ng-zorro-antd';


@Injectable()
export class KendouiService {

  constructor(
    private modal: NzModalService,
    private osharp: OsharpService,
    @Inject(DA_SERVICE_TOKEN) private tokenSrv: ITokenService
  ) { }

  // #region 工具操作

  /**
   * 获取osharp查询条件组
   * @param filter kendo发出的filter
   * @param funcFieldReplace 字段替换函数，用于处理关联实体的字段
   */
  getFilterGroup(filter, funcFieldReplace): FilterGroup {
    if (!funcFieldReplace) {
      funcFieldReplace = field => field;
    }
    if (!filter || !filter.filters) {
      return null;
    }
    if (filter.filters.length == 0) {
      return new FilterGroup();
    }
    const group = new FilterGroup();
    filter.filters.forEach(item => {
      if (item.filters && item.filters.length) {
        group.Groups.push(this.getFilterGroup(item, funcFieldReplace));
      } else {
        group.Rules.push(this.getFilterRule(item, funcFieldReplace));
      }
    });
    group.Operate = this.renderRuleOperate(filter.logic);
    return group;
  }
  /**
   * 获取osharp查询条件
   * @param filter kendo发出的filter
   * @param funcFieldReplace 字段替换函数，用于处理关联实体的字段
   */
  getFilterRule(filter, funcFieldReplace = null): FilterRule {
    if (!funcFieldReplace || !isFunction(funcFieldReplace)) {
      throw new Error("funcFieldReplace muse be function");
    }
    const field = funcFieldReplace(filter.field);
    const operate: FilterOperate = this.renderRuleOperate(filter.operator);
    const rule = new FilterRule(field, filter.value, operate);
    return rule;
  }
  /**
   * 转换查询操作
   * @param operate kendo的查询对比操作字符串
   */
  renderRuleOperate(operate): FilterOperate {
    let dict: { [key: string]: FilterOperate } = {
      "and": FilterOperate.And,
      "or": FilterOperate.Or,
      "eq": FilterOperate.Equal,
      "neq": FilterOperate.NotEqual,
      "lt": FilterOperate.Less,
      "lte": FilterOperate.LessOrEqual,
      "gt": FilterOperate.Greater,
      "gte": FilterOperate.GreaterOrEqual,
      "startswith": FilterOperate.StartsWith,
      "endswith": FilterOperate.EndsWith,
      "contains": FilterOperate.Contains,
      "doesnotcontain": FilterOperate.NotContains
    };
    for (const key in dict) {
      if (dict.hasOwnProperty(key)) {
        const value = dict[key];
        if (key === operate) {
          return value;
        }
      }
    }
    throw new Error(`后端服务器不支持${operate}的比较操作`);
  }
  /**
   * 处理kendoui到osharp框架的查询参数
   * @param options kendo发送的选项
   * @param funcFieldReplace 字段替换函数，用于处理关联实体的字段
   */
  readParameterMap(options, funcFieldReplace): PageRequest {
    if (!funcFieldReplace) {
      funcFieldReplace = field => field;
    }
    const request = new PageRequest();
    let page = request.PageCondition;
    page.PageIndex = options.page;
    page.PageSize = options.pageSize || 100000;
    if (options.sort && options.sort.length) {
      options.sort.forEach(item => {
        let sort = new SortCondition();
        sort.SortField = funcFieldReplace(item.field);
        sort.ListSortDirection = item.dir == "desc" ? ListSortDirection.Descending : ListSortDirection.Ascending;
        page.SortConditions.push(sort);
      });
    }
    if (options.filter && options.filter.filters.length) {
      request.FilterGroup = this.getFilterGroup(options.filter, funcFieldReplace);
    }
    return request;
  }

  /**给每个请求头设置 JWT-Token */
  setAuthToken(dataSource: kendo.data.DataSource) {

    let token = this.tokenSrv.get(JWTTokenModel);
    if (!token || !token.token || token.isExpired()) {
      return;
    }

    if (dataSource && dataSource.options && dataSource.options.transport) {
      const trans = dataSource.options.transport;
      if (trans.read) {
        (<any>trans.read).beforeSend = (xhr, opts) => this.setAuthHeaderToken(xhr, opts);
      }
      if (trans.create) {
        (<any>trans.create).beforeSend = (xhr, opts) => this.setAuthHeaderToken(xhr, opts);
      }
      if (trans.update) {
        (<any>trans.update).beforeSend = (xhr, opts) => this.setAuthHeaderToken(xhr, opts);
      }
      if (trans.destroy) {
        (<any>trans.destroy).beforeSend = (xhr, opts) => this.setAuthHeaderToken(xhr, opts);
      }
    }
  }
  private setAuthHeaderToken(xhr, opts) {
    let token = this.tokenSrv.get(JWTTokenModel);
    xhr.setRequestHeader("Authorization", `Bearer ${token.token}`);
  }

  /**
   * 获取TreeView树数据源
   * @param url 数据源URL
   */
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

  /**
   * 创建一个Grid的选项
   * @param dataSource 数据源
   * @param columns 数据列配置
   */
  CreateGridOptions(
    dataSource: kendo.data.DataSource,
    columns: kendo.ui.GridColumn[]
  ) {
    const options: kendo.ui.GridOptions = {
      dataSource: dataSource,
      toolbar: [{ name: 'create' }, { name: 'save' }, { name: 'cancel' }],
      columns: columns,
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
        e.preventDefault();
        this.modal.confirm({ nzTitle: '请选择', nzContent: '是否提交对表格的更改？', nzOnOk: () => e.sender.dataSource.sync() });
      }
    };
    return options;
  }

  /**
   * 创建Grid数据源
   * @param moduleName 模块名称
   * @param model 模型信息
   * @param funcFieldReplace 字段替换方法委托
   * @param funcGridRefresh 表格刷新方法委托
   */
  CreateDataSourceOptions(moduleName: string,
    model: any,
    funcFieldReplace?: any,
    funcGridRefresh?: any
  ): kendo.data.DataSourceOptions {
    const options: kendo.data.DataSourceOptions = {
      transport: {
        read: { url: "api/admin/" + moduleName + "/read", type: 'post', dataType: 'json', contentType: 'application/json;charset=utf-8' },
        create: { url: "api/admin/" + moduleName + "/create", type: 'post', dataType: 'json', contentType: 'application/json;charset=utf-8' },
        update: { url: "api/admin/" + moduleName + "/update", type: 'post', dataType: 'json', contentType: 'application/json;charset=utf-8' },
        destroy: { url: "api/admin/" + moduleName + "/delete", type: 'post', dataType: 'json', contentType: 'application/json;charset=utf-8' },
        parameterMap: (opts, operation) => {
          if (operation == 'read') {
            let request = this.readParameterMap(opts, funcFieldReplace || (field => field));
            return JSON.stringify(request);
          }
          if (operation == 'create' || operation == 'update') {
            return JSON.stringify(opts.models);
          }
          if (operation == 'destroy' && opts.models.length) {
            const ids = new List(opts.models).Select(m => m['Id']).ToArray();
            return JSON.stringify(ids);
          }
          return {};
        }
      },
      group: [],
      schema: {
        model: model,
        data: d => d.Rows,
        total: d => d.Total
      },
      aggregate: [],
      batch: true,
      pageSize: 24,
      serverPaging: true,
      serverSorting: true,
      serverFiltering: true,
      requestStart: e => this.OnRequestStart(e),
      requestEnd: e => {
        if (e.type == "read" && !e.response.Type) {
          return;
        }
        this.osharp.ajaxResult(e.response, funcGridRefresh);
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

  /**
   * 创建本地数据源
   * @param model 数据模型
   */
  CreateLocalDataSourceOptions(model: any, data?: object[]): kendo.data.DataSourceOptions {
    let options: kendo.data.DataSourceOptions = {
      data: data || [],
      schema: {
        model: model
      },
      pageSize: 20
    };
    return options;
  }

  /**
   * 初始化树数据
   * @param nodes 原始树数据节点
   */
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
  /** 树节点被造反时选中复选框 */
  OnTreeNodeSelect(e) {
    const item = e.sender.dataItem(e.node);
    item.set("checked", !item.checked);
    e.preventDefault();
  }
  /** kendo请求开始时设置JWT-Token */
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
    // const html = value ? '<input type="checkbox" checked="checked"/>' : '<input type="checkbox"/>';
    // return '<div class="checkbox c-checkbox"><label>' + html + '<span class="fa fa-check"></span></label></div>';
    return value ? "<div class=\"checker\"><span class=\"checked\"></span></div>" : "<div class=\"checker\"><span></span></div>";
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
      format: "{0:n" + decimals + "}",
      decimals: decimals,
      step: step || 0.01
    });
  }

  DropDownList(element, dataSource, textField = 'text', valueField = 'id') {
    return new kendo.ui.DropDownList(element, {
      autoBind: true,
      dataTextField: textField,
      dataValueField: valueField,
      dataSource: dataSource
    });
  }

  RemoteDropDownList(element, url, textField = 'text', valueField = 'id') {
    let dataSource = {
      transport: {
        dataType: "json",
        read: { url: url }
      },
      requestStart: e => this.OnRequestStart(e)
    };
    return this.DropDownList(element, dataSource, textField, valueField);
  }

  DropDownListEditor(container, options, dataSource, textField = 'text', valueField = 'id') {
    const input = $('<input/>');
    input.attr('name', options.field);
    input.appendTo(container);
    return this.DropDownList(input, dataSource, textField, valueField);
  }

  RemoteDropDownListEditor(container, options, url, textField = 'text', valueField = 'id') {
    const input = $('<input/>');
    input.attr('name', options.field);
    input.appendTo(container);
    return this.RemoteDropDownList(input, url, textField, valueField);
  }

  ComboBox(element, dataSource, textField = 'text', valueField = 'id') {
    return new kendo.ui.ComboBox(element, {
      autoBind: false,
      filter: "contains",
      dataTextField: textField,
      dataValueField: valueField,
      dataSource: dataSource
    });
  }

  RemoteComboBox(element, url, textField = 'text', valueField = 'id') {
    let dataSource = {
      transport: {
        serverFiltering: true,
        dateType: "json",
        read: { url: url }
      },
      requestStart: e => this.OnRequestStart(e)
    };
    return this.ComboBox(element, dataSource, textField, valueField);
  }

  RemoteFilterComboBox(element, options, url, textField = 'text', valueField = 'id') {
    let dataSource: kendo.data.DataSourceOptions = {
      serverFiltering: true,
      transport: {
        read: { url: url, type: 'post', dataType: 'json', contentType: 'application/json;charset=utf-8' },
        parameterMap: (opts, type) => {
          if (type == 'read') {
            let filter = opts.filter;
            if (options.buildFilter != undefined && filter && filter.filters.length) {
              filter = options.buildFilter(filter);
            }
            let group = this.getFilterGroup(filter, field => field);
            if (group == null) {
              group = new FilterGroup();
            }
            return JSON.stringify(group);
          }
        }
      },
      requestStart: e => this.OnRequestStart(e)
    };
    return this.ComboBox(element, dataSource, textField, valueField);
  }

  ComboBoxEditor(container, options, dataSource, textField = 'text', valueField = 'id') {
    const input = $('<input/>');
    input.attr('name', options.field);
    input.appendTo(container);
    return this.ComboBox(input, dataSource, textField, valueField);
  }

  RemoteComboBoxEditor(container, options, url, textField = 'text', valueField = 'id') {
    let dataSource = {
      transport: {
        serverFiltering: true,
        dateType: "json",
        read: { url: url }
      },
      requestStart: e => this.OnRequestStart(e)
    };
    return this.ComboBoxEditor(container, options, dataSource, textField, valueField);
  }

  RemoteFilterComboBoxEditor(container, options, url, textField = 'text', valueField = 'id') {
    let input = $('<input/>');
    input.attr('name', options.field);
    input.appendTo(container);
    let dataSource: kendo.data.DataSourceOptions = {
      serverFiltering: true,
      transport: {
        read: { url: url, type: 'post', dataType: 'json', contentType: 'application/json;charset=utf-8' },
        parameterMap: (opts, type) => {
          if (type == 'read') {
            let filter = opts.filter;
            if (options.buildFilter != undefined && filter && filter.filters.length) {
              filter = options.buildFilter(filter);
            }
            let group = this.getFilterGroup(filter, field => field);
            if (group == null) {
              group = new FilterGroup();
            }
            return JSON.stringify(group);
          }
        }
      },
      requestStart: e => this.OnRequestStart(e)
    };
    return new kendo.ui.ComboBox(input, {
      autoBind: false,
      filter: options.filter || 'contains',
      dataTextField: textField,
      dataValueField: valueField,
      text: options.selectText || '',
      value: options.selectValue || '',
      minLength: options.minLength || 1,
      delay: 800,
      dataSource: dataSource
    });
  }
  // #endregion
}

/**
 * KendoGrid组件基类
 */
export abstract class KendoGridComponentBase extends ComponentBase {

  public moduleName: string = null;
  public gridOptions: kendo.ui.GridOptions = null;
  public grid: kendo.ui.Grid = null;

  protected zone: NgZone;
  protected element: ElementRef;
  protected kendoui: KendouiService;

  constructor(injector: Injector) {
    super(injector);
    this.zone = injector.get(NgZone);
    this.element = injector.get(ElementRef);
    this.kendoui = injector.get(KendouiService);
  }

  protected InitBase() {
    let dataSourceOptions = this.GetDataSourceOptions();
    dataSourceOptions = this.FilterDataAuth(dataSourceOptions);
    const dataSource = new kendo.data.DataSource(dataSourceOptions);
    this.gridOptions = this.GetGridOptions(dataSource);
    this.gridOptions = this.FilterGridAuth(this.gridOptions);
  }

  protected ViewInitBase() {
    this.zone.runOutsideAngular(() => {
      this.GridInit();
      this.ToolbarInit();
    });
  }

  protected GridInit() {
    const $grid = $($(this.element.nativeElement).find("#grid-box-" + this.moduleName));
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
    $($toolbar).on("click", ".btn-refresh-function", e => this.grid.dataSource.read());
    // $($toolbar).on("click", ".toolbar-right .fullscreen", e => this.toggleGridFullScreen(e));
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let columns = this.GetGridColumns();
    let options = this.kendoui.CreateGridOptions(dataSource, columns);
    options.toolbar.push({ name: "refresh", template: `<button class="btn-refresh-function k-button k-button-icontext"><i class="k-icon k-i-refresh"></i>刷新</button>` });
    return options;
  }

  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    let model = this.GetModel();
    return this.kendoui.CreateDataSourceOptions(this.moduleName, model, this.FieldReplace, () => this.grid.dataSource.read());
  }

  protected FieldReplace(field: string): string {
    return field;
  }

  /**
   * 重写以获取Grid的模型设置Model
   */
  protected abstract GetModel(): any;
  /**
   * 重写以获取Grid的列设置Columns
   */
  protected abstract GetGridColumns(): kendo.ui.GridColumn[];
  /**
   * 根据 this.auth 的设置对 DataSource 进行权限过滤
   * @param options 数据源选项
   */
  protected FilterDataAuth(options: kendo.data.DataSourceOptions) {
    let transport = options.transport;
    if (!this.auth.Create && transport.create) {
      delete transport.create;
    }
    if (!this.auth.Update && transport.update) {
      delete transport.update;
    }
    if (!this.auth.Delete && transport.destroy) {
      delete transport.destroy;
    }
    return options;
  }
  /**
   * 根据 this.auth 的设置对 Grid 进行权限过滤
   * @param options Grid选项
   */
  protected FilterGridAuth(options: kendo.ui.GridOptions) {
    // 工具栏
    let toolbar = options.toolbar;
    if (!this.auth.Create) {
      this.osharp.remove(toolbar, (m: kendo.ui.GridToolbarItem) => m.name === "create");
    }
    // 新增、更新、删除都需要保存或取消
    if (!this.auth.Create && !this.auth.Update && !this.auth.Delete) {
      this.osharp.remove(toolbar, (m: kendo.ui.GridToolbarItem) => m.name === "save");
      this.osharp.remove(toolbar, (m: kendo.ui.GridToolbarItem) => m.name === "cancel");
    }
    // 新增和更新的编辑状态
    options.beforeEdit = e => {
      if (e.model.isNew()) {
        if (!this.auth.Create) {
          e.preventDefault();
        }
      } else {
        if (!this.auth.Update || !((e.model) as any).Updatable) {
          e.preventDefault();
        }
      }
    };

    // 命令列
    let cmdColumn = options.columns && options.columns.find(m => m.command != null);
    let cmds = cmdColumn && cmdColumn.command as kendo.ui.GridColumnCommandItem[];
    if (cmds) {
      if (!this.auth.Delete) {
        this.osharp.remove(cmds, m => m.name == "destroy");
      }
      if (cmds.length == 0) {
        this.osharp.remove(options.columns, m => m == cmdColumn);
      }
      cmdColumn.width = cmds.length * 45;
    }
    return options;
  }

  /**
   * 重置Grid高度
   * @param init 是否初次
   */
  protected ResizeGrid(init: boolean) {
    const $content = $("#grid-box-" + this.moduleName + " .k-grid-content");
    let winHeight = window.innerHeight;
    let otherHeight = $("layout-header.header").height() + $(".ant-tabs-nav-container").height() + 120 + 20;
    $content.height(winHeight - otherHeight);
  }

  protected InValidTab() {
    let els = $(this.element.nativeElement).parent().find("#grid-box-" + this.moduleName);
    return els && els.length > 0;
  }

  private KeyDownEvent(e) {
    if (!this.InValidTab() || !this.grid) {
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


export abstract class KendoTreeListComponentBase extends ComponentBase {

  protected moduleName: string = null;
  protected treeListOptions: kendo.ui.TreeListOptions = null;
  protected treeList: kendo.ui.TreeList = null;

  protected zone: NgZone;
  protected element: ElementRef;
  protected kendoui: KendouiService;

  constructor(injector: Injector) {
    super(injector);
    this.zone = injector.get(NgZone);
    this.element = injector.get(ElementRef);
    this.kendoui = injector.get(KendouiService);
  }

  protected InitBase() {
    const dataSourceOptions = this.GetDataSourceOptions();
    const dataSource = new kendo.data.TreeListDataSource(dataSourceOptions);
    this.treeListOptions = this.GetTreeListOptions(dataSource);
    this.treeListOptions = this.FilterTreeListAuth(this.treeListOptions);
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
      filterable: true,
      sortable: { allowUnsort: true, mode: 'multiple' },
      editable: { move: false }
    };
    return options;
  }
  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    const options: kendo.data.DataSourceOptions = {
      transport: {
        read: { url: "api/admin/" + this.moduleName + "/read", type: 'post', dataType: 'json', contentType: 'application/json;charset=utf-8' },
        create: { url: "api/admin/" + this.moduleName + "/create", type: 'post', dataType: 'json', contentType: 'application/json;charset=utf-8' },
        update: { url: "api/admin/" + this.moduleName + "/update", type: 'post', dataType: 'json', contentType: 'application/json;charset=utf-8' },
        destroy: { url: "api/admin/" + this.moduleName + "/delete", type: 'post' },
        parameterMap: (item, operation) => {
          if (operation == "read") {
            return item;
          }
          if ((operation == "create" || operation == "update") && item) {

            return JSON.stringify(item);
          }
          if (operation == "destroy" && item) {
            return { id: item['Id'] };
          }
        }
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

  /**
   * 根据 this.auth 的设置对 TreeList 选项进行权限过滤
   * @param options TreeList选项
   */
  protected FilterTreeListAuth(options: kendo.ui.TreeListOptions) {
    // 命令列
    let cmdColumn = options.columns && options.columns.find(m => m.command != null);
    let cmds = cmdColumn && cmdColumn.command as kendo.ui.TreeListColumnCommandItem[];
    if (cmds) {
      if (!this.auth.Create) {
        this.osharp.remove(cmds, m => m.name == "createChild");
      }
      if (!this.auth.Update) {
        this.osharp.remove(cmds, m => m.name == "edit");
      }
      if (!this.auth.Delete) {
        this.osharp.remove(cmds, m => m.name == "destroy");
      }
      if (cmds.length == 0) {
        this.osharp.remove(options.columns, m => m == cmdColumn);
      }
      cmdColumn.width = cmds.length * 45;
    }
    return options;
  }

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
