import { Component, OnInit, AfterViewInit, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-security-user-function',
  templateUrl: './user-function.component.html'
})
export class UserFunctionComponent extends GridComponentBase implements OnInit, AfterViewInit {

  splitterOptions: kendo.ui.SplitterOptions = null;
  functionReadUrl = "/api/admin/userfunction/readfunctions";
  selectedUserId = 0;

  constructor(injector: Injector, private http: HttpClient) {
    super(injector);
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
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "Email", title: "邮箱", width: 250,
        filterable: this.osharp.data.stringFilterable
      }
    ];
  }

  async ngOnInit() {
    await this.checkAuth();
    super.InitBase();
  }

  ngAfterViewInit(): void {
    super.ViewInitBase();
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Identity.RoleFunction", []);
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.editable = false;
    options.selectable = true;
    options.toolbar = [{ template: '<span style="line-height:30px;">角色列表</span>' }];
    options.change = e => {
      let row = this.grid.select();
      if (row) {
        let data: any = this.grid.dataItem(row);
        this.selectedUserId = data.Id;
      }
    };
    return options;
  }

  onUserSelectedChange(functionGrid: kendo.ui.Grid) {
    let read: any = functionGrid.dataSource.options.transport.read;
    read.url = this.functionReadUrl + "?userId=" + this.selectedUserId;
    functionGrid.dataSource.read();
  }
}
