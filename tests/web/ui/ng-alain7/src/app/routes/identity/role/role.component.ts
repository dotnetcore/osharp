import { Component, OnInit, Injector } from '@angular/core';
import { STComponentBase } from '@shared/osharp/services/ng-alain.service';
import { STColumn } from '@delon/abc';

@Component({
  selector: 'app-identity-role',
  templateUrl: './role.component.html',
  styles: []
})
export class RoleComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'role';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): STColumn[] {
    return [
      { title: '编号', index: 'Id' },
      { title: '名称', index: 'Name' },
      { title: '备注', index: 'Remark' },
      { title: '管理角色', index: 'IsAdmin', type: "yn" },
      { title: '默认', index: 'IsDefault', type: "yn" },
      { title: '锁定', index: 'IsLocked', type: "yn" },
      { title: '创建时间', index: 'CreatedTime', type: 'date' }
    ];
  }

}
