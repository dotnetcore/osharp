import { Component, OnInit, OnDestroy, AfterViewInit, Injector, } from '@angular/core';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';

@Component({
  selector: 'admin-security-entityinfo',
  template: `<div id="grid-box-{{moduleName}}"></div>`
})
export class EntityinfoComponent extends GridComponentBase implements OnInit, AfterViewInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = "entityinfo";
  }

  ngOnInit() {
    super.InitBase();
  }

  ngAfterViewInit() {
    super.ViewInitBase();
  }

  protected GetModel() {
    return {
      id: "Id",
      fields: {
        Name: { type: "string", editable: false },
        TypeName: { type: "string", editable: false },
        AuditEnabled: { type: "boolean" }
      }
    };
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      { field: "Name", title: "实体名称", width: 150, filterable: this.osharp.data.stringFilterable },
      { field: "TypeName", title: "实体类型", width: 250, filterable: this.osharp.data.stringFilterable },
      {
        field: "AuditEnabled", title: "数据审计", width: 95,
        template: d => this.kendoui.Boolean(d.AuditEnabled),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }
    ];
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.columnMenu = { sortable: false };
    options.toolbar.splice(0, 1);
    return options;
  }

  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    let options = super.GetDataSourceOptions();
    delete options.transport.create;
    delete options.transport.destroy;
    return options;
  }
}
