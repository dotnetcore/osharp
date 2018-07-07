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
    .rule-box nz-select{width:150px;}
    `]
})
export class FilterRuleComponent implements OnChanges {

    @Input() rule: FilterRule;
    @Input() properties: EntityProperty[];
    @Output() remove: EventEmitter<FilterRule> = new EventEmitter<FilterRule>();

    operateEntries: FilterOperateEntry[] = [];

    constructor(
        private osharp: OsharpService
    ) { }

    ngOnChanges(): void {
        if (this.rule) {
            this.fieldChange(this.rule.Field);
        } else {
            this.operateEntries = [];
        }
    }
    removeRule() {
        this.remove.emit(this.rule);
    }

    fieldChange(field) {
        let property = new List(this.properties).First(m => m.Name == field);
        switch (property.TypeName) {
            case 'System.Boolean':
                this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual]);
                break;
            case 'System.Int32':
                this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual]);
                break;
            case 'System.String':
                this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.StartsWith, FilterOperate.EndsWith, FilterOperate.Contains, FilterOperate.NotContains]);
                break;
            default:
                this.operateEntries = this.getOperateEntries([FilterOperate.Equal, FilterOperate.NotEqual, FilterOperate.Less, FilterOperate.LessOrEqual, FilterOperate.Greater, FilterOperate.GreaterOrEqual, FilterOperate.StartsWith, FilterOperate.EndsWith, FilterOperate.Contains, FilterOperate.NotContains]);
                break;
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
