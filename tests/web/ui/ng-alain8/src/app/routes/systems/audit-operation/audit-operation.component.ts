import { Component, OnInit, Injector, ViewChildren, QueryList } from '@angular/core';
import { STComponentBase, } from '@shared/osharp/components/st-component-base';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STColumn, STData, STChange, STComponent } from '@delon/abc';
import { PageRequest, FilterRule, PageData } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-audit-operation',
  templateUrl: './audit-operation.component.html'
})
export class AuditOperationComponent extends STComponentBase implements OnInit {

  keys: string[] = [];
  @ViewChildren(STComponent) sts: QueryList<STComponent>;

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'auditOperation';
  }

  ngOnInit() {
    super.InitBase();
    this.entityInit();
    this.propertyInit();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    return [
      { title: '功能', index: 'FunctionName', filterable: true },
      { title: '用户名', index: 'UserName', className: 'max-200', filterable: true },
      { title: '昵称', index: 'NickName', filterable: true },
      { title: 'IP地址', index: 'Ip', filterable: true },
      { title: '操作系统', index: 'OperationSystem', filterable: true },
      { title: '浏览器', index: 'Browser', filterable: true },
      { title: '结果', index: 'ResultType', type: 'tag', tag: this.alain.AjaxResultTypeTags, filterable: true },
      { title: '执行时间', index: 'CreatedTime', type: 'date', filterable: true },
      { title: '耗时(ms)', index: 'Elapsed', type: 'number', filterable: true },
      { title: '消息', index: 'Message', className: 'max-300', filterable: true },
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
      this.getEntityAudits(data.Id);
    }
  }

  private getEntityAudits(id: string) {
    let request = new PageRequest();
    request.FilterGroup.Rules.push(new FilterRule('OperationId', id));
    this.http.post('api/admin/auditEntity/read', request).subscribe((res: PageData<any>) => {
      for (const item of res.Rows) {
        item.OperationId = id;
      }
      let index = this.keys.indexOf(id);
      let entityST: STComponent = this.sts.toArray()[index * 2 + 1];
      entityST.data = res.Rows;
      entityST.reload();
      // 显示属性变更
      if (res.Rows.length > 0) {
        this.showProps(res.Rows[0]);
      }
    });
  }

  // #region 数据审计

  entityColumns: STColumn[];

  private entityInit() {
    this.entityColumns = [
      { title: '实体名称', index: 'Name' },
      { title: '实体类型', index: 'TypeName' },
      { title: '数据编号', index: 'EntityKey' },
      { title: '操作', index: 'OperateType', type: 'tag', tag: this.alain.OperateTypeTags },
    ];
  }

  entityChange(value: STChange) {
    if (value.type === 'click' && value.click && value.click.item) {
      let data = value.click.item;
      this.showProps(data);
    }
  }

  private showProps(data: STData) {
    let index = this.keys.indexOf(data.OperationId);
    let propertyST: STComponent = this.sts.toArray()[index * 2 + 2];
    propertyST.data = data.Properties;
    propertyST.reload();
  }

  // #endregion

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
