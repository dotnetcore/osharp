<script lang="ts">
import $ from "jquery";

export default {
  name: "kendoui-treelist",
  template: "<div></div>",
  props: ["options"],
  mounted() {
    $(this.$el).kendoTreeList(this.options);
  }
};
</script>
