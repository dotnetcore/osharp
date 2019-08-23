import { Component, OnInit, Injector } from '@angular/core';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { SFUISchema } from '@delon/form';

@Component({
  selector: 'app-entityinfo',
  templateUrl: './entityinfo.component.html',
  styles: []
})
export class EntityinfoComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'entityinfo';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      { title: '操作', fixed: 'left', width: 65, buttons: [{ text: '修改', icon: 'edit', iif: row => row.Updatable, click: row => this.edit(row) }] },
      { title: '实体名称', index: 'Name', editable: true, ftype: 'string', filterable: true },
      { title: '实体类型', index: 'TypeName', editable: true, ftype: 'string', filterable: true },
      { title: '数据审计', index: 'AuditEnabled', type: 'yn', editable: true, filterable: true }
    ];
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Name: { grid: { span: 24 }, widget: 'text' },
      $TypeName: { grid: { span: 24 }, widget: 'text' },
      $AuditEnabled: { grid: { span: 24 } },
    };
    return ui;
  }
}
