import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { FunctionViewComponent } from '@shared/components/function-view/function-view.component';
import { FilterGroup } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-user-function',
  templateUrl: './user-function.component.html',
  styles: []
})
export class UserFunctionComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'userfunction';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      {
        title: '操作', fixed: 'left', width: 100, buttons: [
          { text: '用户功能', icon: 'edit', acl: 'Root.Admin.Security.UserFunction.Read', click: row => this.showDrawer(row) },
        ]
      },
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'number' },
      { title: '用户名', index: 'UserName', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '邮箱', index: 'Email', sort: true, editable: true, filterable: true, ftype: 'string' },
    ];
  }

  // #region 查看功能

  drawerTitle = '';
  drawerVisible = false;
  functionReadUrl: string;
  @ViewChild('function', { static: false }) function: FunctionViewComponent;

  showDrawer(item) {
    this.drawerTitle = `查看功能 - ${item.UserName}`;
    this.drawerVisible = true;

    this.functionReadUrl = `api/admin/userfunction/readfunctions?userId=${item.Id}`;
    let filter: FilterGroup = new FilterGroup();
    setTimeout(() => {
      this.function.reload(filter);
    }, 100);
  }

  closeDrawer() {
    this.drawerVisible = false;
  }

  // #endregion
}
