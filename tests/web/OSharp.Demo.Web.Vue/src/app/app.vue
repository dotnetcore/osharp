<template>
    <div id="app">
        <nav>
            <router-link to="/">Home</router-link>
            <router-link to="/demo">Demo</router-link>
            <a href="/admin/index.html">AdminApp</a>
        </nav>
        <div class="content">
            <header>App-Header</header>
            <router-view></router-view>
            <footer>App-Footer</footer>
        </div>
    </div>
</template>

<script lang="ts">
export default {
  name: "app"
};
</script>

<style scoped>
.content {
  color: green;
}
</style>