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
  protected moduleName = "function";

  constructor() {
    super({
      model: {
        id: "Id",
        fields: {
          Name: { type: "string", editable: false },
          AccessType: { type: "number" },
          CacheExpirationSeconds: { type: "number" },
          AuditOperationEnabled: { type: "boolean" },
          AuditEntityEnabled: { type: "boolean" },
          IsCacheSliding: { type: "boolean" },
          IsLocked: { type: "boolean" },
          IsAjax: { type: "boolean", editable: false },
          Area: { type: "string", editable: false },
          Controller: { type: "string", editable: false },
          Action: { type: "string", editable: false }
        }
      }
    });
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [
      {
        field: "Name",
        title: "功能名称",
        width: 200,
        filterable: osharp.Data.stringFilterable
      },
      {
        field: "AccessType",
        title: "功能类型",
        width: 95,
        template: d =>
          osharp.Tools.valueToText(d.AccessType, osharp.Data.AccessTypes),
        editor: (container, options) =>
          kendoui.Controls.DropDownListEditor(
            container,
            options,
            osharp.Data.AccessTypes
          ),
        filterable: {
          ui: element =>
            kendoui.Controls.DropDownList(element, osharp.Data.AccessTypes)
        }
      },
      { field: "CacheExpirationSeconds", title: "缓存秒数", width: 95 },
      {
        field: "AuditOperationEnabled",
        title: "操作审计",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.AuditOperationEnabled)
      },
      {
        field: "AuditEntityEnabled",
        title: "数据审计",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.AuditEntityEnabled)
      },
      {
        field: "IsCacheSliding",
        title: "滑动过期",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.IsCacheSliding)
      },
      {
        field: "IsLocked",
        title: "已锁定",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.IsLocked)
      },
      {
        field: "IsAjax",
        title: "Ajax访问",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.IsAjax)
      },
      { field: "Area", title: "区域", width: 100, hidden: true },
      { field: "Controller", title: "控制器", width: 100, hidden: true },
      { field: "Action", title: "功能方法", width: 120, hidden: true }
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