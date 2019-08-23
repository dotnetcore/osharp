import { Component, OnInit, Injector } from '@angular/core';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';

@Component({
  selector: 'app-pack',
  templateUrl: './pack.component.html',
  styles: []
})
export class PackComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'pack';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      { title: '', index: '', type: 'no' },
      { title: '名称', index: 'Display' },
      { title: '类型', index: 'Class' },
      { title: '级别', index: 'Level', type: 'tag', tag: this.alain.PackLevelTags, filterable: true },
      { title: '启动顺序', index: 'Order', type: 'number', filterable: true },
      { title: '是否启用', index: 'IsEnabled', type: "yn", filterable: true },
    ];
  }
}
