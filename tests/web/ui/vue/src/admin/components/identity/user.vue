
<template>
    <div>
      <kendo-datasource ref="gridDataSource" v-bind="gridDataSourceOptions"></kendo-datasource>
      <kendo-grid :data-source-ref="'gridDataSource'">
        <kendo-grid-column field="Id" title="编号" :width="70"></kendo-grid-column>
        <kendo-grid-column field="UserName" title="用户名" :width="150"></kendo-grid-column>
        <kendo-grid-column field="Email" title="邮箱" :width="200"></kendo-grid-column>
        <kendo-grid-column field="EmailConfirmed" title="邮箱确认" :width="95"></kendo-grid-column>
      </kendo-grid>
    </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { kendoui } from "../../../shared/kendoui";

@Component({})
export default class UserComponent extends kendoui.KendoGridVueBase {
  constructor() {
    super();
    this.moduleName = "user";
  }

  protected GetModel() {
    return {
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
    };
  }

  created() {
    super.CreatedBase();
  }
  mounted() {}
}
</script>
