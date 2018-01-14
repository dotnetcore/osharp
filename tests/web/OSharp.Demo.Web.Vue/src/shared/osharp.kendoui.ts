import Vue from 'vue'
import { List } from "linqts"
import $ from 'jquery'
import { osharp } from "./osharp"
import Bus from './eventBus'

export namespace kendoui {

    export abstract class GridVueBase extends Vue {

        protected moduleName: string = null;
        public gridOptions: kendo.ui.GridOptions = null;
        public $grid: kendo.ui.Grid = null;

        constructor() {
            super();
        }

        protected CreatedBase() {
            var dataSourceOptions = this.GetDataSourceOptions();
            var dataSource = new kendo.data.DataSource(dataSourceOptions);
            this.$emit("DataSourceCreated", dataSource);
            this.gridOptions = this.GetGridOptions(dataSource);
        }
        protected MountedBase() {
            this.$grid = $((<any>this.$refs.grid).$el).data("kendoGrid");
            this.ResizeGrid(true);
            window.addEventListener('keydown', e => this.KeyDownEvent(e));
            window.addEventListener('resize', e => this.ResizeGrid(false));
        }

        protected GetGridOptions(dataSource: kendo.data.DataSource): kendo.ui.GridOptions {
            var options: kendo.ui.GridOptions = {
                dataSource: dataSource,
                toolbar: ['create', 'save', 'cancel'],
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
                pageSize: 20,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                requestEnd: e => osharp.Tools.ajaxResult(e.response, () => this.$grid.options.dataSource.read(), null),
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

        protected ResizeGrid(init: boolean) {
            var winWidth = window.innerWidth, winHeight = window.innerHeight, diffHeight = winWidth < 480 ? 57 : 65;;
            var $content = $("#grid-box .k-grid-content");
            var otherHeight = $("#grid-box").height() - $content.height();
            $content.height(winHeight - diffHeight - otherHeight - (init ? 0 : 20));
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

    export abstract class TreeListVueBase extends Vue {

        protected moduleName: string = null;
        public treelistOptions: kendo.ui.TreeListOptions = null;
        public $treelist: kendo.ui.TreeList = null;

        constructor() {
            super();
        }

        protected CreatedBase() {
            var dataSourceOptions = this.GetDataSourceOptions();
            var dataSource = new kendo.data.TreeListDataSource(dataSourceOptions);
            this.$emit("DataSourceCreated", dataSource);
            this.treelistOptions = this.GetTreeListOptions(dataSource);
        }
        protected MountedBase() {
            this.$treelist = $((<any>this.$refs.treelist).$el).data("kendoTreeList");
        }

        protected GetTreeListOptions(dataSource: kendo.data.TreeListDataSource): kendo.ui.TreeListOptions {
            var options: kendo.ui.TreeListOptions = {
                dataSource: dataSource,
                columns: this.GetTreeListColumns(),
                selectable: true,
                resizable: true,
                editable: { move: true }
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
                },
                schema: {
                    model: this.GetModel()
                },
                requestEnd: e => osharp.Tools.ajaxResult(e.response)
            };

            return options;
        }
        protected FieldReplace(field: string): string {
            return field;
        }
        protected abstract GetModel(): any;
        protected abstract GetTreeListColumns(): kendo.ui.TreeListColumn[];
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
            input.kendoDropDownList({
                autoBind: true,
                dataTextField: textField,
                dataValueField: valueField,
                dataSource: dataSource
            });
        }
    }
}