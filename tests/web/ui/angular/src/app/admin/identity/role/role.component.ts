import { Component, OnInit, OnDestroy, NgZone, ElementRef, AfterViewInit, } from '@angular/core';

import { kendoui } from '../../../shared/kendoui';
import { osharp } from '../../../shared/osharp';
import { ToasterService } from 'angular2-toaster/src/toaster.service';

@Component({
    selector: 'identity-role',
    template: `<div id="grid-box"></div>`
})
export class RoleComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

    constructor(protected zone: NgZone, protected el: ElementRef) {
        super(zone, el);
        this.moduleName = "role";
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
                Id: { type: "number", editable: false },
                Name: { type: "string", validation: { required: true } },
                Remark: { type: "string" },
                IsAdmin: { type: "boolean" },
                IsDefault: { type: "boolean" },
                IsLocked: { type: "boolean" },
                IsSystem: { type: "boolean", editable: false },
                CreatedTime: { type: "date", editable: false }
            }
        };
    }

    protected GetGridColumns(): kendo.ui.GridColumn[] {
        return [
            {
                command: [
                    { name: "destroy", iconClass: "k-icon k-i-delete", text: "" }
                ],
                width: 50
            },
            { field: "Id", title: "编号", width: 70 },
            {
                field: "Name", title: "角色名", width: 150,
                filterable: osharp.Data.stringFilterable
            },
            {
                field: "Remark", title: "备注", width: 250,
                filterable: osharp.Data.stringFilterable
            },
            {
                field: "IsAdmin", title: "管理", width: 95,
                template: d => kendoui.Controls.Boolean(d.IsAdmin)
            },
            {
                field: "IsDefault", title: "默认", width: 95,
                template: d => kendoui.Controls.Boolean(d.IsDefault)
            },
            {
                field: "IsLocked", title: "锁定", width: 95,
                template: d => kendoui.Controls.Boolean(d.IsLocked)
            },
            {
                field: "IsSystem", title: "系统", width: 95,
                template: d => kendoui.Controls.Boolean(d.IsSystem)
            },
            { field: "CreatedTime", title: "注册时间", width: 130, format: "{0:yy-MM-dd HH:mm}" }
        ];
    }
}
