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
  protected moduleName = "user";

  constructor() {
    super({
      model: {
        id: "Id",
        fields: {
          Id: { type: "number", editable: false },
          UserName: { type: "string", validation: { required: true } },
          Email: { type: "string", validation: { required: true } },
          EmailConfirmed: { type: "boolean" },
          PhoneNumber: { type: "string" },
          PhoneNumberConfirmed: { type: "boolean" },
          LockoutEnabled: { type: "boolean" },
          LockoutEnd: { type: "date", editable: false },
          AccessFailedCount: { type: "number", editable: false },
          CreatedTime: { type: "date", editable: false },
          Roles: { editable: false }
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
        field: "UserName",
        title: "用户名",
        width: 150,
        filterable: osharp.Data.stringFilterable
      },
      {
        field: "Email",
        title: "邮箱",
        width: 200,
        filterable: osharp.Data.stringFilterable
      },
      {
        field: "EmailConfirmed",
        title: "邮箱确认",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.EmailConfirmed)
      },
      {
        field: "PhoneNumber",
        title: "手机号",
        width: 95,
        filterable: osharp.Data.stringFilterable
      },
      {
        field: "PhoneNumberConfirmed",
        title: "手机确认",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.PhoneNumberConfirmed)
      },
      {
        field: "Roles",
        title: "角色",
        width: 180,
        template: d => osharp.Tools.expandAndToString(d.Roles)
      },
      {
        field: "LockoutEnabled",
        title: "启用锁",
        width: 95,
        template: d => kendoui.Controls.Boolean(d.LockoutEnabled)
      },
      {
        field: "LockoutEnd",
        title: "锁时间",
        width: 110,
        format: "{0:yy-MM-dd HH:mm}"
      },
      {
        field: "AccessFailedCount",
        title: "登录错误",
        width: 95
      },
      {
        field: "CreatedTime",
        title: "注册时间",
        width: 110,
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