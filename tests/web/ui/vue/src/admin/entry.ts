import Vue from 'vue'
import axios from 'axios'
import $ from "jquery";

import AdminApp from './admin-app.vue'
import router from './router'

import MuseUI from 'muse-ui'
import 'muse-ui/dist/muse-ui.css'
import 'static/fonts/material-icons/material-icons.css'

import '@progress/kendo-ui'
import '@progress/kendo-ui/js/cultures/kendo.culture.zh-CN'
import '@progress/kendo-ui/js/messages/kendo.messages.zh-CN'
import { KendoGrid, KendoGridInstaller } from '@progress/kendo-grid-vue-wrapper'
import { KendoDataSource, KendoDataSourceInstaller } from '@progress/kendo-datasource-vue-wrapper'

Vue.config.productionTip = false
Vue.prototype.$http = axios;
Vue.use(MuseUI)

Vue.use(KendoGridInstaller)
Vue.use(KendoDataSourceInstaller)

new Vue({
  el: '#admin-app',
  router,
  template: '<AdminApp/>',
  components: { AdminApp }
})
