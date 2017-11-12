import Vue from 'vue'
import Router from 'vue-router'
import Home from 'app/components/home.vue'
import Demo from 'app/components/demo.vue'

Vue.use(Router)

export default new Router({
    routes: [
        { path: '/', component: Home },
        { path: '/demo', component: Demo },
    ]
})