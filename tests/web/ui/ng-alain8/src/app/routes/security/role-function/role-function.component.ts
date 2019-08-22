import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { FunctionViewComponent } from '@shared/components/function-view/function-view.component';
import { FilterGroup } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-role-function',
  templateUrl: './role-function.component.html',
  styles: []
})
export class RoleFunctionComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'rolefunction';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      {
        title: '操作', fixed: 'left', width: 100, buttons: [
          { text: '角色功能', icon: 'edit', acl: 'Root.Admin.Security.RoleFunction.Read', click: row => this.showDrawer(row) },
        ]
      },
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'number' },
      { title: '名称', index: 'Name', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '备注', index: 'Remark', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '管理角色', index: 'IsAdmin', sort: true, type: "yn", editable: true, filterable: true }
    ];
  }

  // #region 抽屉

  drawerTitle = '';
  drawerVisible = false;
  functionReadUrl: string;
  @ViewChild('function', { static: false }) function: FunctionViewComponent;

  showDrawer(item) {
    this.drawerTitle = `查看功能 - ${item.Name}`;
    this.drawerVisible = true;

    this.functionReadUrl = `api/admin/rolefunction/readfunctions?roleId=${item.Id}`;
    // this.function.ReadUrl = this.functionReadUrl;
    let filter: FilterGroup = new FilterGroup();
    this.function.reload(filter);
  }

  closeDrawer() {
    this.drawerVisible = false;
  }

  // #endregion
}
