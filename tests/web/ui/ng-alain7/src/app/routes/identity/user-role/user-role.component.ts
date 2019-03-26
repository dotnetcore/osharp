import { Component, OnInit, Injector } from '@angular/core';
import { STComponentBase, AlainService } from '@shared/osharp/services/ng-alain.service';
import { OsharpSTColumn } from '@shared/osharp/services/ng-alain.types';

@Component({
  selector: 'app-identity-user-role',
  templateUrl: './user-role.component.html'
})
export class UserRoleComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'userRole';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      {
        title: '操作', fixed: 'left', width: 65, buttons: [{
          text: '操作', children: [
            { text: '编辑', icon: 'edit', acl: 'Root.Admin.Identity.UserRole.Update', iif: row => row.Updatable, click: row => this.edit(row) },
            { text: '删除', icon: 'delete', type: 'del', acl: 'Root.Admin.Identity.UserRole.Delete', iif: row => row.Deletable, click: row => this.delete(row) },
          ]
        }]
      },
      { title: '用户', index: 'UserName', editable: true, ftype: 'string', readOnly: true, format: d => `${d.UserId}. ${d.UserName}`, ui: { widget: 'text' } },
      { title: '角色', index: 'RoleName', editable: true, ftype: 'string', readOnly: true, format: d => `${d.RoleId}. ${d.RoleName}`, ui: { widget: 'text' } },
      { title: '锁定', index: 'IsLocked', type: 'yn', editable: true },
      { title: '注册时间', index: 'CreatedTime', type: 'date' },
    ];
  }
}
