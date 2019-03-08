import { Component, OnInit, Input, Output, ElementRef, EventEmitter } from '@angular/core';

@Component({
  selector: 'kendoui-window',
  template: `
    <div>
        <div class="win-content">
            <ng-content></ng-content>
        </div>
        <div class="win-footer k-edit-buttons k-state-default">
            <a class="k-button k-button-icontext k-grid-cancel" href="javascript:;" (click)="cancel()">
                <span class="k-icon k-i-cancel"></span>取消
            </a>
            <button type="submit" class="k-button k-button-icontext k-primary k-grid-update" (click)="submit()">
                <span class="k-icon k-i-save"></span>确定
            </button>
        </div>
    </div>
    `
})
export class KendouiWindowComponent implements OnInit {

  @Input() options: kendo.ui.WindowOptions;
  @Output() init: EventEmitter<kendo.ui.Window> = new EventEmitter<kendo.ui.Window>();
  @Output() closing: EventEmitter<kendo.ui.Window> = new EventEmitter<kendo.ui.Window>();
  @Output() submited: EventEmitter<kendo.ui.Window> = new EventEmitter<kendo.ui.Window>();

  $element: any;
  window: kendo.ui.Window;

  constructor(private element: ElementRef) { }

  ngOnInit() {
    this.$element = $($(this.element.nativeElement).find("div").eq(0));
    if (this.options.resize) {
      let innerResize = this.options.resize;
      this.options.resize = e => {
        this.resize(e);
        innerResize(e);
      };
    } else {
      this.options.resize = e => this.resize(e);
    }
    this.window = new kendo.ui.Window(this.$element, this.options);
    this.init.emit(this.window);
  }

  private resize(e) {
    $($(this.$element).find(".win-content")).height(e.height - 70);
  }

  cancel() {
    this.closing.emit(this.window);
    this.window.close();
  }

  submit() {
    this.submited.emit(this.window);
  }
}
