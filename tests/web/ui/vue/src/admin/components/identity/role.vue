<template>
    <div>Identity-Role</div>
</template>

<script lang="ts">
import Vue from "vue";
import { osharp } from "../../../shared/osharp";

export default Vue.extend({
  name: "admin-identity-role",

  mounted() {
    var s = new osharp.filter.Rule("name", "123");
    console.log(s);
  }
});
</script>
