import { Component, OnInit, OnDestroy, AfterViewInit, NgZone, ElementRef, AfterContentInit, } from '@angular/core';


import { osharp } from "../../../shared/osharp";
import { kendoui } from "../../../shared/kendoui";
import { element } from 'protractor';

@Component({
  selector: 'security-function',
  template: `<div id="grid-box"></div>`
})

export class FunctionComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit, AfterContentInit {

  constructor(protected zone: NgZone, protected el: ElementRef) {
    super(zone, el);
    this.moduleName = "function";
  }

  ngOnInit() {
    super.InitBase();
  }
  ngAfterContentInit(): void {
    super.ViewInitBase();
  }
  ngAfterViewInit() {
    //super.ViewInitBase();
  }

  protected GetModel() {
    return {
      id: "Id",
      fields: {
        Id: { type: "string", editable: false },
        Name: { type: "string", editable: false },
        AccessType: { type: "number" },
        CacheExpirationSeconds: { type: "number" },
        AuditOperationEnabled: { type: "boolean" },
        AuditEntityEnabled: { type: "boolean" },
        IsCacheSliding: { type: "boolean" },
        IsController: { type: "boolean", editable: false },
        IsLocked: { type: "boolean" },
        IsAjax: { type: "boolean", editable: false },
        Area: { type: "string", editable: false },
        Controller: { type: "string", editable: false },
        Action: { type: "string", editable: false }
      }
    };
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        field: "Id", title: "编号", width: 200, hidden: true,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "Name", title: "功能名称", width: 200,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "AccessType", title: "功能类型", width: 95,
        template: d => osharp.Tools.valueToText(d.AccessType, osharp.Data.AccessTypes),
        editor: (container, options) => kendoui.Controls.DropDownListEditor(container, options, osharp.Data.AccessTypes),
        filterable: { ui: element => kendoui.Controls.DropDownList(element, osharp.Data.AccessTypes) }
      }, { field: "CacheExpirationSeconds", title: "缓存秒数", width: 95 },
      {
        field: "AuditOperationEnabled", title: "操作审计", width: 95,
        template: d => kendoui.Controls.Boolean(d.AuditOperationEnabled),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "AuditEntityEnabled", title: "数据审计", width: 95,
        template: d => kendoui.Controls.Boolean(d.AuditEntityEnabled),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "IsCacheSliding", title: "滑动过期", width: 95,
        template: d => kendoui.Controls.Boolean(d.IsCacheSliding),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "IsLocked", title: "已锁定", width: 95,
        template: d => kendoui.Controls.Boolean(d.IsLocked),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "IsAjax", title: "Ajax访问", width: 95,
        template: d => kendoui.Controls.Boolean(d.IsAjax),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "IsController", title: "是否控制器", width: 95,
        template: d => kendoui.Controls.Boolean(d.IsController),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      }, {
        field: "Area", title: "区域", width: 100, hidden: true,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "Controller", title: "控制器", width: 100, hidden: true,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "Action", title: "功能方法", width: 120, hidden: true,
        filterable: osharp.Data.stringFilterable
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
    options.group = [{ field: "Area" }, { field: "Controller" }];
    options.pageSize = 19;
    delete options.transport.create;
    delete options.transport.destroy;
    return options;
  }
}
