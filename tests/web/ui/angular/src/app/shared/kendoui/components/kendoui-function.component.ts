import { Component, OnInit, AfterViewInit, NgZone, ElementRef, Input } from '@angular/core';
declare var $: any;

import { osharp } from "../../osharp";
import { kendoui } from '../../kendoui';

@Component({
  selector: 'kendoui-function',
  template: `<div id="grid-box"></div>`
})
export class KendouiFunctionComponent extends kendoui.GridComponentBase implements OnInit, AfterViewInit {

  @Input() ReadUrl: string;

  constructor(protected zone: NgZone, protected element: ElementRef) {
    super(zone, element);
  }

  ngOnInit(): void {
    super.InitBase();
  }
  ngAfterViewInit(): void {
    super.ViewInitBase();
  }
  ngOnChanges(): void { }

  protected GetModel() {
    return {
      id: "Id",
      fields: {
        Name: { type: "string", editable: false },
        AccessType: { type: "number", editable: false }
      }
    };
  }
  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      { field: "Name", title: "功能名称", width: 300 },
      { field: "AccessType", title: "功能类型", width: 100, template: d => osharp.Tools.valueToText(d.AccessType, osharp.Data.AccessTypes) }
    ];
  }
  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    var options = super.GetGridOptions(dataSource);
    options.toolbar = [{ template: '<span style="line-height:30px;">功能列表</span>' },
    { name: "refresh", template: `<button id="btn-refresh-function" class="k-button k-button-icontext"><i class="k-icon k-i-refresh"></i>刷新</button>` }];
    options.pageable = false;
    return options;
  }
  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    var options = super.GetDataSourceOptions();
    options.transport.read = { url: "/api/admin/module/readfunctions", type: "post" };
    options.transport.create = options.transport.update = options.transport.destroy = null;
    return options;
  }
}
