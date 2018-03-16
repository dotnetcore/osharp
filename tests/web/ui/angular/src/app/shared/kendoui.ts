import { NgZone, ElementRef } from "@angular/core";
import { osharp } from "./osharp";
import { List } from "linqts";
import { element } from "protractor";
declare var $: any;

export namespace kendoui {
    export abstract class GridComponentBase {

        protected moduleName: string = null;
        public gridOptions: kendo.ui.GridOptions = null;
        public grid: kendo.ui.Grid = null;

        constructor(protected zone: NgZone, protected element: ElementRef) { }

        protected InitBase() {
            var dataSourceOptions = this.GetDataSourceOptions();
            var dataSource = new kendo.data.DataSource(dataSourceOptions);
            this.gridOptions = this.GetGridOptions(dataSource);
        }

        protected ViewInitBase() {
            this.zone.runOutsideAngular(() => {
                let $grid = $($(this.element.nativeElement).find("#grid-box"));
                this.grid = new kendo.ui.Grid($grid, this.gridOptions);
                this.ResizeGrid(true);
                window.addEventListener('keydown', e => this.KeyDownEvent(e));
                window.addEventListener('resize', e => this.ResizeGrid(false));
            });
        }

        protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
            var options: kendo.ui.GridOptions = {
                dataSource: dataSource,
                toolbar: [{ name: 'create' }, { name: 'save' }, { name: 'cancel' }],
                columns: this.GetGridColumns(),
                navigatable: true,
                filterable: true,
                resizable: true,
                scrollable: true,
                selectable: false,
                reorderable: true,
                columnMenu: false,
                sortable: { allowUnsort: true, showIndexes: true, mode: 'multiple' },
                pageable: { refresh: true, pageSizes: [10, 15, 20, 50, 'all'], buttonCount: 5 },
                editable: { mode: "incell", confirmation: true },
                saveChanges: e => {
                    if (!confirm('是否提交对表格的更改？')) {
                        e.preventDefault();
                    }
                }
            };
            return options;
        }

        protected GetDataSourceOptions(): kendo.data.DataSourceOptions {
            var options: kendo.data.DataSourceOptions = {
                transport: {
                    read: { url: "/api/admin/" + this.moduleName + "/read", type: 'post' },
                    create: { url: "/api/admin/" + this.moduleName + "/create", type: 'post' },
                    update: { url: "/api/admin/" + this.moduleName + "/update", type: 'post' },
                    destroy: { url: "/api/admin/" + this.moduleName + "/delete", type: 'post' },
                    parameterMap: (opts, operation) => {
                        if (operation == 'read') {
                            return osharp.kendo.Grid.readParameterMap(opts, this.FieldReplace);
                        }
                        if (operation == 'create' || operation == 'update') {
                            return { dtos: opts.models };
                        }
                        if (operation == 'destroy' && opts.models.length) {
                            var ids = new List(opts.models).Select(m => {
                                return m['Id'];
                            }).ToArray();
                            return { ids: ids };
                        }
                        return {};
                    }
                },
                group: [],
                schema: {
                    model: this.GetModel(),
                    data: d => d.Rows,
                    total: d => d.Total
                },
                aggregate: [],
                batch: true,
                pageSize: 24,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                requestEnd: e => osharp.Tools.ajaxResult(e.response, () => this.grid.options.dataSource.read(), null),
                change: function () { }
            };

            return options;
        }

        protected FieldReplace(field: string): string {
            return field;
        }

        /**重写以获取Grid的模型设置Model */
        protected abstract GetModel(): any;
        /**重写以获取Grid的列设置Columns */
        protected abstract GetGridColumns(): kendo.ui.GridColumn[];

        /**重置Grid高度 */
        protected ResizeGrid(init: boolean) {
            var winWidth = window.innerWidth, winHeight = window.innerHeight, diffHeight = winWidth >= 1114 ? 80 : winWidth >= 768 ? 64 : 145;
            var $content = $("#grid-box .k-grid-content");
            var otherHeight = $("#grid-box").height() - $content.height();
            $content.height(winHeight - diffHeight - otherHeight - (init ? 0 : 0));
        }

        private KeyDownEvent(e) {
            if (!this.grid) {
                return;
            }
            var key = e.keyCode;
            if (key === 83 && e.altKey) {
                this.grid.saveChanges();
            } else if (key === 65 && e.altKey) {
                this.grid.dataSource.read();
            }
        }
    }

    export class Controls {
        static Boolean(value: boolean) {
            let html = value ? '<input type="checkbox" checked="checked"/>' : '<input type="checkbox"/>';
            return '<div class="checkbox c-checkbox" style="margin-top:0;margin-bottom:0;margin-left:4px;"><label>' + html + '<span class="fa fa-check" style="width:17px;height:17px;"></span></label></div>';
        }

        static BooleanEditor(container, options) {
            var guid = kendo.guid();
            $('<input class="k-checkbox" type="checkbox" id="' + guid + '" name="' + options.field + '" data-type="boolean" data-bind="checked:' + options.field + '">').appendTo(container);
            $('<label class="k-checkbox-label" for="' + guid + '"></label>').appendTo(container);
        }

        static NumberEditor(container, options, decimals, step?) {
            var input = $('<input/>');
            input.attr('name', options.field);
            input.appendTo(container);
            input.kendoNumericTextBox({
                format: '{0:' + decimals + '}',
                step: step || 0.01
            });
        }

        static DropDownList(element, dataSource, textField = 'text', valueField = 'id') {
            element.kendoDropDownList({
                autoBind: true,
                dataTextField: textField || "text",
                dataValueField: valueField || "id",
                dataSource: dataSource
            });
        }

        static DropDownListEditor(container, options, dataSource, textField = 'text', valueField = 'id') {
            var input = $('<input/>');
            input.attr('name', options.field);
            input.appendTo(container);
            new kendo.ui.DropDownList(input, {
                autoBind: true,
                dataTextField: textField,
                dataValueField: valueField,
                dataSource: dataSource
            });
        }
    }
}