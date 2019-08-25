import { AlainService } from '@shared/osharp/services/alain.service';
import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { NzModalComponent } from 'ng-zorro-antd';
import { FilterRule, FilterOperate, PageRequest, FilterGroup } from '@shared/osharp/osharp.model';
import { List } from 'linqts';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';

@Component({
  selector: 'osharp-ad-search',
  templateUrl: './ad-search.component.html',
  styles: [],
})
export class AdSearchComponent implements OnInit {
  @Input() request: PageRequest;
  @Input() columns: OsharpSTColumn[];
  @Output() submited: EventEmitter<PageRequest> = new EventEmitter<PageRequest>();
  @ViewChild('searchModal', { static: false }) searchModal: NzModalComponent;

  rule: FilterRule;

  constructor(private osharp: OsharpService, private alain: AlainService) {
    this.rule = new FilterRule(null, null);
    this.rule.control = 'string';
  }

  ngOnInit() {
    this.columns = new List(this.columns)
      .Where(m => m.index && m.filterable && m.type !== 'date') // TODO: date 的UI未调好，暂不过滤
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
    let field = this.rule.Field;
    if (!field) {
      this.osharp.warning('请选择要查询的列');
      return;
    }
    let column = this.columns.find(m => m.index === field);
    if (!column) {
      this.osharp.warning(`指定属性${field}不存在`);
      return;
    }
    field = column.filterIndex || field;

    let rule = new FilterRule(field, this.rule.Value);
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

  adSearchSubmited(request: PageRequest) {
    if (this.submited) {
      this.submited.emit(request);
    }
  }

  reset() {
    this.rule = new FilterRule(null, null);
    if (this.searchModal) {
      this.searchModal.reset();
    }
  }
}
