<script lang="ts">
import $ from "jquery";

export default {
  name: "kendoui-grid",
  template: "<div></div>",
  props: ["options"],
  mounted() {
    $(this.$el).kendoGrid(this.options);
  }
};
</script>
