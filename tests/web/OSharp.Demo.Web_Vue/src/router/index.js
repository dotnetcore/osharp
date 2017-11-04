import Vue from 'vue'
import VueRouter from 'vue-router'
import HelloWorld from '../components/HelloWorld'
import Test02 from '../components/Test02'
import { Test01Component } from '../components/test01.component'

Vue.use(VueRouter)

export default new VueRouter({
  routes: [
    {
      path: '/',
      name: 'Hello',
      component: HelloWorld
    },
    { path: 'test01', name: 'Test01', component: Test01Component },
    { path: "test02", name: "Test02", component: Test02 }
  ]
})
