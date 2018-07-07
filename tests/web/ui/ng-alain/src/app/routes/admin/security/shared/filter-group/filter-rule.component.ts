import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Rule, EntityProperty } from '@shared/osharp/osharp.model';

@Component({
    selector: 'security-filter-rule',
    templateUrl: './filter-rule.component.html',
    styles: [`
    nz-select{margin-right:8px;}
    .rule-box{margin-left:10px;margin-bottom:3px;}
    `]
})
export class FilterRuleComponent implements OnInit {

    selectField = 1;
    selectOperate = 1;
    @Input() rule: Rule;
    @Input() properties: EntityProperty[];
    @Output() remove: EventEmitter<Rule> = new EventEmitter<Rule>();

    constructor() { }

    ngOnInit() {
    }

    removeRule() {
        this.remove.emit(this.rule);
    }
}
