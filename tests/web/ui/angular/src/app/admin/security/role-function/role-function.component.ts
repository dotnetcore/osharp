import { Component, OnInit, AfterViewInit, NgZone, ElementRef, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { osharp } from "../../../shared/osharp";
import { kendoui } from '../../../shared/kendoui';

@Component({
  selector: 'security-role-function',
  templateUrl: './role-function.component.html'
})
export class RoleFunctionComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

  splitterOptions: kendo.ui.SplitterOptions = null;
  functionReadUrl: string = "/api/admin/rolefunction/readfunctions";
  selectedRoleId: number = 0;

  constructor(protected zone: NgZone, protected element: ElementRef, private http: HttpClient) {
    super(zone, element);
    this.moduleName = "rolefunction";
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
        field: "Name", title: "角色名", width: 150,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "Remark", title: "备注", width: 250,
        filterable: osharp.Data.stringFilterable
      }, {
        field: "IsAdmin", title: "管理", width: 95,
        template: d => kendoui.Controls.Boolean(d.IsAdmin),
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
        this.selectedRoleId = data.Id;
      }
    }
    return options;
  }

  onRoleSelectedChange(functionGrid: kendo.ui.Grid) {
    var read: any = functionGrid.dataSource.options.transport.read;
    read.url = this.functionReadUrl + "?roleId=" + this.selectedRoleId;
    functionGrid.dataSource.read();
  }
}
