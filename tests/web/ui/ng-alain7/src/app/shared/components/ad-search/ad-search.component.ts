import { Component, OnInit, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { NzModalComponent } from 'ng-zorro-antd';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { FilterGroup, FilterOperateEntry, FilterOperate, FilterRule, PageRequest } from '@shared/osharp/osharp.model';
import { List } from 'linqts';
import { OsharpSTColumn } from '@shared/osharp/services/ng-alain.types';
import { AlainService } from '@shared/osharp/services/ng-alain.service';
import { SFSchema } from '@delon/form';
import { STComponent } from '@delon/abc';

@Component({
  selector: 'app-ad-search',
  templateUrl: './ad-search.component.html',
  styles: [`
  .group-box{margin:2px;padding:3px; border:dashed 2px #ddd;}
  .group-box nz-select{margin-right:5px;}
  .group-operate{margin:3px 0;}
  .rule-box{margin:3px 3px;}
  .rule-box nz-select,
    .rule-box nz-input-number,
    .rule-box input[nz-input],
    .rule-box nz-date-picker {width:150px;float:left; margin-right:8px;}
  .f-left{float:left;}
  .k-input{padding:0;line-height:normal;}
  `]
})
export class AdSearchComponent implements OnInit {

  @Input() columns: OsharpSTColumn[];
  @Input() title = '高级搜索';
  @Input() group: FilterGroup;
  @Input() st: STComponent;
  @Output() submited: EventEmitter<FilterGroup> = new EventEmitter<FilterGroup>();
  @ViewChild("modal") modal: NzModalComponent;

  visible: boolean;
  groupOperateEntries: FilterOperateEntry[] = [new FilterOperateEntry(FilterOperate.And), new FilterOperateEntry(FilterOperate.Or)];
  operateEntries: FilterOperateEntry[] = [];
  schemas: SFSchema[] = [];

  constructor(private osharp: OsharpService, private alain: AlainService) { }

  ngOnInit() {
    if (this.group == null) {
      this.group = new FilterGroup();
    }
  }

  open() {
    if (!this.columns || !this.columns.length) {
      this.osharp.warning("列配置信息[columns]不存在，无法进行高级搜索");
      return;
    }
    this.columns = new List(this.columns).Where(m => m.index && m.filterable).ToArray();
    this.modal.open();
  }

  close() {
    this.modal.close();
  }

  reset() {
    this.group.Groups = [];
    this.group.Rules = [];
    if (this.st && this.st.req.body) {
      let request: PageRequest = this.st.req.body as PageRequest;
      if (request) {
        request.FilterGroup = new FilterGroup();
        this.st.reload();
      }
    }
  }

  private copyGroup(g: FilterGroup) {
    let group = new FilterGroup();
    group.Operate = g.Operate;
    for (const item of g.Groups) {
      group.Groups.push(this.copyGroup(item))
    }
    for (const item of g.Rules) {
      group.Rules.push(new FilterRule(item.Field, item.Value, item.Operate));
    }
    return group;
  }

  submit() {
    let group = this.copyGroup(this.group);
    if (this.submited) {
      this.submited.emit(group);
    }
    if (this.st && this.st.req.body) {
      let request: PageRequest = this.st.req.body as PageRequest;
      if (request) {
        request.FilterGroup = group;
        this.st.reload();
      }
    }
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

  private getOperateEntries(operates: FilterOperate[]): FilterOperateEntry[] {
    return new List(operates).Select(m => new FilterOperateEntry(m)).ToArray();
  }

  fieldChange(field: string, rule: FilterRule) {
    let column: OsharpSTColumn = this.columns.find(m => m.index === field);
    if (!column) return;
    let type = column.type || column.ftype || 'string';
    rule.control = 'string';
    switch (type) {
      case 'tag':
      case 'number':
        if (column.enum || column.tag) {
          rule.entries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual]);
          rule.Operate = rule.entries[0].Operate;
          rule.control = 'enum';
          rule.enums = column.enum || this.alain.TagsToEnums(column.tag);
          rule.Value = rule.enums[0].value;
        } else {
          rule.entries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual]);
          rule.Operate = rule.entries[0].Operate;
          rule.Value = 0;
          rule.control = 'number';
        }
        break;
      case 'date':
        rule.entries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual]);
        rule.Operate = rule.entries[0].Operate;
        rule.control = 'date';
        rule.Value = null;
        break;
      case 'yn':
      case 'boolean':
        rule.entries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual]);
        rule.Operate = rule.entries[0].Operate;
        rule.control = 'boolean';
        rule.Value = false;
        break;
      default:
        rule.entries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.StartsWith, FilterOperate.EndsWith, FilterOperate.Contains, FilterOperate.NotContains]);
        rule.Operate = FilterOperate.Contains;
        rule.control = 'string';
        rule.Value = '';
        break;
    }
  }
}
