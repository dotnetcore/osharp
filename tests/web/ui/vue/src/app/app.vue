<template>
    <div class="container">
      <div class="row">
        <div class="panel panel-default">
          <div class="panel-heading">
            <div class="panel-title">
              <h2 class="text-center">OSharp framework with .NetStandard2.0 & Vue</h2>
            </div>
          </div>
          <div class="panel-body">
            <router-link to="/" class="btn btn-default">
              <i class="glyphicon glyphicon-home"></i>
              <span>网站首页</span>
            </router-link>
            <router-link to="users" class="btn btn-default">
              <i class="glyphicon glyphicon-user"></i>
              <span>用户中心</span>
            </router-link>
            <a href="/admin/index.html" class="btn btn-info">
              <i class="glyphicon glyphicon-globe"></i>
              <span>进入后台</span>
            </a>
          </div>
        </div>
      </div>
      <router-view></router-view>
    </div>
</template>

<script lang="ts">
export default {
  name: "app"
};
</script>

<style lang="less">
//== Bootstrap
@import "../../node_modules/bootstrap/dist/css/bootstrap.css";
</style>
