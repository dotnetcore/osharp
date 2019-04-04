import { PageRequest, FilterGroup } from './../../../osharp/osharp.model';
import { AlainService } from '@shared/osharp/services/ng-alain.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
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
  @Input() request: PageRequest;
  @Input() searchModal: NzModalComponent;
  @Input() columns: OsharpSTColumn[];
  @Output() submited: EventEmitter<PageRequest> = new EventEmitter<
    PageRequest
  >();

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
    let group = new FilterGroup();
    group.Rules.push(rule);
    this.request.FilterGroup = group;
    if (this.submited) {
      this.submited.emit(this.request);
    }
  }

  adSearch() {
    if (this.searchModal) {
      this.searchModal.open();
    }
  }

  reset() {
    this.rule = new FilterRule(null, null);
    if (this.searchModal) {
      this.searchModal.reset();
    }
  }
}
