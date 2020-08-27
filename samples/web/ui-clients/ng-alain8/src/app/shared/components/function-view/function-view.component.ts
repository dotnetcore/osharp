import { Component, OnInit, Injector, ViewChild, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { STComponent } from '@delon/abc';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { FilterRule, FilterOperate, FilterGroup } from '@shared/osharp/osharp.model';

@Component({
  selector: 'osharp-function-view',
  template: `
  <div>
    <button nz-button (click)="st.reload()"><i nz-icon nzType="reload" theme="outline"></i>刷新</button>
  </div>
  <st #st [data]="ReadUrl" [columns]="columns" size="small" [req]="req" [res]="res" [(pi)]="request.PageCondition.PageIndex" [(ps)]="request.PageCondition.PageSize" [page]="page"></st>
  `,
  styles: []
})
export class FunctionViewComponent extends STComponentBase implements OnInit {

  @Input() ReadUrl: string;

  @ViewChild('st', { static: false }) st: STComponent;

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      { title: '功能名称', index: 'Name' },
      { title: '功能类型', index: 'AccessType', type: 'tag', tag: this.alain.AccessTypeTags, width: 100 },
      { title: '区域', index: 'Area', width: 80 },
      { title: '控制器', index: 'Controller', width: 100 },
    ];
  }

  reload(filter: FilterGroup) {
    this.st.pi = 1;
    this.request.FilterGroup = filter;
    this.st.req.body = this.request;
    this.st.reload();
  }
}
