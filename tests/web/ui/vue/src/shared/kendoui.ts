import Vue from "vue";
import { List } from "linqts"
import { osharp } from "./osharp";

export namespace kendoui {
  export abstract class KendoGridVueBase extends Vue {

    protected moduleName: string = null;
    public gridDataSourceOptions: kendo.data.DataSourceOptions = null;

    constructor() {
      super();
    }

    protected CreatedBase() {
      this.gridDataSourceOptions = this.GetDataSourceOptions();
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
        //requestEnd: e => osharp.Tools.ajaxResult(e.response, () => this.$grid.options.dataSource.read(), null),
        change: function () { }
      };

      return options;
    }

    /**重写以替换字段的查询时字段名，可以点号.进行导航属性的分隔 */
    protected FieldReplace(field: string): string {
      return field;
    }
    /**重写以获取Grid的模型设置Model */
    protected abstract GetModel(): any;

    protected ResizeGrid(init: boolean) {
      var winWidth = window.innerWidth, winHeight = window.innerHeight, diffHeight = winWidth < 480 ? 57 : 65;;
      var $content = $("#grid-box .k-grid-content");
      var otherHeight = $("#grid-box").height() - $content.height();
      $content.height(winHeight - diffHeight - otherHeight - (init ? 0 : 20));
    }

  }
}
