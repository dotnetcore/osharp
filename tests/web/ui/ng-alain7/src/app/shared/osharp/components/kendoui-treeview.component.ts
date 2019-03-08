import { Component, OnInit, Input, ElementRef, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'kendoui-treeview',
  template: `<div><ng-content></ng-content></div>`
})
export class KendouiTreeviewComponent implements OnInit {

  @Input() options: kendo.ui.TreeViewOptions;
  @Output() init: EventEmitter<kendo.ui.TreeView> = new EventEmitter<kendo.ui.TreeView>();

  $element: any;
  treeview: kendo.ui.TreeView;

  constructor(private element: ElementRef) { }

  ngOnInit() {
    this.$element = $($(this.element.nativeElement).find("div").eq(0));
    this.treeview = new kendo.ui.TreeView(this.$element, this.options);
    this.init.emit(this.treeview);
  }
}
