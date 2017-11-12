import Vue from 'vue'
import AdminApp from './admin-app.vue'
import router from './router'

import MuseUI from 'muse-ui'
import 'muse-ui/dist/muse-ui.css'
import 'static/fonts/material-icons/material-icons.css'

Vue.config.productionTip = false
Vue.use(MuseUI)

new Vue({
    el: '#admin-app',
    router,
    template: '<AdminApp/>',
    components: { AdminApp }
})