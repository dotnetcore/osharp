import { Component, OnInit, ElementRef, Input } from '@angular/core';


@Component({
  selector: 'kendoui-splitter',
  template: `<div><ng-content></ng-content></div>`
})
export class KendouiSplitterComponent implements OnInit {

  @Input() options: kendo.ui.SplitterOptions;
  @Input() height = 300;
  $element: any;
  splitter: kendo.ui.Splitter;

  constructor(private element: ElementRef) { }

  ngOnInit() {
    this.$element = $($(this.element.nativeElement).find("div").eq(0));
    this.$element.height(this.height);
    this.splitter = new kendo.ui.Splitter(this.$element, this.options);
  }
}
