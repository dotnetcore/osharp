import { PageRequest, FilterGroup } from './../../../osharp/osharp.model';
import { AlainService } from '@shared/osharp/services/ng-alain.service';
import { Component, OnInit, Input } from '@angular/core';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { STComponent, STColumn } from '@delon/abc';
import { NzModalComponent } from 'ng-zorro-antd';
import { FilterRule, FilterOperate } from '@shared/osharp/osharp.model';
import { List } from 'linqts';
import { OsharpSTColumn } from '@shared/osharp/services/ng-alain.types';

@Component({
  selector: 'app-ad-search-button',
  templateUrl: './button.component.html',
  styles: [],
})
export class AdSearchButtonComponent implements OnInit {
  @Input() st: STComponent;
  @Input() searchModal: NzModalComponent;
  @Input() columns: OsharpSTColumn[];

  rule: FilterRule;

  constructor(private osharp: OsharpService, private alain: AlainService) {
    this.rule = new FilterRule(null, null);
    this.rule.control = 'string';
  }

  ngOnInit() {
    this.columns = new List(this.columns)
      .Where(m => m.index && m.filterable && m.type !== 'date')
      .ToArray();
  }

  fieldChange(field: string) {
    let column = this.columns.find(m => m.index === field);
    if (!column) {
      this.osharp.warning(`指定属性${field}不存在`);
      return;
    }
    this.alain.changeFilterRuleType(this.rule, column);
  }

  search() {
    if (!this.rule.Field) {
      this.osharp.warning('请选择要查询的列');
      return;
    }
    let rule = new FilterRule(this.rule.Field, this.rule.Value);
    rule.Operate =
      this.rule.control === 'string'
        ? FilterOperate.Contains
        : FilterOperate.Equal;
    if (this.st && this.st.req.body) {
      let request: PageRequest = this.st.req.body as PageRequest;
      if (request) {
        let group = new FilterGroup();
        group.Rules.push(rule);
        request.FilterGroup = group;
        this.st.reload();
      }
    }
  }

  adSearch() {
    if (this.searchModal) {
      this.searchModal.open();
    }
  }
}
[];
