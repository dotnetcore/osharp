import Vue from 'vue'
import AdminApp from './admin-app.vue'
import router from './router'

import MuseUI from 'muse-ui'
import 'muse-ui/dist/muse-ui.css'
import 'static/fonts/material-icons/material-icons.css'

import '@progress/kendo-ui'
import { KendoDataSource, KendoDataSourceInstaller } from '@progress/kendo-datasource-vue-wrapper'
import { KendoGrid, KendoGridInstaller } from '@progress/kendo-grid-vue-wrapper'

Vue.config.productionTip = false
Vue.use(MuseUI)
Vue.use(KendoDataSourceInstaller)
Vue.use(KendoGridInstaller)

new Vue({
    el: '#admin-app',
    render: h => h(AdminApp),
    router,
    template: '<AdminApp/>',
    components: { KendoDataSource, KendoGrid }
})