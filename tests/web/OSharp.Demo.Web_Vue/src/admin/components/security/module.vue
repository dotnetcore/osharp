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
import { kendoui } from "../../../shared/osharp.kendoui";

@Component({})
export default class ModuleComponent extends kendoui.GridVueBase {
  moduleName = "module";

  constructor() {
    super({
      model: {
        id: "Id",
        fields: {
          Id: { type: "number" },
          Name: { type: "string" }
        }
      }
    });
  }

  protected GetGridColumns(): kendo.ui.GridColumn[] {
    return [{ field: "Id", title: "编号" }, { field: "Name", title: "名称" }];
  }

  created() {
    this.GetGridOptions();
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