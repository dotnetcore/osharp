import { Component, OnInit, OnDestroy, AfterViewInit, NgZone, ElementRef, } from '@angular/core';
declare var $: any;

import { KendouiModule } from "../../../shared/kendoui/kendoui.module";
import { osharp } from "../../../shared/osharp";
import { kendoui } from "../../../shared/kendoui";
import { element } from 'protractor';

@Component({
    selector: 'security-function',
    template: `<div id="grid-box"></div>`
})

export class FunctionComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

    constructor(protected zone: NgZone, protected el: ElementRef) {
        super(zone, el);
        this.moduleName = "function";
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
                AccessType: { type: "number" },
                CacheExpirationSeconds: { type: "number" },
                AuditOperationEnabled: { type: "boolean" },
                AuditEntityEnabled: { type: "boolean" },
                IsCacheSliding: { type: "boolean" },
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
                field: "Name", title: "功能名称", width: 200,
                filterable: osharp.Data.stringFilterable
            },
            {
                field: "AccessType", title: "功能类型", width: 95,
                template: d => osharp.Tools.valueToText(d.AccessType, osharp.Data.AccessTypes),
                editor: (container, options) => kendoui.Controls.DropDownListEditor(container, options, osharp.Data.AccessTypes),
                filterable: { ui: element => kendoui.Controls.DropDownList(element, osharp.Data.AccessTypes) }
            },
            { field: "CacheExpirationSeconds", title: "缓存秒数", width: 95 },
            {
                field: "AuditOperationEnabled", title: "操作审计", width: 95,
                template: d => kendoui.Controls.Boolean(d.AuditOperationEnabled)
            },
            {
                field: "AuditEntityEnabled", title: "数据审计", width: 95,
                template: d => kendoui.Controls.Boolean(d.AuditEntityEnabled)
            },
            {
                field: "IsCacheSliding", title: "滑动过期", width: 95,
                template: d => kendoui.Controls.Boolean(d.IsCacheSliding)
            },
            {
                field: "IsLocked", title: "已锁定", width: 95,
                template: d => kendoui.Controls.Boolean(d.IsLocked)
            },
            {
                field: "IsAjax", title: "Ajax访问", width: 95,
                template: d => kendoui.Controls.Boolean(d.IsAjax)
            },
            {
                field: "Area", title: "区域", width: 100, hidden: true,
                filterable: osharp.Data.stringFilterable
            },
            {
                field: "Controller", title: "控制器", width: 100, hidden: true,
                filterable: osharp.Data.stringFilterable
            },
            {
                field: "Action", title: "功能方法", width: 120, hidden: true,
                filterable: osharp.Data.stringFilterable
            }
        ];
    }

    protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
        var options = super.GetGridOptions(dataSource);
        options.columnMenu = { sortable: false };
        options.toolbar = [{ name: 'save' }, { name: 'cancel' }];
        return options;
    }

    protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
        var options = super.GetDataSourceOptions();
        delete options.transport.create;
        delete options.transport.destroy;
        return options;
    }
}
