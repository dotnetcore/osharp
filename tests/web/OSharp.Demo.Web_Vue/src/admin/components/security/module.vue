<template>
<kendoui-splitter ref="splitter" :options="splitterOptions">
  <div>
    <kendoui-treelist ref="treelist" :options="treelistOptions"/>
  </div>
  <div>222</div>
</kendoui-splitter>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import $ from "jquery";
import { KendoGrid } from "@progress/kendo-grid-vue-wrapper";
import { kendoui } from "../../../shared/osharp.kendoui";
import KendouiSplitter from "../../../shared/kendoui/kendoui-splitter.vue";
import KendouiTreeList from "../../../shared/kendoui/kendoui-treelist.vue";

@Component({
  components: {
    "kendoui-splitter": KendouiSplitter,
    "kendoui-treelist": KendouiTreeList
  }
})
export default class ModuleComponent extends kendoui.TreeListVueBase {
  moduleName = "module";
  splitterOptions: kendo.ui.SplitterOptions = null;
  $splitter: kendo.ui.Splitter = null;

  constructor() {
    super();
    this.splitterOptions = {
      panes: [{ size: "60%" }, { collapsible: true, collapsed: false }]
    };
  }

  protected GetModel() {
    return {
      id: "Id",
      parentId: "ParentId",
      fields: {
        Id: { type: "number", nullable: false, editable: false },
        ParentId: { type: "number", nullable: true },
        Name: { type: "string" },
        OrderCode: { type: "number" },
        Remark: { type: "string" }
      },
      expanded: true
    };
  }
  protected GetTreeListColumns() {
    return [
      {
        field: "Name",
        title: "名称",
        width: 200,
        template: d => "<span>" + d.Id + ". " + d.Name + "</span>"
      },
      { field: "Remark", title: "备注", width: 200 },
      {
        field: "OrderCode",
        title: "排序",
        width: 70,
        editor: (container, options) =>
          kendoui.Controls.NumberEditor(container, options, 3)
      },
      {
        title: "操作",
        command: [
          {
            name: "setFuncs",
            imageClass: "k-i-categorize",
            text: " ",
            click: function(e) {
              //$scope.module.setFuncs.open(e);
            }
          },
          { name: "createChild", text: " " },
          { name: "edit", text: " " },
          { name: "destroy", imageClass: "k-i-delete", text: " " }
        ],
        width: 180
      }
    ];
  }

  private ResizeSplitter() {
    if ((<any>this.$splitter)._size.width < 1150) {
      this.$splitter.collapse(".k-pane:last");
    }
  }

  created() {
    super.CreatedBase();
  }
  mounted() {
    super.MountedBase();
    this.$splitter = $((<any>this.$refs.splitter).$el).data("kendoSplitter");
    this.ResizeSplitter();
  }
}
</script>
 
<style scoped>
#grid-box {
  padding: 10px;
}
</style>