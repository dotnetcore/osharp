import Vue from 'vue'
import Router from 'vue-router'

import Dashboard from '@admin/components/dashboard.vue'
import IdentityUser from '@admin/components/identity/user.vue'
import IdentityRole from '@admin/components/identity/role.vue'
import IdentityUserRole from '@admin/components/identity/user-role.vue'
import SecurityModule from '@admin/components/security/module.vue'
import SecurityFunction from '@admin/components/security/function.vue'
import SecurityRoleFunction from '@admin/components/security/role-function.vue'
import SecurityUserFunction from '@admin/components/security/user-function.vue'
import SecurityEntityInfo from '@admin/components/security/entityinfo.vue'
import SecurityRoleEntityInfo from '@admin/components/security/role-entityinfo.vue'
import SecurityUserEntityInfo from '@admin/components/security/user-entityinfo.vue'
import SystemSettings from '@admin/components/system/settings.vue'

Vue.use(Router)

const routes = [
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
    { path: '/system/settings', component: SystemSettings },
    { path: '*', redirect: '/' }
];

const router = new Router({
    routes: routes
});

router.beforeEach((to, from, next) => {
    window.scrollTo(0, 0); //scroll to top
    next()
});

export default router;