import { Component, OnInit, Injector } from '@angular/core';
import { STComponentBase, AlainService } from '@shared/osharp/services/ng-alain.service';
import { OsharpSTColumn } from '@shared/osharp/services/ng-alain.types';
import { SFUISchema, SFSchemaEnumType } from '@delon/form';

@Component({
  selector: 'app-role-entityinfo',
  templateUrl: './role-entityinfo.component.html',
  styles: []
})
export class RoleEntityinfoComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector, private alain: AlainService) {
    super(injector);
    this.moduleName = 'roleEntity';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      { title: '操作', fixed: 'left', width: 65, buttons: [{ text: '修改', icon: 'edit', iif: row => row.Updatable, click: row => this.edit(row) }] },
      { title: '角色', index: 'RoleId', type: 'number', className: 'text-left', format: d => `${d.RoleId}. ${d.RoleName}`, editable: true },
      { title: '数据实体', index: 'EntityId', className: 'text-left', format: d => `${d.EntityName} [${d.EntityType}]`, editable: true },
      {
        title: '操作', index: 'Operation', type: 'number', className: 'text-center', format: d => this.osharp.valueToText(d.Operation, this.osharp.data.dataAuthOperations),
        editable: true, enum: this.toEnum(this.osharp.data.dataAuthOperations)
      },
      { title: '锁定', index: 'IsLocked', type: 'yn', editable: true },
      { title: '注册时间', index: 'CreatedTime', type: 'date' },
    ];
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $RoleId: {
        grid: { span: 24 }, widget: 'select', asyncData: () => this.alain.ReadNode('api/admin/role/readNode', 'RoleName', 'RoleId')
      },
      $EntityId: {
        grid: { span: 24 }, widget: 'select', asyncData: () => this.alain.ReadNode('api/admin/entityinfo/readNode', 'Name', 'Id')
      },
    };
    return ui;
  }
}
