import { Component, Output, EventEmitter, Input, OnChanges } from '@angular/core';
import { FilterRule, EntityProperty, FilterOperate, FilterOperateEntry } from '@shared/osharp/osharp.model';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'osharp-filter-rule',
  templateUrl: './filter-rule.component.html',
  styles: [`
  .rule-box{margin:3px 3px;}
  .rule-box nz-select,
    .rule-box nz-input-number,
    .rule-box input[nz-input],
    .rule-box nz-date-picker {width:150px;float:left; margin-right:8px;}
  .f-left{float:left;}
  .k-input{padding:0;line-height:normal;}
  `]
})
export class FilterRuleComponent implements OnChanges {

  @Input() rule: FilterRule;
  @Input() properties: EntityProperty[];
  @Output() remove: EventEmitter<FilterRule> = new EventEmitter<FilterRule>();

  operateEntries: FilterOperateEntry[] = [];
  property: EntityProperty;

  constructor(
    private osharp: OsharpService
  ) { }

  ngOnChanges(): void {
    if (this.rule) {
      this.fieldChange(this.rule.Field, true);
    } else {
      this.operateEntries = [];
    }
  }
  removeRule() {
    this.remove.emit(this.rule);
  }

  fieldChange(field: string, first: boolean = false) {
    if (this.properties.length === 0 || !field) {
      return;
    }
    this.property = this.properties.find(m => m.Name === field);
    if (this.property === null) {
      return;
    }
    if (!first) {
      this.rule.Value = null;
    }
    switch (this.property.TypeName) {
      case 'System.Boolean':
        this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual]);
        if (!this.rule.Value) {
          this.rule.Value = 'false';
        }
        break;
      case 'System.Guid':
        this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual]);
        if (!this.rule.Value) {
          this.rule.Value = '';
        }
        break;
      case 'System.Int32':
        if (this.property.ValueRange.length === 0) {
          // 数值类型
          this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual]);
          if (!this.rule.Value) {
            this.rule.Value = '0';
          }
        } else {
          // 枚举类型
          this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual]);
          if (!this.rule.Value) {
            this.rule.Value = this.property.ValueRange[0].id;
          }
        }
        break;
      case 'System.DateTime':
        this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual]);
        if (!this.rule.Value) {
          // this.rule.Value = new Date().toLocaleString();
        }
        break;
      case 'System.String':
        this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.StartsWith, FilterOperate.EndsWith, FilterOperate.Contains, FilterOperate.NotContains]);
        if (!this.rule.Value) {
          this.rule.Value = '';
        }
        break;
      default:
        this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual,
        FilterOperate.StartsWith, FilterOperate.EndsWith, FilterOperate.Contains, FilterOperate.NotContains]);
        if (!this.rule.Value) {
          this.rule.Value = '';
        }
        break;
    }
    if (!this.rule.Operate || this.operateEntries.filter(m => m.Operate === this.rule.Operate).length === 0) {
      this.rule.Operate = this.operateEntries[0].Operate;
    }
  }

  private getOperateEntries(operates: FilterOperate[]): FilterOperateEntry[] {
    let entries: FilterOperateEntry[] = [];
    for (let operate of operates) {
      entries.push(new FilterOperateEntry(operate));
    }
    return entries;
  }

  onTagsChangeEvent(e) {
    if (!e || e.length === 0) {
      this.osharp.error(`${this.property.Display}不能为空`);
      this.rule.Value = null;
      return;
    }
    this.rule.Value = e[0];
  }
}
