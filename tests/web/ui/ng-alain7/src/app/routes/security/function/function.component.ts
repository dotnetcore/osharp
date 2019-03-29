import { Component, OnInit, Injector } from '@angular/core';
import { STComponentBase } from '@shared/osharp/services/ng-alain.service';
import { SFUISchema } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/ng-alain.types';

@Component({
  selector: 'app-function',
  templateUrl: './function.component.html',
  styles: []
})
export class FunctionComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'function';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      { title: '操作', fixed: 'left', width: 65, buttons: [{ text: '修改', icon: 'edit', iif: row => row.Updatable, click: row => this.edit(row) }] },
      { title: '选择', index: 'Id', type: 'checkbox' },
      { title: '名称', index: 'Name', fixed: 'left', width: 270, sort: true, editable: true, ftype: 'string', readOnly: true },
      {
        title: '功能类型', index: 'AccessType', type: 'number', width: 100, sort: true, format: d => this.osharp.valueToText(d.AccessType, this.osharp.data.accessType),
        editable: true, enum: this.toEnum(this.osharp.data.accessType)
      },
      { title: '操作审计', index: 'AuditOperationEnabled', sort: true, type: "yn", editable: true },
      { title: '数据审计', index: 'AuditEntityEnabled', sort: true, type: "yn", editable: true },
      { title: '缓存秒数', index: 'CacheExpirationSeconds', sort: true, type: "number", editable: true },
      { title: '滑动过期', index: 'IsCacheSliding', sort: true, type: "yn", editable: true },
      { title: '已锁定', index: 'IsLocked', sort: true, type: "yn", editable: true },
      { title: 'Ajax访问', index: 'IsAjax', sort: true, type: "yn" },
      { title: '是否控制器', index: 'IsController', sort: true, type: "yn" },
      { title: '区域', index: 'Area' },
      { title: '控制器', index: 'Controller' },
      { title: '功能方法', index: 'Action' }
    ];
    return columns;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Name: { grid: { span: 24 } },
      $AccessType: { widget: 'select' }
    };
    return ui;
  }
}
