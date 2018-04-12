import { Component, Input, OnInit, NgZone, ElementRef } from '@angular/core';
declare var $: any;
import { kendoui } from '../../../shared/kendoui';
import { osharp } from '../../../shared/osharp';

@Component({
  selector: 'admin-module-function',
  template: `<div id="grid-box"></div>`
})
export class ModuleFunctionComponent extends kendoui.GridComponentBase implements OnInit {

  @Input() ModuleId: number;

  constructor(protected zone: NgZone, protected element: ElementRef) {
    super(zone, element);
  }

  ngOnInit() {
    super.InitBase();
  }
  ngAfterViewInit(): void {
    super.ViewInitBase();
  }
  ngOnChanges() {
    this.GetFunctions();
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
      { field: "Name", title: "功能名称", width: 300 },
      { field: "AccessType", title: "功能类型", width: 100, template: d => osharp.Tools.valueToText(d.AccessType, osharp.Data.AccessTypes) }
    ];
  }
  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    var options = super.GetGridOptions(dataSource);
    options.toolbar = [{ template: '<span style="line-height:30px;">模块功能列表</span>' },
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
  protected ResizeGrid(init: boolean) {
    var winWidth = window.innerWidth, winHeight = window.innerHeight, diffHeight = winWidth >= 1114 ? 80 : winWidth >= 768 ? 64 : 145;
    var $content = $("#grid-box .k-grid-content");
    var otherHeight = $("#grid-box").height() - $content.height() + 50;
    $content.height(winHeight - diffHeight - otherHeight - (init ? 0 : 0));
  }
  protected ToolbarInit() {
    let $toolbar = $(this.grid.element).find(".k-grid-toolbar");
    if (!$toolbar) {
      return;
    }
    $($toolbar).on("click", "#btn-refresh-function", e => this.grid.dataSource.read());
  }
  private GetFunctions() {
    if (!this.grid) {
      return;
    }
    var options = {
      filter: {
        logic: "and",
        filters: [
          { field: "TreePathString", operator: "contains", value: "$" + this.ModuleId + "$" }
        ]
      }
    };
    options = kendoui.Tools.queryOptions(this.grid.dataSource, options);
    this.grid.dataSource.query(options);
  }
}
