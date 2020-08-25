import { Component, OnInit, Input, ViewChild, Output, EventEmitter, } from '@angular/core';
import { NzModalComponent } from 'ng-zorro-antd';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { FilterGroup, FilterOperateEntry, FilterOperate, FilterRule, PageRequest, } from '@shared/osharp/osharp.model';
import { List } from 'linqts';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { AlainService } from '@shared/osharp/services/alain.service';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'osharp-ad-search-modal',
  templateUrl: './modal.component.html',
  styles: [
    `
      .group-box {
        margin: 2px;
        padding: 3px;
        border: dashed 2px #ddd;
      }
      .group-box nz-select {
        margin-right: 5px;
      }
      .group-operate {
        margin: 3px 0;
      }
      .rule-box {
        margin: 3px 3px;
      }
      .rule-box nz-select,
      .rule-box nz-input-number,
      .rule-box input[nz-input],
      .rule-box nz-date-picker {
        width: 150px;
        float: left;
        margin-right: 8px;
      }
      .f-left {
        float: left;
      }
      .k-input {
        padding: 0;
        line-height: normal;
      }
    `,
  ],
})
export class AdSearchModalComponent implements OnInit {
  @Input() request: PageRequest;
  @Input() columns: OsharpSTColumn[];
  @Input() title = '高级搜索';
  @Output() submited: EventEmitter<PageRequest> = new EventEmitter<
    PageRequest
  >();
  @ViewChild('modal', { static: false }) modal: NzModalComponent;

  visible: boolean;
  group: FilterGroup;
  groupOperateEntries: FilterOperateEntry[] = [
    new FilterOperateEntry(FilterOperate.And),
    new FilterOperateEntry(FilterOperate.Or),
  ];
  operateEntries: FilterOperateEntry[] = [];
  schemas: SFSchema[] = [];

  constructor(private osharp: OsharpService, private alain: AlainService) { }

  ngOnInit() {
    this.group = this.request.FilterGroup;
  }

  open() {
    if (!this.columns || !this.columns.length) {
      this.osharp.warning('列配置信息[columns]不存在，无法进行高级搜索');
      return;
    }
    this.columns = new List(this.columns)
      .Where(m => m.index && m.filterable)
      .ToArray();
    this.modal.open();
  }

  close() {
    this.modal.close();
  }

  reset() {
    this.group.Groups = [];
    this.group.Rules = [];
    this.request.FilterGroup = new FilterGroup();
    this.submited.emit(this.request);
  }

  private copyGroup(g: FilterGroup) {
    let group = new FilterGroup();
    group.Operate = g.Operate;
    for (const item of g.Groups) {
      group.Groups.push(this.copyGroup(item));
    }
    for (const item of g.Rules) {
      let field: string = item.Field;
      let column = this.columns.find(m => m.index === field);
      field = column.filterIndex || field;
      group.Rules.push(new FilterRule(field, item.Value, item.Operate));
    }
    return group;
  }

  submit() {
    let group = this.copyGroup(this.group);
    this.request.FilterGroup = group;
    this.submited.emit(this.request);
    this.close();
  }

  addGroup(group: FilterGroup) {
    let subGroup = new FilterGroup();
    subGroup.Level = group.Level + 1;
    group.Groups.push(subGroup);
  }
  removeGroup(group: FilterGroup, parentGroup: FilterGroup) {
    if (parentGroup) {
      let index = parentGroup.Groups.indexOf(group);
      if (index >= 0) {
        parentGroup.Groups.splice(index, 1);
      }
    }
  }

  addRule(group: FilterGroup) {
    let rule = new FilterRule(null, null);
    group.Rules.push(rule);
  }

  removeRule(rule: FilterRule, group: FilterGroup) {
    if (rule) {
      let index = group.Rules.indexOf(rule);
      if (index >= 0) {
        group.Rules.splice(index, 1);
      }
    }
  }

  fieldChange(field: string, rule: FilterRule) {
    let column: OsharpSTColumn = this.columns.find(m => m.index === field);
    if (!column) return;
    this.alain.changeFilterRuleType(rule, column);
  }
}
