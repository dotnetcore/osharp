import { Component, AfterViewInit, Injector, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-security-role-function',
  templateUrl: './role-function.component.html'
})
export class RoleFunctionComponent extends GridComponentBase implements AfterViewInit {

  splitterOptions: kendo.ui.SplitterOptions = null;
  functionReadUrl = "api/admin/rolefunction/readfunctions";
  selectedRoleId = 0;

  constructor(injector: Injector, private http: HttpClient) {
    super(injector);
    this.moduleName = "rolefunction";
    this.splitterOptions = {
      panes: [{ size: "60%" }, { collapsible: true, collapsed: false }]
    };
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
    return new AuthConfig("Root.Admin.Security.RoleFunction", ["Read"]);
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
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "Remark", title: "备注", width: 250,
        filterable: this.osharp.data.stringFilterable
      }, {
        field: "IsAdmin", title: "管理", width: 95,
        template: d => this.kendoui.Boolean(d.IsAdmin),
      }
    ];
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.editable = false;
    options.selectable = true;
    options.toolbar.push({ name: 'list', template: '<span style="line-height:30px;">角色列表</span>' });
    options.change = e => {
      let row = this.grid.select();
      if (row) {
        let data: any = this.grid.dataItem(row);
        this.selectedRoleId = data.Id;
      }
    };

    return options;
  }

  onRoleSelectedChange(functionGrid: kendo.ui.Grid) {
    let read: any = functionGrid.dataSource.options.transport.read;
    read.url = this.functionReadUrl + "?roleId=" + this.selectedRoleId;
    functionGrid.dataSource.read();
  }
}
