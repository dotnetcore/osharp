import { Component, AfterViewInit, Injector, } from '@angular/core';

import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-security-function',
  template: `<div id="grid-box-{{moduleName}}"></div>`
})

export class FunctionComponent extends GridComponentBase implements AfterViewInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = "function";
  }

  async ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    } else {
      this.osharp.error("无权查看此页面");
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Security.Function", ["Read", "Update"]);
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
        Action: { type: "string", editable: false },
        Updatable: { type: "boolean", editable: false },
      }
    };
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        field: "Id", title: "编号", width: 200, hidden: true,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "Name", title: "功能名称", width: 200,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "AccessType", title: "功能类型", width: 95,
        template: d => this.osharp.valueToText(d.AccessType, this.osharp.data.accessType),
        editor: (container, options) => this.kendoui.DropDownListEditor(container, options, this.osharp.data.accessType),
        filterable: { ui: element => this.kendoui.DropDownList(element, this.osharp.data.accessType) }
      }, { field: "CacheExpirationSeconds", title: "缓存秒数", width: 95 },
      {
        field: "AuditOperationEnabled", title: "操作审计", width: 95,
        template: d => this.kendoui.Boolean(d.AuditOperationEnabled),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "AuditEntityEnabled", title: "数据审计", width: 95,
        template: d => this.kendoui.Boolean(d.AuditEntityEnabled),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "IsCacheSliding", title: "滑动过期", width: 95,
        template: d => this.kendoui.Boolean(d.IsCacheSliding),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "IsLocked", title: "已锁定", width: 95,
        template: d => this.kendoui.Boolean(d.IsLocked),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "IsAjax", title: "Ajax访问", width: 95,
        template: d => this.kendoui.Boolean(d.IsAjax),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "IsController", title: "是否控制器", width: 95,
        template: d => this.kendoui.Boolean(d.IsController),
        editor: (container, options) => this.kendoui.BooleanEditor(container, options)
      }, {
        field: "Area", title: "区域", width: 100, hidden: true,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "Controller", title: "控制器", width: 100, hidden: true,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "Action", title: "功能方法", width: 120, hidden: true,
        filterable: this.osharp.data.stringFilterable
      }
    ];
  }

  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    let options = super.GetDataSourceOptions();
    options.group = [{ field: "Area" }, { field: "Controller" }];
    options.pageSize = 20;
    return options;
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.columnMenu = { sortable: false };
    return options;
  }
}
