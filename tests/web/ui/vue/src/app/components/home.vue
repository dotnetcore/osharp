<template>
    <div class="row">
      <div class="row" v-if="startupInfo && startupInfo.Modules">
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="panel-title">模块列表</div>
            </div>
            <div class="panel-body" style="overflow:auto">
                <table class="table table-responsive table-bordered table-hover">
                    <thead>
                        <tr>
                            <td>名称</td>
                            <td>路径</td>
                            <td>级别</td>
                            <td>层次</td>
                            <td>已启用</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(module, index) in startupInfo.Modules" :key="index">
                            <td>{{module.Name}}</td>
                            <td>{{module.Class}}</td>
                            <td>{{module.Level}}</td>
                            <td>{{module.Order}}</td>
                            <td>{{module.IsEnabled}}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="row" v-if="startupInfo && startupInfo.Lines">
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="panel-title">其他信息</div>
            </div>
            <div class="panel-body">
                <ul class="list-group">
                   <li v-for="(line, index) in startupInfo.Lines" class="list-group-item" :key="index">{{line}}</li>
                </ul>
            </div>
        </div>
    </div>
    </div>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  name: "app-home",
  data() {
    return {
      startupInfo: null
    };
  },
  mounted() {
    this.$http.get("api/admin/system/info").then(res => {
      this.startupInfo = res.data;
    });
  }
});
</script>
