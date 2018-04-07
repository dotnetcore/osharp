import Vue from 'vue'
import axios from 'axios'
import $ from "jquery";

import App from './app.vue'
import router from './router'

Vue.config.productionTip = false
Vue.prototype.$http = axios;

new Vue({
  el: '#app',
  router,
  template: '<App/>',
  components: { App }
})
