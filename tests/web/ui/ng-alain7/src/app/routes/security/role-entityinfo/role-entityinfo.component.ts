import { Component, OnInit, Injector } from '@angular/core';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { OsharpSTColumn } from '@shared/osharp/services/ng-alain.types';
import { SFUISchema } from '@delon/form';
import { AjaxResult } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-role-entityinfo',
  templateUrl: './role-entityinfo.component.html',
  styles: []
})
export class RoleEntityinfoComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'roleEntity';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] =
      [
        { title: '操作', fixed: 'left', width: 65, buttons: [{ text: '修改', icon: 'edit', iif: row => row.Updatable, click: row => this.edit(row) }] },
        { title: '角色', index: 'RoleId', type: 'number', className: 'text-left', format: d => `${d.RoleId}. ${d.RoleName}`, editable: true },
        { title: '数据实体', index: 'EntityId', className: 'text-left', format: d => `${d.EntityName} [${d.EntityType}]`, editable: true },
        {
          title: '操作', index: 'Operation', type: 'tag', tag: this.alain.DataAuthOperationTags, className: 'text-center',
          editable: true, ftype: 'number', enum: this.toEnum(this.osharp.data.dataAuthOperations)
        },
        { title: '锁定', index: 'IsLocked', type: 'yn', editable: true },
        { title: '注册时间', index: 'CreatedTime', type: 'date' },
      ];
    return columns;
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

  showGroupJson(item: any) {
    item.groupJson = JSON.stringify(item.FilterGroup, null, 2);
  }
  saveFilterGroup(item: any) {
    let dto = { Id: item.Id, RoleId: item.RoleId, EntityId: item.EntityId, Operation: item.Operation, FilterGroup: item.FilterGroup, IsLocked: item.IsLocked };
    this.http.post<AjaxResult>('api/admin/roleEntity/update', [dto]).subscribe(result => {
      this.osharp.ajaxResult(result);
    });
  }
}
