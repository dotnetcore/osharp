import { Component, OnInit, OnDestroy, AfterViewInit, NgZone } from '@angular/core';
declare var $: any;

import { UserService, } from './user.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
    selector: 'identity-user',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit, OnDestroy, AfterViewInit {

    moduleName = "user";
    grid: kendo.ui.Grid;
    gridOptions: kendo.ui.GridOptions;

    constructor(private _service: UserService, private zone: NgZone) { }

    ngOnInit() {
        this.gridOptions = {
            dataSource: {
                transport: {
                    read: { url: "/api/admin/" + this.moduleName + "/read", type: 'post' },
                },
                schema: {
                    model: {
                        id: "Id",
                        fields: {
                            Id: { type: "number", editable: false },
                            UserName: { type: "string", validation: { required: true } },
                            Email: { type: "string", validation: { required: true } },
                        }
                    },
                    data: d => d.Rows,
                    total: d => d.Total
                }
            },

            columns: [
                { field: "Id", title: "编号", width: 70 },
                { field: "UserName", title: "用户名", width: 150 },
                { field: "Email", title: "邮箱", width: 150 },
            ]
        };
    }

    ngAfterViewInit(): void {
        this.zone.runOutsideAngular(() => {
            this.grid = new kendo.ui.Grid($("#grid-user"), this.gridOptions);
        });
    }


    ngOnDestroy() {
    }
}
