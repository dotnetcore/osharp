import { Component, AfterViewInit, Injector, } from '@angular/core';
import { GridComponentBase } from '@shared/osharp/services/kendoui.service';
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  selector: 'admin-identity-user-role',
  template: `<div id="grid-box-{{moduleName}}"></div>`
})
export class UserRoleComponent extends GridComponentBase implements AfterViewInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = "userrole";
  }

  async ngAfterViewInit() {
    let auth = await this.checkAuth();
    if (auth.Read) {
      super.InitBase();
      super.ViewInitBase();
    }
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Admin.Identity.UserRole", ["Read", "Update"]);
  }

  protected GetModel() {
    return {
      id: "Id",
      fields: {
        UserId: { type: "number", editable: false },
        RoleId: { type: "number", editable: false },
        UserName: { type: "string", validation: { required: true } },
        RoleName: { type: "string", validation: { required: true } },
        IsLocked: { type: "boolean" },
        CreatedTime: { type: "date", editable: false },
        Updatable: { type: "boolean", editable: false },
      }
    };
  }
  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        field: "UserId",
        title: "用户",
        width: 150,
        template: "#=UserId#.#=UserName#"
      }, {
        field: "RoleId",
        title: "角色",
        width: 150,
        template: "#=RoleId#.#=RoleName#"
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
      }
    ];
  }

  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    let options = super.GetDataSourceOptions();
    delete options.transport.destroy;
    return options;
  }

}
