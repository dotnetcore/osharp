import { Component, OnInit, Injector } from '@angular/core';
import { STComponentBase } from '@shared/osharp/services/ng-alain.service';
import { STColumn } from '@delon/abc';

@Component({
  selector: 'app-function',
  templateUrl: './function.component.html',
  styles: []
})
export class FunctionComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.url = 'api/admin/function/read';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): STColumn[] {
    return [
      {
        title: '操作', fixed: 'left', width: 60, buttons: [{
          text: '操作', children: [{
            text: '修改', icon: 'edit', click: (row) => this.osharp.info(row.Name)
          }]
        }]
      },
      { title: '选择', index: 'Id', type: 'checkbox' },
      { title: '名称', index: 'Name', fixed: 'left', width: 270, sort: true },
      { title: '功能类型', index: 'AccessType', sort: true },
      { title: '操作审计', index: 'AuditOperationEnabled', sort: true, type: "yn" },
      { title: '数据审计', index: 'AuditEntityEnabled', sort: true, type: "yn" },
      { title: '缓存秒数', index: 'CacheExpirationSeconds', sort: true, type: "number" },
      { title: '滑动过期', index: 'IsCacheSliding', sort: true, type: "yn" },
      { title: '已锁定', index: 'IsLocked', sort: true, type: "yn" },
      { title: 'Ajax访问', index: 'IsAjax', sort: true, type: "yn" },
      { title: '是否控制器', index: 'IsController', sort: true, type: "yn" },
      { title: '区域', index: 'Area' },
      { title: '控制器', index: 'Controller' },
      { title: '功能方法', index: 'Action' }
    ];
  }

}
