import { Component, OnInit, ElementRef, AfterViewInit, NgZone } from '@angular/core';
declare var $: any;

import { ModuleService, } from './module.service';
import { kendoui } from '../../../shared/kendoui';

@Component({
    selector: 'security-module',
    templateUrl: './module.component.html'
})
export class ModuleComponent extends kendoui.TreeListComponentBase implements OnInit, AfterViewInit {

    splitterOptions: kendo.ui.SplitterOptions = null;

    constructor(protected zone: NgZone, protected element: ElementRef) {
        super(zone, element);
        this.moduleName = "module";
        this.splitterOptions = {
            panes: [{ size: "60%" }, { collapsible: true, collapsed: false }]
        };
    }

    ngOnInit() {
        super.InitBase();
    }
    ngAfterViewInit(): void {
        super.ViewInitBase();
    }

    protected GetModel() {
        return {
            id: "Id",
            parentId: "ParentId",
            fields: {
                Id: { type: "number", nullable: false, editable: false },
                ParentId: { type: "number", nullable: true },
                Name: { type: "string" },
                OrderCode: { type: "number" },
                Remark: { type: "string" }
            },
            expanded: true
        };
    }
    protected GetTreeListColumns(): kendo.ui.TreeListColumn[] {
        return [
            {
                field: "Name", title: "名称", width: 200,
                template: d => "<span>" + d.Id + ". " + d.Name + "</span>"
            },
            { field: "Remark", title: "备注", width: 200 },
            {
                field: "OrderCode", title: "排序", width: 70,
                editor: (container, options) => kendoui.Controls.NumberEditor(container, options, 3)
            },
            {
                title: "操作",
                command: [
                    {
                        name: "setFuncs",
                        imageClass: "k-i-categorize",
                        text: " ",
                        click: function (e) {
                            //$scope.module.setFuncs.open(e);
                        }
                    },
                    { name: "createChild", text: " " },
                    { name: "edit", text: " " },
                    { name: "destroy", imageClass: "k-i-delete", text: " " }
                ],
                width: 180
            }
        ];
    }

}
