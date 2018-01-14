<script lang="ts">
import $ from "jquery";

export default {
  name: "kendoui-splitter",
  template: "<div><slot/></div>",
  props: ["options"],
  mounted() {
    $(this.$el).kendoSplitter(this.options);
  }
};
</script>
