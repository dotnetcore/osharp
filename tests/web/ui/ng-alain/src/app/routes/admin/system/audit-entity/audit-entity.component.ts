import { Component, OnInit, Injector, AfterViewInit } from '@angular/core';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-system-audit-entity',
  templateUrl: './audit-entity.component.html',
  styles: []
})
export class AuditEntityComponent extends GridComponentBase implements AfterViewInit {

  splitterOptions: kendo.ui.SplitterOptions = null;

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = "auditEntity";
    this.splitterOptions = {
      panes: [{ size: "55%" }, { collapsible: false, collapsed: false }]
    };
  }

  async ngAfterViewInit() {
    let auth = await this.checkAuth();

    if (auth.Read) {
      super.InitBase();
      super.ViewInitBase();

      let propel = $(this.element.nativeElement).find("#audit-entity-property");
      this.auditPropertyInit(propel);
    } else {
      this.osharp.error("无权查看此页");
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Systems.AuditEntity", ["Read"]);
  }

  protected GetModel() {
    return {
      fields: {
        Name: { type: "string" },
        TypeName: { type: "string" },
        EntityKey: { type: "string" },
        OperateType: { type: "number" },
        FunctionName: { type: "string" },
        NickName: { type: "string" },
        CreatedTime: { type: "date" },
      }
    };
  }
  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      { field: "Name", title: "实体名称", width: 100, filterable: this.osharp.data.stringFilterable },
      { field: "TypeName", title: "实体类型", width: 200, filterable: this.osharp.data.stringFilterable },
      { field: "EntityKey", title: "数据编号", width: 160 },
      {
        field: "OperateType", title: "操作", width: 65,
        template: d => this.osharp.valueToText(d.OperateType, this.osharp.data.operateType),
        filterable: { ui: element => this.kendoui.DropDownList(element, this.osharp.data.operateType) }
      },
      { field: "NickName", title: "执行人", width: 100, filterable: this.osharp.data.stringFilterable },
      { field: "FunctionName", title: "所属功能", width: 120, filterable: this.osharp.data.stringFilterable },
      { field: "CreatedTime", title: "执行时间", width: 115, format: "{0:yy-MM-dd HH:mm}" },
    ];
  }

  protected FieldReplace(field: string): string {
    if (field == "FunctionName") return "Operation.FunctionName";
    if (field == "NickName") return "Operation.NickName";
    return field;
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.selectable = true;
    options.change = e => {
      let data: any = e.sender.dataItem(e.sender.select());

      let properties = data.Properties;
      if (this.propertyGrid && properties) {
        this.propertyGrid.dataSource.data(properties);
      }
    };
    return options;
  }

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
