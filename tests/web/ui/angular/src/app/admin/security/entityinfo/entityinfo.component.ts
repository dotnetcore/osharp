import { Component, OnInit, OnDestroy, NgZone, ElementRef, AfterViewInit, } from '@angular/core';

import { kendoui } from '../../../shared/kendoui';
import { osharp } from '../../../shared/osharp';

@Component({
  selector: 'security-entityinfo',
  template: `<div id="grid-box"></div>`
})
export class EntityinfoComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

  constructor(protected zone: NgZone, protected el: ElementRef) {
    super(zone, el);
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
      { field: "Name", title: "实体名称", width: 150, filterable: osharp.Data.stringFilterable },
      { field: "TypeName", title: "实体类型", width: 250, filterable: osharp.Data.stringFilterable },
      {
        field: "AuditEnabled", title: "数据审计", width: 95,
        template: d => kendoui.Controls.Boolean(d.AuditEnabled),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }
    ];
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    var options = super.GetGridOptions(dataSource);
    options.columnMenu = { sortable: false };
    options.toolbar.splice(0, 1);
    return options;
  }

  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    var options = super.GetDataSourceOptions();
    delete options.transport.create;
    delete options.transport.destroy;
    return options;
  }
}
