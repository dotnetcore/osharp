import Vue from 'vue'
import Router from 'vue-router'

import Dashboard from '../components/dashboard.vue'
import IdentityUser from '../components/identity/user.vue'
import IdentityRole from '../components/identity/role.vue'
import IdentityUserRole from '../components/identity/user-role.vue'
import SecurityModule from '../components/security/module.vue'
import SecurityFunction from '../components/security/function.vue'
import SecurityRoleFunction from '../components/security/role-function.vue'
import SecurityUserFunction from '../components/security/user-function.vue'
import SecurityEntityInfo from '../components/security/entityinfo.vue'
import SecurityRoleEntityInfo from '../components/security/role-entityinfo.vue'
import SecurityUserEntityInfo from '../components/security/user-entityinfo.vue'
import SystemSettings from '../components/system/settings.vue'

Vue.use(Router)

export default new Router({
    base: '/admin/',
    //mode: 'history',
    routes: [
        { path: '/', component: Dashboard },
        { path: '/dashboard', component: Dashboard },
        { path: '/identity/user', component: IdentityUser },
        { path: '/identity/role', component: IdentityRole },
        { path: '/identity/user-role', component: IdentityUserRole },
        { path: '/security/module', component: SecurityModule },
        { path: '/security/function', component: SecurityFunction },
        { path: '/security/role-function', component: SecurityRoleFunction },
        { path: '/security/user-function', component: SecurityUserFunction },
        { path: '/security/entityinfo', component: SecurityEntityInfo },
        { path: '/security/role-entityinfo', component: SecurityRoleEntityInfo },
        { path: '/security/user-entityinfo', component: SecurityUserEntityInfo },
        { path: '/system/settings', component: SystemSettings }
    ]
})