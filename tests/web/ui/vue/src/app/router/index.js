import Vue from 'vue'
import Router from 'vue-router'
import Home from 'app/components/home.vue'
import Users from 'app/components/users.vue'

Vue.use(Router)

export default new Router({
  routes: [
    { path: '/', component: Home },
    { path: '/users', component: Users },
  ]
})
