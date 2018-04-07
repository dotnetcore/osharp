import { Component, OnInit, AfterViewInit, NgZone, ElementRef, } from '@angular/core';

import { kendoui } from '../../../shared/kendoui';
import { osharp } from '../../../shared/osharp';

@Component({
  selector: 'identity-user-role',
  template: `<div id="grid-box"></div>`
})
export class UserRoleComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

  constructor(protected zone: NgZone, protected el: ElementRef) {
    super(zone, el);
    this.moduleName = "userrole";
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
        UserId: { type: "number", editable: false },
        RoleId: { type: "number", editable: false },
        UserName: { type: "string", validation: { required: true } },
        RoleName: { type: "string", validation: { required: true } },
        IsLocked: { type: "boolean" },
        CreatedTime: { type: "date", editable: false }
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
      },
      {
        field: "RoleId",
        title: "角色",
        width: 150,
        template: "#=RoleId#.#=RoleName#"
      },
      {
        field: "IsLocked",
        title: "锁定",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.IsLocked),
        editor: (container, options) => kendoui.Controls.BooleanEditor(container, options)
      },
      {
        field: "CreatedTime",
        title: "注册时间",
        width: 115,
        format: "{0:yy-MM-dd HH:mm}"
      }
    ];
  }

  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    var options = super.GetGridOptions(dataSource);
    options.toolbar.splice(0, 1);
    return options;
  }

  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    var options = super.GetDataSourceOptions();
    delete options.transport.destroy;
    return options;
  }

}
