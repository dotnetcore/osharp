import { Component, OnInit, OnDestroy, AfterViewInit, NgZone, ElementRef } from '@angular/core';
declare var $: any;

import { kendoui } from '../../../shared/kendoui';
import { osharp } from '../../../shared/osharp';

@Component({
    selector: 'identity-user',
    templateUrl: './user.component.html'
})
export class UserComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

    window: kendo.ui.Window;
    windowOptions: kendo.ui.WindowOptions;
    tabstripOptions: kendo.ui.TabStripOptions;
    roleTreeOptions: kendo.ui.TreeViewOptions;
    roleTree: kendo.ui.TreeView;
    moduleTreeOptions: kendo.ui.TreeViewOptions;
    moduleTree: kendo.ui.TreeView;

    constructor(protected zone: NgZone, protected el: ElementRef) {
        super(zone, el);
        this.moduleName = "user";
        this.windowOptions = {
            visible: false, width: 500, height: 620, modal: true, title: "用户权限设置", actions: ["Pin", "Minimize", "Maximize", "Close"],
            resize: e => this.onWinResize(e)
        };
        this.roleTreeOptions = { checkboxes: { checkChildren: true, }, dataTextField: "Name", select: e => this.onTreeNodeSelect(e), dataBound: e => this.onTreeDataBound(e) };
        this.moduleTreeOptions = { checkboxes: { checkChildren: true }, dataTextField: "Name", select: e => this.onTreeNodeSelect(e), dataBound: e => this.onTreeDataBound(e) };
    }

    ngOnInit() {
        super.InitBase();
    }

    ngAfterViewInit() {
        super.ViewInitBase();
    }

    //#region GridBase

    protected GetModel() {
        return {
            id: "Id",
            fields: {
                Id: { type: "number", editable: false },
                UserName: { type: "string", validation: { required: true } },
                Email: { type: "string", validation: { required: true } },
                EmailConfirmed: { type: "boolean" },
                PhoneNumber: { type: "string" },
                PhoneNumberConfirmed: { type: "boolean" },
                LockoutEnabled: { type: "boolean", editable: false },
                LockoutEnd: { type: "date", editable: false },
                AccessFailedCount: { type: "number", editable: false },
                CreatedTime: { type: "date", editable: false },
                Roles: { editable: false }
            }
        };
    }
    protected GetGridColumns(): kendo.ui.GridColumn[] {
        return [
            {
                command: [
                    { name: "permission", text: "", iconClass: "k-icon k-i-unlink-horizontal", click: e => this.windowOpen(e) },
                    { name: "destroy", iconClass: "k-icon k-i-delete", text: "" }
                ],
                width: 100
            },
            { field: "Id", title: "编号", width: 70 },
            {
                field: "UserName",
                title: "用户名",
                width: 150,
                filterable: osharp.Data.stringFilterable
            }, {
                field: "Email",
                title: "邮箱",
                width: 200,
                filterable: osharp.Data.stringFilterable
            }, {
                field: "EmailConfirmed",
                title: "邮箱确认",
                width: 95,
                template: d => kendoui.Controls.Boolean(d.EmailConfirmed),
                editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
            }, {
                field: "PhoneNumber",
                title: "手机号",
                width: 105,
                filterable: osharp.Data.stringFilterable
            }, {
                field: "PhoneNumberConfirmed",
                title: "手机确认",
                width: 95,
                template: d => kendoui.Controls.Boolean(d.PhoneNumberConfirmed),
                editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
            }, {
                field: "Roles",
                title: "角色",
                width: 180,
                template: d => osharp.Tools.expandAndToString(d.Roles)
            }, {
                field: "Locked",
                title: "是否锁定",
                width: 95,
                template: d => kendoui.Controls.Boolean(d.Locked),
                editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
            }, {
                field: "LockoutEnabled",
                title: "是否登录锁",
                width: 95,
                template: d => kendoui.Controls.Boolean(d.LockoutEnabled),
            }, {
                field: "LockoutEnd",
                title: "锁时间",
                width: 115,
                format: "{0:yy-MM-dd HH:mm}"
            }, {
                field: "AccessFailedCount",
                title: "登录错误",
                width: 95
            }, {
                field: "CreatedTime",
                title: "注册时间",
                width: 115,
                format: "{0:yy-MM-dd HH:mm}"
            }
        ];
    }

    protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
        var options = super.GetGridOptions(dataSource);
        options.toolbar = [
            { name: 'create' },
            { name: 'save' },
            { name: 'cancel' },
            {
                name: 'fullscreen', template: function () {
                    return `
<ul class="toolbar-right">
    <li><a (click)="toggleGridFullScreen($event)"><em class="fa fa-expand"></em></a></li>
</ul>
`;
                }
            }
        ];
        return options;
    }

    //#endregion

    //#region Grid
    public toggleGridFullScreen(e) {
        console.log("grid-full-screen");
    }

    public toggleGridFullScreen1(e) {
        console.log("toggleGridFullScreen1");
    }
    //#endregion

    //#region Window

    private windowOpen(e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var data: any = this.grid.dataItem(tr);
        this.window.title("用户权限设置-" + data.UserName).open().center().resize();
        //设置树数据
        this.roleTree.setDataSource(new kendo.data.HierarchicalDataSource({ transport: { read: { url: "/api/admin/role/ReadUserRoles?userId=" + data.Id } } }));
        this.moduleTree.setDataSource(new kendo.data.HierarchicalDataSource({ transport: { read: { url: "/api/admin/module/ReadUserModules?userId=" + data.Id } } }));
    }
    onWinInit(win) {
        this.window = win;
    }
    onWinClose(win) {
        console.log("close111");
    }
    onWinSubmit(win) {
        console.log("submit111");
    }
    private onWinResize(e) {
        $(".win-content .k-tabstrip .k-content").height(e.height - 140);
    }

    //#endregion

    //#region Tree

    onRoleTreeInit(tree) {
        this.roleTree = tree;
    }
    onModuleTreeInit(tree) {
        this.moduleTree = tree;
    }

    //#endregion

}
