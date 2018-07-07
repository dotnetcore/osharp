import { Component, OnInit, Output, EventEmitter, Input, OnChanges } from '@angular/core';
import { FilterRule, EntityProperty, FilterOperate, FilterOperateEntry } from '@shared/osharp/osharp.model';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { List } from 'linqts';

@Component({
    selector: 'security-filter-rule',
    templateUrl: './filter-rule.component.html',
    styles: [`
    nz-select{margin-right:8px;}
    .rule-box{margin-left:10px;margin-bottom:3px;}
    .rule-box nz-select,.rule-box nz-input-number{width:150px;float:left;}
    .f-left{float:left;}
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
        if (this.properties.length == 0 || !field) {
            return;
        }
        this.property = new List(this.properties).First(m => m.Name == field);
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
                this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual]);
                if (!this.rule.Value) {
                    this.rule.Value = '0';
                }
                break;
            case 'System.DateTime':
                this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual]);
                if (!this.rule.Value) {
                    // this.rule.Value = new Date().toLocaleString();
                    console.log(this.rule.Value);
                }
                break;
            case 'System.String':
                this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.StartsWith, FilterOperate.EndsWith, FilterOperate.Contains, FilterOperate.NotContains]);
                if (!this.rule.Value) {
                    this.rule.Value = '';
                }
                break;
            default:
                this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual, FilterOperate.StartsWith, FilterOperate.EndsWith, FilterOperate.Contains, FilterOperate.NotContains]);
                if (!this.rule.Value) {
                    this.rule.Value = '';
                }
                break;
        }
        if (!this.rule.Operate || !new List(this.operateEntries).Any(m => m.Operate == this.rule.Operate)) {
            this.rule.Operate = this.operateEntries[0].Operate;
        }
    }

    private getOperateEntries(operates: FilterOperate[]): FilterOperateEntry[] {
        let entries: FilterOperateEntry[] = [];
        for (let index = 0; index < operates.length; index++) {
            const operate = operates[index];
            entries.push(new FilterOperateEntry(operate));
        }
        return entries;
    }
}
