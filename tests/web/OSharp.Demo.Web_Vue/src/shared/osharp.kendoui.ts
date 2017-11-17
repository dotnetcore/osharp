import Vue from 'vue'
import { List } from "linqts"
import $ from 'jquery'
import { osharp } from "./osharp"

export namespace kendoui {

    export abstract class GridVueBase extends Vue {

        protected moduleName: string = null;
        public gridOptions: kendo.ui.GridOptions = null;
        public $grid: kendo.ui.Grid = null;

        constructor(private dataSourceOptions: DataSourceOptions) {
            super();
        }

        GetGridOptions() {
            var dataSourceOptions = this.GetDataSourceOptionsInternal(this.dataSourceOptions);
            var dataSource = new kendo.data.DataSource(dataSourceOptions);
            this.$emit('DataSourceCreated', dataSource);
            this.gridOptions = this.GetGridOptionsInternal(dataSource);

            document.addEventListener('keydown', e => this.KeyDownEvent(e));
        }

        protected GetDataSourceOptionsInternal(options?: DataSourceOptions): kendo.data.DataSourceOptions {
            if (!options.fieldReplace) {
                options.fieldReplace = field => field;
            }
            if (!options.url) {
                options.url = {
                    read: "/api/admin/" + this.moduleName + "/read",
                    create: "/api/admin/" + this.moduleName + "/create",
                    update: "/api/admin/" + this.moduleName + "/update",
                    destroy: "/api/admin/" + this.moduleName + "/delete"
                }
            }
            var dataSourceOptions: kendo.data.DataSourceOptions = {
                transport: {
                    read: { url: options.url.read, type: 'post' },
                    create: { url: options.url.create, type: 'post' },
                    update: { url: options.url.update, type: 'post' },
                    destroy: { url: options.url.destroy, type: 'post' },
                    parameterMap: (opts, operation) => {
                        if (operation == 'read') {
                            return osharp.kendo.Grid.readParameterMap(opts, options.fieldReplace);
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
                group: options.group || [],
                schema: {
                    model: options.model,
                    data: d => d.Rows,
                    total: d => d.Total
                },
                aggregate: options.aggregate || [],
                batch: options.batch != null ? options.batch : true,
                pageSize: options.pageSize || 20,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                requestEnd: e => this.RequestEnd(e),
                change: options.change || function () { }
            }
            return dataSourceOptions;
        }
        protected GetGridOptionsInternal(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
            var gridOptions: kendo.ui.GridOptions = {
                dataSource: dataSource,
                toolbar: ['create', 'save', 'cancel'],
                columns: this.GetGridColumns(),
                height: 300,
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
            return gridOptions;
        }

        /**重写以获取Grid的列设置Columns */
        protected abstract GetGridColumns(): kendo.ui.GridColumn[];

        private RequestEnd(e) {
            if (!e.response) {
                return;
            }
            var data = e.response;
            if (!data.Type) {
                return;
            }
            if (data.Type == 'Error') {
                osharp.Tip.error(data.Content);
                return;
            }
            if (data.Type == 'Info') {
                osharp.Tip.info(data.Content);
            }
            if (data.Type == 'Success') {
                osharp.Tip.success(data.Content);
            }
            if (this.$grid && this.$grid.options) {
                this.$grid.options.dataSource.read();
            }
        }
        private KeyDownEvent(e) {
            if (!this.$grid) {
                return;
            }
            var key = e.keyCode;
            if (key === 83 && e.altKey) {
                this.$grid.saveChanges();
            } else if (key === 65 && e.altKey) {
                this.$grid.dataSource.read();
            }
        }


        model: {
            event: "DataSourceCreated"
        }
    }

    export interface DataSourceOptions {
        url?: GridUrl;
        fieldReplace?(field: string): string;
        group?: kendo.data.DataSourceGroupItem[];
        model: any;
        aggregate?: kendo.data.DataSourceAggregateItem[];
        batch?: boolean;
        pageSize?: number;
        change?(e: kendo.data.DataSourceChangeEvent): void;
        requestEnd?(e: kendo.data.DataSourceRequestEndEvent): void;
    }

    interface GridUrl {
        read: string;
        create?: string;
        update?: string;
        destroy?: string;
    }

    export class Controls {
        static Boolean(value: boolean) {
            return value ? '是' : '否';
        }

        static BooleanEditor(container, options) {
            var guid = kendo.guid();
            $('<input class="k-checkbox" type="checkbox" id="' + guid + '" name="' + options.field + '" data-type="boolean" data-bind="checked:' + options.field + '">').appendTo(container);
            $('<label class="k-checkbox-label" for="' + guid + '"></label>').appendTo(container);
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
            input.kendoDropDownList({
                autoBind: true,
                dataTextField: textField,
                dataValueField: valueField,
                dataSource: dataSource
            });
        }
    }
}