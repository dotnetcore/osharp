<template>
    <div id="admin-app">
      <mu-appbar :zDepth=0 :title="title" class="admin-appbar" :class="{'nav-hide':!open}">
        <mu-icon-button @click="toggleNav" icon="menu" slot="left"/>
      </mu-appbar>
      <admin-nav @change="handleMenuChange" @close="toggleNav" :open="open" :docked="docked"/>
      <div class="admin-content admin-page" :class="{'nav-hide':!open}">
          <router-view></router-view>
      </div>
    </div>
</template>

<script lang="ts">
import { List } from "linqts";
import $ from "jquery";
import Bus from "../shared/eventBus";
import AdminNav from "./layout/admin-nav.vue";
import { MenuItem, menuItems } from "./layout/admin-menus";
import { osharp } from "../shared/osharp";

export default {
  name: "admin-app",
  data() {
    const desktop: Boolean = isDesktop();
    return {
      open: desktop,
      docked: desktop,
      desktop: desktop,
      title: ""
    };
  },
  mounted() {
    this.routes = this.$router.options.routes;
    this.setTitle();
    this.handleResize();
    window.addEventListener("resize", () => this.handleResize());
    window.addEventListener("hashchange", () => {
      this.setTitle();
    });
  },
  computed: {
    menus: function() {
      let items = [];
      new List<MenuItem>(menuItems).Where(m => !m.heading).ForEach(item => {
        if (!item.submenus || item.submenus.length == 0) {
          items.push(item);
          return true;
        }
        item.submenus.forEach(sub => {
          if (sub.link) {
            items.push(sub);
          }
        });
      });
      return items;
    }
  },
  components: {
    "admin-nav": AdminNav
  },
  methods: {
    toggleNav() {
      this.open = !this.open;
    },
    handleResize() {
      this.resizeNav();
    },
    resizeNav() {
      const desktop = isDesktop();
      this.docked = desktop;
      if (desktop === this.desktop) return;
      if (!desktop && this.desktop && this.open) {
        this.open = false;
      }
      if (desktop && !this.desktop && !this.open) {
        this.open = true;
      }
      this.desktop = desktop;
    },
    handleMenuChange(menu) {
      if (!this.desktop) this.open = false;
    },
    setTitle() {
      let hash = window.location.hash.replace("#/", "") || "dashboard";
      this.menus.forEach(item => {
        if (item.link == hash) {
          this.title = item.text;
          return false;
        }
      });
    }
  }
};

function isDesktop() {
  return window.innerWidth > 993;
}
</script>

<style lang="less">
@import url("../../node_modules/muse-ui/src/styles/import.less");
@import url("../../node_modules/@progress/kendo-theme-default/dist/all.css");
//@import "../../node_modules/@progress/kendo-theme-bootstrap/dist/all.css";
//@import "../../node_modules/@progress/kendo-theme-material/dist/all.css";
//@import url("https://kendo.cdn.telerik.com/2017.3.1026/styles/kendo.common-material.min.css");
//@import url("https://kendo.cdn.telerik.com/2017.3.1026/styles/kendo.common.min.css");
//@import url("https://kendo.cdn.telerik.com/2017.3.1026/styles/kendo.material.min.css");

#admin-app {
  height: 100%;
}

.admin-appbar {
  position: fixed;
  left: 256px;
  right: 0;
  top: 0;
  width: auto;
  transition: all 0.45s @easeOutFunction;
  &.nav-hide {
    left: 0;
  }
}

.admin-content {
  padding-top: 56px;
  padding-left: 256px;
  transition: all 0.45s @easeOutFunction;
  &.nav-hide {
    padding-left: 0;
  }
}

.content-wrapper {
  padding: 48px 72px;
}

@media (min-width: 480px) {
  .admin-content {
    padding-top: 64px;
  }
}

@media (max-width: 993px) {
  .admin-appbar {
    left: 0;
  }
  .admin-content {
    padding-left: 0;
  }
  .content-wrapper {
    padding: 24px 36px;
  }
}
.home-page {
  padding: 0;
  .admin-content {
    transition-duration: 0s;
  }
}
.k-grid-header .k-grid-filter {
  margin: -4px -1px -4px -5px;
}
.k-header > .k-grid-filter,
.k-header > .k-header-column-menu {
  float: left;
  padding: 0.5em 0.2em 0.4em 0.1em;
  width: 16px;
  height: 28px;
}
.k-grid-header .k-header > .k-link {
  margin: -0.5em -0.6em -0.4em 0;
  padding: 0.5em 0.6em 0.4em 1.2em;
  overflow: visible;
}
.k-header .k-with-icon {
  padding-left: 6px;
  margin-left: -6px;
}
.k-grid-header .k-header > .k-link > .k-icon.k-i-sort-asc-sm,
.k-grid-header .k-header > .k-link > .k-icon.k-i-sort-desc-sm {
  margin-left: 0;
}
// .k-grid-header .k-grid-filter,
// .k-grid-header .k-header-column-menu {
//   padding: 0.714em 0;
//   margin: -0.786em -0.6em -0.786em -0.5em;
// }
</style>
