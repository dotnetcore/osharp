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
  protected moduleName = "entityinfo";

  constructor() {
    super({
      model: {
        id: "Id",
        fields: {
          Name: { type: "string", editable: false },
          TypeName: { type: "string", editable: false },
          AuditEnabled: { type: "boolean" }
        }
      }
    });
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        field: "Name",
        title: "实体名称",
        width: 150,
        filterable: osharp.Data.stringFilterable
      },
      {
        field: "TypeName",
        title: "实体类型",
        width: 250,
        filterable: osharp.Data.stringFilterable
      },
      {
        field: "AuditEnabled",
        title: "数据审计",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.AuditEnabled)
      }
    ];
  }

  protected GetGridOptionsInternal(
    dataSource: kendo.data.DataSource
  ): kendo.ui.GridOptions {
    var options = super.GetGridOptionsInternal(dataSource);
    options.columnMenu = { sortable: false };
    options.toolbar = ["save", "cancel"];
    return options;
  }

  created() {
    super.CreatedBase();
  }
  mounted() {
    super.MountedBase();
  }
}
</script>

<style scoped>
#grid-box {
  padding: 10px;
}
</style>