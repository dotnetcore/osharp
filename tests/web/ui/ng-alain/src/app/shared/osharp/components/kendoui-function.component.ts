import { Component, OnInit, AfterViewInit, NgZone, ElementRef, EventEmitter, Input, Output, Injector } from '@angular/core';
import { GridComponentBase } from "../services/kendoui.service";

@Component({
  selector: 'kendoui-function',
  template: `<div id="grid-box"></div>`
})
export class KendouiFunctionComponent extends GridComponentBase implements OnInit, AfterViewInit {

  @Input() ReadUrl: string;
  @Input() TypeId: any;
  @Output() TypeIdChange: EventEmitter<kendo.ui.Grid> = new EventEmitter<kendo.ui.Grid>();

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    super.InitBase();
  }
  ngAfterViewInit(): void {
    super.ViewInitBase();
  }
  ngOnChanges(): void {
    if (this.grid) {
      this.TypeIdChange.emit(this.grid);
    }
  }

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
      { field: "Name", title: "功能名称", width: 300, filterable: false, sortable: false },
      { field: "AccessType", title: "功能类型", width: 100, filterable: false, sortable: false, template: d => this.osharp.valueToText(d.AccessType, this.osharp.data.accessType) }
    ];
  }
  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.toolbar = [{ template: '<span style="line-height:30px;">功能列表</span>' },
    { name: "refresh", template: `<button id="btn-refresh-function" class="k-button k-button-icontext"><i class="k-icon k-i-refresh"></i>刷新</button>` }];
    // options.pageable = false;
    return options;
  }
  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    let options = super.GetDataSourceOptions();
    options.transport.read = { url: this.ReadUrl, type: "post" };
    options.transport.create = options.transport.update = options.transport.destroy = null;
    options.group = [{ field: "Area" }, { field: "Controller" }];
    options.pageSize = 15;
    options.filter = false;
    return options;
  }
  protected ToolbarInit() {
    let $toolbar = $(this.grid.element).find(".k-grid-toolbar");
    if (!$toolbar) {
      return;
    }
    $($toolbar).on("click", "#btn-refresh-function", e => this.grid.dataSource.read());
  }
  protected ResizeGrid(init: boolean) {
    let $content = $("kendoui-function #grid-box .k-grid-content");
    $content.height(740);
  }
}
