import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Component, Input, OnInit, forwardRef, ElementRef } from '@angular/core';
import { DataSource } from '@angular/cdk/table';
import { Observable } from 'rxjs';

export const KendouiComboBox_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => KendouiComboBoxComponent),
  multi: true
};

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'kendoui-combo',
  template: `<input/>`,
  providers: [KendouiComboBox_VALUE_ACCESSOR]
})
export class KendouiComboBoxComponent implements OnInit, ControlValueAccessor {

  private _value: any;
  private $element: any;
  private combobox: kendo.ui.ComboBox;

  @Input() dataSource: Observable<Object>[] | Object[];
  @Input() textField: string = 'text';
  @Input() valueField: string = 'id';

  set value(value: any) {
    this._value = value;
    this.notifyValueChange();
  }

  get value() {
    return this._value;
  }

  onChange: (value) => {};
  onTouched: () => {};

  constructor(private element: ElementRef) { }

  ngOnInit(): void {
    this.$element = $($(this.element.nativeElement).find("input").eq(0));
    let options: kendo.ui.ComboBoxOptions = {
      dataSource: this.dataSource,
      dataTextField: this.textField,
      dataValueField: this.valueField,
      select: e => {
        if (!e.dataItem) {
          return;
        }
        this.value = e.dataItem.id;
      },
      change: e => this.value = this.combobox.value()
    };
    this.combobox = new kendo.ui.ComboBox(this.$element, options);
  }

  notifyValueChange() {
    if (this.onChange) {
      this.onChange(this.value);
    }
  }

  writeValue(obj: any): void {
    this.value = obj;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
  }
}
