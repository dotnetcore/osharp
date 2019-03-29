import { Component, AfterViewInit, NgZone, ElementRef, EventEmitter, Input, Output, Injector, OnInit } from '@angular/core';
import { KendoGridComponentBase } from "@shared/osharp/services/kendoui.service";
import { AuthConfig } from '@shared/osharp/osharp.model';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'kendoui-function',
  template: `<div id="grid-box-{{moduleName}}"></div>`
})
export class KendouiFunctionComponent extends KendoGridComponentBase implements OnInit, AfterViewInit {

  @Input() ModuleName: string;
  @Input() ReadUrl: string;
  @Input() TypeId: any;
  @Input() Position: string;
  @Output() TypeIdChange: EventEmitter<kendo.ui.Grid> = new EventEmitter<kendo.ui.Grid>();

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.moduleName = this.ModuleName;
    // this.InitBase();
  }

  async ngAfterViewInit() {
    await this.checkAuth();
    if (this.auth.ReadFunctions) {
      super.InitBase();
      super.ViewInitBase();
    }
  }
  ngOnChanges() {
    if (this.grid) {
      this.TypeIdChange.emit(this.grid);
    }
  }

  protected AuthConfig(): AuthConfig {
    return { position: this.Position, funcs: ["ReadFunctions"] };
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
      { field: "AccessType", title: "功能类型", width: 100, template: d => this.osharp.valueToText(d.AccessType, this.osharp.data.accessType) }
    ];
  }
  protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
    let options = super.GetGridOptions(dataSource);
    options.toolbar.unshift({ template: '<span>功能列表</span>' });
    return options;
  }
  protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
    let options = super.GetDataSourceOptions();
    options.transport.read = { url: this.ReadUrl, type: "post", dataType: 'json', contentType: 'application/json;charset=utf-8' };
    options.transport.create = options.transport.update = options.transport.destroy = null;
    options.group = [{ field: "Area" }, { field: "Controller" }];
    options.pageSize = 15;
    options.filter = false;
    return options;
  }
  protected ResizeGrid(init: boolean) {
    let $content = $(this.grid.element).find('.k-grid-content');
    let winHeight = window.innerHeight;
    console.log(winHeight);
    let otherHeight = $($(this.grid.element).find('.k-grid-header')).height() + $(".reuse-tab").height() + $('.alain-default__header').height() + 140;
    console.log(otherHeight);
    $content.height(winHeight - otherHeight);
    console.log($content.height());
  }
}
