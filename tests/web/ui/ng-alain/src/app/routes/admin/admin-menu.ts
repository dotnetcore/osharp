import { Menu } from "@delon/theme";

// 导航菜单
const headingMain = { text: '导航菜单', group: true };

const home = { text: '主页', link: '/admin/dashboard', icon: 'icon-home' };

export const adminMenu: Menu[] = [
  {
    text: '导航菜单', group: true, children: [
      { text: '主页', link: '/admin/dashboard', icon: 'icon-home' }
    ]
  }, {
    text: '权限模块', group: true, children: [
      {
        text: '身份认证', group: true, icon: 'icon-key', children: [
          { text: '用户信息管理', link: '/admin/identity/user', icon: 'icon-user' },
          { text: '角色信息管理', link: '/admin/identity/role', icon: 'icon-badge' },
          { text: '用户角色管理', link: '/admin/identity/user-role', icon: 'icon-people' }
        ]
      }, {
        text: '权限安全', group: true, icon: 'icon-shield', children: [
          { text: '模块信息管理', link: '/admin/security/module', icon: 'icon-directions' },
          { text: '功能信息管理', link: '/admin/security/function', icon: 'icon-direction' },
          { text: '角色功能管理', link: '/admin/security/role-function', icon: 'icon-graduation' },
          { text: '用户功能管理', link: '/admin/security/user-function', icon: 'icon-magnet' },
          { text: '实体信息管理', link: '/admin/security/entityinfo', icon: 'icon-puzzle' },
          { text: '角色数据管理', link: '/admin/security/role-entity', icon: 'icon-vector' },
          { text: '用户数据管理', link: '/admin/security/user-entityinfo', icon: 'icon-social-reddit' }
        ]
      }
    ]
  }, {
    text: '系统模块', group: true, children: [
      {
        text: '系统管理', group: true, icon: 'icon-globe', children: [
          { text: '系统设置', link: '/admin/system/settings', icon: 'icon-settings' }
        ]
      }
    ]
  }
];
