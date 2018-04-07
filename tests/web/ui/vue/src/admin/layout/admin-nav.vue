<template>
    <mu-drawer @hide="handleHide" @close="handleClose" :open="open" :docked="docked" :overlay="docked" class="admin-drawer" :zDepth="1">
        <mu-appbar :zDepth="0" class="admin-nav-appbar">
            <a href="/index.html" class="admin-appbar-title">OSharpNS</a>
        </mu-appbar>
        <mu-divider/>
        <div class="admin-drawer-content">
            <mu-list @change="handleMenuChange" :value="hash">
              <div v-for="(menu,index) in menus" :key="'menu-item-'+index">
                <mu-divider v-if="menu.heading && index!=0"/>
                <mu-sub-header v-if="menu.heading">{{menu.text}}</mu-sub-header>
                <mu-list-item v-if="!menu.heading" :title="menu.text" :toggleNested="menu.submenus!=null" :value="menu.link">
                  <mu-icon v-if="menu.icon!=null" slot="left" :value="menu.icon"></mu-icon>
                  <mu-list-item v-for="(submenu,subindex) in menu.submenus" :key="'sub-menu'+subindex" slot="nested" :title="submenu.text" :value="submenu.link">
                    <mu-icon v-if="submenu.icon!=null" slot="left" :value="submenu.icon"></mu-icon>
                  </mu-list-item>
                </mu-list-item>
              </div>
            </mu-list>
        </div>
    </mu-drawer>
</template>

<script lang="ts">
import { List } from "linqts";
import { MenuItem, menuItems } from "./admin-menus";
export default {
  props: {
    open: { type: Boolean, default: true },
    docked: { type: Boolean, default: true }
  },
  data() {
    return {
      menus: [],
      hash: null
    };
  },
  methods: {
    handleClose() {
      this.$emit("close");
    },
    handleMenuChange(hash) {
      this.checkedMenu = hash;
      if (this.docked) {
        window.location.hash = hash;
        window.location.href = window.location.href;
      } else {
        this.changeHref = true;
      }

      this.$emit("change", hash);
    },
    handleHide() {
      if (!this.changeHref) {
        return;
      }
      window.location.hash = this.checkedMenu.link;
      this.changeHref = false;
    }
  },
  mounted() {
    this.hash = window.location.hash;
    this.menus = menuItems;
    window.addEventListener("hashchange", () => {
      this.hash = window.location.hash;
    });
  }
};
</script>

<style lang="less">
@import "../../../node_modules/muse-ui/src/styles/import.less";
.app-drawer {
  display: flex;
  flex-direction: column;
}

.admin-nav-appbar.mu-appbar {
  flex-shrink: 0;
}
.admin-drawer-content {
  flex: 1;
  .scrollable();
  .no-scrollbar();
}
.admin-appbar-title {
  color: inherit;
  display: inline-block;
}

.admin-version {
  margin-left: 10px;
  vertical-align: text-top;
}

.admin-nav-sub-header {
  padding-left: 34px;
}

.app-drawer {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -webkit-flex-direction: column;
  -ms-flex-direction: column;
  flex-direction: column;
}
.mu-drawer.open {
  -webkit-transform: translateZ(0);
  transform: translateZ(0);
  visibility: visible;
}
</style>
