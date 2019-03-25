import { Component, OnInit, Injector } from '@angular/core';
import { STComponentBase } from '@shared/osharp/services/ng-alain.service';
import { STColumn, STData } from '@delon/abc';
import { OsharpSTColumn } from '@shared/osharp/services/ng-alain.types';
import { SFUISchema } from '@delon/form';

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

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      {
        title: '操作', fixed: 'left', width: 65, buttons: [{
          text: '操作', children: [
            { text: '编辑', icon: 'edit', acl: 'Root.Admin.Identity.Role.Update', iif: row => row.Updatable, click: row => this.edit(row) },
            { text: '权限', icon: 'safety', acl: 'Root.Admin.Identity.Role.SetModules', click: row => this.permiss(row) },
            { text: '删除', icon: 'delete', type: 'del', acl: 'Root.Admin.Identity.Role.Delete', iif: row => row.Deletable, click: row => this.delete(row) },
          ]
        }]
      },
      { title: '编号', index: 'Id', readOnly: true, editable: true, ftype: 'number' },
      { title: '名称', index: 'Name', editable: true, ftype: 'string' },
      { title: '备注', index: 'Remark', editable: true, ftype: 'string' },
      { title: '管理角色', index: 'IsAdmin', type: "yn", editable: true },
      { title: '默认', index: 'IsDefault', type: "yn", editable: true },
      { title: '锁定', index: 'IsLocked', type: "yn", editable: true },
      { title: '创建时间', index: 'CreatedTime', type: 'date' }
    ];
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Name: { grid: { span: 24 } },
      $Remark: { widget: 'textarea', grid: { span: 24 } }
    };
    return ui;
  }

  private permiss(row: STData) {
    this.osharp.info(`设置角色 ${row.Name} 的权限`);
  }
}
