import { Component, AfterViewInit, Injector } from '@angular/core';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig, FilterGroup, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { container } from '../../../../../../node_modules/@angular/core/src/render3/instructions';

@Component({
  selector: 'admin-security-role-entity',
  templateUrl: './role-entity.component.html'
})
export class RoleEntityComponent extends GridComponentBase implements AfterViewInit {

  splitterOptions: kendo.ui.SplitterOptions;
  http: HttpClient;
  filterGroup: FilterGroup;
  entityType: string;
  selectName: string = "未选择";
  groupJson: string;
  selectData: any = null;

  constructor(injector: Injector) {
    super(injector);
    this.http = injector.get(HttpClient);
    this.moduleName = "roleentity";
    this.splitterOptions = {
      panes: [{ size: "50%" }, { collapsible: true, collapsed: false }]
    };
  }

  async ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Security.RoleEntity", ["Read", "Create", "Update", "Delete"])
  }

  protected GetModel() {
    return {
      id: "Id",
      fields: {
        Id: { type: "string", editable: false, defaultValue: "00000000-0000-0000-0000-000000000000" },
        RoleId: { type: "number", editable: true },
        EntityId: { type: "string", editable: true },
        RoleName: { type: "string", validation: { required: true } },
        EntityName: { type: "string", validation: { required: true } },
        EntityType: { type: "string", validation: { required: true } },
        FilterGroup: { type: "object" },
        IsLocked: { type: "boolean" },
        CreatedTime: { type: "date", editable: false }
      }
    };
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    const columns = [{
      command: [
        { name: "destroy", iconClass: "k-icon k-i-delete", text: "" },
      ],
      width: 100
    }, {
      field: "RoleId",
      title: "角色",
      width: 150,
      template: "#=RoleId#.#=RoleName#",
      editor: (container, options) => this.kendoui.RemoteDropDownListEditor(container, options, "api/admin/role/ReadNode", "RoleName", "RoleId"),
      filterable: { ui: el => this.kendoui.RemoteDropDownList(el, "api/admin/role/ReadNode", "RoleName", "RoleId") }
    }, {
      field: "EntityId",
      title: "数据实体",
      width: 300,
      template: "#=EntityName# [#=EntityType#]",
      editor: (container, options) => this.kendoui.RemoteDropDownListEditor(container, options, "api/admin/entityinfo/ReadNode", "Name", "Id"),
      filterable: { ui: el => this.kendoui.RemoteDropDownList(el, "api/admin/entityinfo/ReadNode", "Name", "Id") }
    }, {
      field: "IsLocked",
      title: "锁定",
      width: 95,
      template: d => this.kendoui.Boolean(d.IsLocked),
      editor: (container, options) => this.kendoui.BooleanEditor(container, options)
    }, {
      field: "CreatedTime",
      title: "注册时间",
      width: 115,
      format: "{0:yy-MM-dd HH:mm}"
    }];
    return columns;
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.selectable = true;
    options.change = e => {
      let row = this.grid.select();
      if (row) {
        let data: any = this.grid.dataItem(row);
        if (data) {
          this.selectData = data;
          this.selectName = `角色: ${data.RoleName} + 实体: ${data.EntityName}`
          if (data.FilterGroup) {
            this.filterGroup = data.FilterGroup;
          } else {
            this.filterGroup = new FilterGroup();
          }
          this.entityType = data.EntityType;
        }
        else {
          this.selectName = "未选择";
          this.filterGroup = new FilterGroup();
          this.entityType = null;
        }
      } else {
        this.selectName = "未选择";
        this.filterGroup = new FilterGroup();
        this.entityType = null;
      }
    }

    return options;
  }

  showGroupJson() {
    this.groupJson = JSON.stringify(this.filterGroup, null, 2);
  }
  saveFilterGroup() {
    if (this.entityType == null) {
      this.osharp.info("请在左边选中一行，再进行操作");
      return;
    }
    let data = this.selectData;
    let dto = { Id: data.Id, RoleId: data.RoleId, EntityId: data.EntityId, FilterGroup: this.filterGroup, IsLocked: data.IsLocked };
    this.http.post<AjaxResult>("api/admin/roleentity/Update", [dto]).subscribe(res => {
      this.osharp.ajaxResult(res);
    });
  }
}
