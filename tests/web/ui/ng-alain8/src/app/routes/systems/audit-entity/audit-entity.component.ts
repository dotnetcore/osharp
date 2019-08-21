import { Component, OnInit, Injector, ViewChildren, QueryList } from '@angular/core';
import { STComponentBase, } from '@shared/osharp/components/st-component-base';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STColumn, STChange, STComponent, STData } from '@delon/abc';

@Component({
  selector: 'app-audit-entity',
  templateUrl: './audit-entity.component.html',
  styles: []
})
export class AuditEntityComponent extends STComponentBase implements OnInit {

  keys: string[] = [];
  @ViewChildren(STComponent) sts: QueryList<STComponent>;

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'auditEntity';
  }

  ngOnInit() {
    super.InitBase();
    this.propertyInit();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      { title: '实体名称', index: 'Name', filterable: true },
      { title: '实体类型', index: 'TypeName', filterable: true },
      { title: '数据编号', index: 'EntityKey', filterable: true },
      { title: '操作', index: 'OperateType', type: 'tag', tag: this.alain.OperateTypeTags, filterable: true },
    ];
  }

  protected ResponseDataProcess(data: STData[]): STData[] {
    this.keys = [];
    for (const item of data) {
      this.keys.push(item.Id);
    }
    return data;
  }

  change(value: STChange) {
    if (value.type === 'expand' && value.expand && value.expand.expand) {
      // 操作审计行展开时，加载数据审计信息
      let data = value.expand;
      let index = this.keys.indexOf(data.Id);
      let propertyST: STComponent = this.sts.toArray()[index + 1];
      propertyST.data = data.Properties;
      propertyST.reload();
    }
  }

  // #region  数据审计明细

  propertyColumns: STColumn[];

  private propertyInit() {
    this.propertyColumns = [
      { title: '属性名称', index: 'DisplayName' },
      { title: '实体属性', index: 'FieldName' },
      { title: '原始值', index: 'OriginalValue' },
      { title: '变更值', index: 'NewValue' },
      { title: '数据类型', index: 'DataType' },
    ];
  }

  // #endregion
}
