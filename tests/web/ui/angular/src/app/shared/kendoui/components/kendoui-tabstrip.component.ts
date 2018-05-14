import { Component, OnInit, ElementRef, Input } from '@angular/core';


@Component({
    selector: 'kendoui-tabstrip',
    template: `<div><ng-content></ng-content></div>`
})
export class KendouiTabstripComponent implements OnInit {

    @Input() options: kendo.ui.TabStripOptions;

    $element: any;
    tabstrip: kendo.ui.TabStrip;

    constructor(private element: ElementRef) { }

    ngOnInit() {
        this.$element = $($(this.element.nativeElement).find("div").eq(0));
        this.tabstrip = new kendo.ui.TabStrip(this.$element, this.options);
    }
}