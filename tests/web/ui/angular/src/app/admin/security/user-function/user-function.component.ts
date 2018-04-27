import { Component, OnInit, AfterViewInit, NgZone, ElementRef, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { osharp } from "../../../shared/osharp";
import { kendoui } from '../../../shared/kendoui';

@Component({
  selector: 'security-user-function',
  templateUrl: './user-function.component.html'
})
export class UserFunctionComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

  splitterOptions: kendo.ui.SplitterOptions = null;
  functionReadUrl: string = "/api/admin/userfunction/readfunctions";
  selectedUserId: number = 0;

  constructor(protected zone: NgZone, protected element: ElementRef, private http: HttpClient) {
    super(zone, element);
    this.moduleName = "userfunction";
    this.splitterOptions = {
      panes: [{ size: "60%" }, { collapsible: true, collapsed: false }]
    };
  }

  protected GetModel() {
    return {
      id: "Id",
      field: {
        Id: { type: 'number' },
        Name: { type: 'string' },
        Remark: { type: 'string' },
        IsAdmin: { type: "boolean" }
      }
    };
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      { field: "Id", title: "编号", width: 70 },
      {
        field: "UserName", title: "用户名", width: 150,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "Email", title: "邮箱", width: 250,
        filterable: osharp.Data.stringFilterable
      }
    ];
  }

  ngOnInit() {
    super.InitBase();
  }

  ngAfterViewInit(): void {
    super.ViewInitBase();
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    var options = super.GetGridOptions(dataSource);
    options.editable = false;
    options.selectable = true;
    options.toolbar = [{ template: '<span style="line-height:30px;">角色列表</span>' }];
    options.change = e => {
      var row = this.grid.select();
      if (row) {
        var data: any = this.grid.dataItem(row);
        this.selectedUserId = data.Id;
      }
    }
    return options;
  }

  onUserSelectedChange(functionGrid: kendo.ui.Grid) {
    var read: any = functionGrid.dataSource.options.transport.read;
    read.url = this.functionReadUrl + "?userId=" + this.selectedUserId;
    functionGrid.dataSource.read();
  }
}
