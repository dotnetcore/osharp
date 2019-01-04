import { Component, AfterViewInit, Injector } from '@angular/core';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-system-audit-operation',
  template: '<div id="grid-box-{{moduleName}}"></div>',
  styles: []
})
export class AuditOperationComponent extends GridComponentBase implements AfterViewInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = "auditOperation";
  }

  async ngAfterViewInit() {
    let auth = await this.checkAuth();
    if (auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Systems.AuditOperation", ["Read"]);
  }

  protected GetModel() {
    return {
      fields: {
        FunctionName: { type: "string", },
        UserId: { type: "string" },
        UserName: { type: "string" },
        NickName: { type: "string" },
        Ip: { type: "string" },
        OperationSystem: { type: "string" },
        Browser: { type: "string" },
        CreatedTime: { type: "date" },
        Message: { type: "string" },
        Elapsed: { type: "number" }
      }
    };
  }
  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      { field: "FunctionName", title: "功能", width: 150, filterable: this.osharp.data.stringFilterable },
      { field: "UserName", title: "用户名", width: 150, filterable: this.osharp.data.stringFilterable },
      { field: "NickName", title: "昵称", width: 150, filterable: this.osharp.data.stringFilterable },
      { field: "Ip", title: "IP地址", width: 115, filterable: this.osharp.data.stringFilterable },
      { field: "OperationSystem", title: "操作系统", width: 250, filterable: this.osharp.data.stringFilterable },
      { field: "Browser", title: "浏览器", width: 150, filterable: this.osharp.data.stringFilterable },
      {
        field: "ResultType", title: "结果", width: 65,
        template: d => this.osharp.valueToText(d.ResultType, this.osharp.data.ajaxResultType),
        filterable: { ui: element => this.kendoui.DropDownList(element, this.osharp.data.ajaxResultType) }
      },
      { field: "CreatedTime", title: "执行时间", width: 115, format: "{0:yy-MM-dd HH:mm}" },
      { field: "Elapsed", title: "耗时(ms)", width: 75 },
      { field: "Message", title: "消息", width: 300, filterable: this.osharp.data.stringFilterable },
    ];
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.detailTemplate = kendo.template('<div class="detailSplitter"><div class="left"></div><div class="right"></div></div>');
    options.detailInit = e => this.auditEntityInit(e);
    return options;
  }

  auditEntityInit(e: kendo.ui.GridDetailInitEvent) {
    let row = $(e.detailRow);
    //splitter
    let $splitter = row.find('.detailSplitter');
    let splitter = new kendo.ui.Splitter($splitter, { panes: [{ size: "50%" }, { collapsible: false, collapsed: false }] });

    //审计实体表格
    let dataSourceOptions = this.kendoui.CreateDataSourceOptions('auditEntity', {
      fields: {
        Name: { type: "string" },
        TypeName: { type: "string" },
        EntityKey: { type: "string" },
        OperateType: { type: "number" },
      }
    });
    dataSourceOptions.filter = { field: "OperationId", operator: "eq", value: e.data['Id'] };

    let gridOptions = this.kendoui.CreateGridOptions(new kendo.data.DataSource(dataSourceOptions), [
      { field: "Name", title: "实体名称", width: 130, filterable: this.osharp.data.stringFilterable },
      { field: "TypeName", title: "实体类型", width: 250, filterable: this.osharp.data.stringFilterable },
      { field: "EntityKey", title: "数据编号", width: 285 },
      {
        field: "OperateType", title: "操作", width: 75,
        template: d => this.osharp.valueToText(d.OperateType, this.osharp.data.operateType),
        filterable: { ui: element => this.kendoui.DropDownList(element, this.osharp.data.operateType) }
      }
    ]);
    gridOptions.toolbar = [];
    gridOptions.editable = false;
    gridOptions.selectable = true;
    gridOptions.change = ex => {
      let row2 = ex.sender.select();
      let data: any = ex.sender.dataItem(row2);
      data = data.Properties;
      if (this.propertyGrid && data) {
        this.propertyGrid.dataSource.data(data);
      }
    };
    let grid = new kendo.ui.Grid($(row.find('.detailSplitter .left')), gridOptions);
    this.auditPropertyInit($(row.find('.detailSplitter .right')));
  }

  // 审计实体属性表格
  private propertyGrid: kendo.ui.Grid = null;
  auditPropertyInit(el: any, data?: any) {
    let dataSourceOptions = this.kendoui.CreateLocalDataSourceOptions({
      fields: {
        DisplayName: { type: 'string' },
        FieldName: { type: 'string' },
        OriginalValue: { type: 'string' },
        NewValue: { type: 'string' },
        DataType: { type: 'string' }
      }
    }, data);
    let gridOptions = this.kendoui.CreateGridOptions(new kendo.data.DataSource(dataSourceOptions), [
      { field: "DisplayName", title: "属性名称", width: 100 },
      { field: "FieldName", title: "实体属性", width: 100 },
      { field: "OriginalValue", title: "原始值", width: 100 },
      { field: "NewValue", title: "变更值", width: 100 },
      { field: "DataType", title: "数据类型", width: 100 }
    ]);
    gridOptions.toolbar = [];
    gridOptions.editable = false;
    this.propertyGrid = new kendo.ui.Grid(el, gridOptions);
  }
}
