<template>
    <div id="grid-box">
      <kendo-grid ref="grid" v-bind="gridOptions"></kendo-grid>
    </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import $ from "jquery";
import { KendoGrid } from "@progress/kendo-grid-vue-wrapper";
import { osharp } from "../../../shared/osharp";
import { kendoui } from "../../../shared/osharp.kendoui";

@Component({})
export default class FunctionComponent extends kendoui.GridVueBase {
  protected moduleName = "role";

  constructor() {
    super({
      model: {
        id: "Id",
        fields: {
          Id: { type: "number", editable: false },
          Name: { type: "string", validation: { required: true } },
          Remark: { type: "string" },
          IsAdmin: { type: "boolean" },
          IsDefault: { type: "boolean" },
          IsLocked: { type: "boolean" },
          IsSystem: { type: "boolean", editable: false },
          CreatedTime: { type: "date", editable: false }
        }
      }
    });
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        command: [
          { name: "destroy", iconClass: "k-icon k-i-delete", text: "" }
        ],
        width: 50
      },
      { field: "Id", title: "编号", width: 70 },
      {
        field: "Name",
        title: "角色名",
        width: 150,
        filterable: osharp.Data.stringFilterable
      },
      {
        field: "Remark",
        title: "备注",
        width: 250,
        filterable: osharp.Data.stringFilterable
      },
      {
        field: "IsAdmin",
        title: "管理",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.IsAdmin)
      },
      {
        field: "IsDefault",
        title: "默认",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.IsDefault)
      },
      {
        field: "IsLocked",
        title: "锁定",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.IsLocked)
      },
      {
        field: "IsSystem",
        title: "系统",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.IsSystem)
      },
      {
        field: "CreatedTime",
        title: "注册时间",
        width: 130,
        format: "{0:yy-MM-dd HH:mm}"
      }
    ];
  }

  protected GetGridOptionsInternal(
    dataSource: kendo.data.DataSource
  ): kendo.ui.GridOptions {
    var options = super.GetGridOptionsInternal(dataSource);
    //options.columnMenu = { sortable: false };
    options.toolbar = ["create", "save", "cancel"];
    return options;
  }

  created() {
    this.GetGridOptions();
  }
  mounted() {
    this.$grid = $((<any>this.$refs.grid).$el).data("kendoGrid");
  }
}
</script>

<style scoped>
#grid-box {
  padding: 10px;
}
</style>