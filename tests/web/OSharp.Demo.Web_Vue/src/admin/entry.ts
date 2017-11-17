import Vue from 'vue'
import AdminApp from './admin-app.vue'
import router from './router'

import MuseUI from 'muse-ui'
import 'muse-ui/dist/muse-ui.css'
import '@static/fonts/material-icons/material-icons.css'

import VueNotifications from 'vue-notifications'
import toastr from 'toastr'
import 'toastr/build/toastr.min.css'

import '@progress/kendo-ui'
import { KendoGrid, KendoGridInstaller } from '@progress/kendo-grid-vue-wrapper'
require('@progress/kendo-ui/js/cultures/kendo.culture.zh-CN')
require('@progress/kendo-ui/js/messages/kendo.messages.zh-CN')

Vue.config.productionTip = false
Vue.use(MuseUI)
Vue.use(KendoGridInstaller)

new Vue({
    el: '#admin-app',
    render: h => h(AdminApp),
    router,
    template: '<AdminApp/>',
    components: {}
})