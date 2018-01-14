//导航菜单
const headingMain = {
    text: "导航菜单",
    heading: true
};
const Home = {
    text: "信息汇总",
    link: "dashboard",
    icon: "home"
};

//权限模块
const headingPermission = {
    text: "权限模块",
    heading: true
};
const Identity = {
    text: "身份认证",
    icon: "perm_contact_calendar",
    submenus: [
        { text: "用户信息管理", link: "identity/user", icon: "person" },
        { text: "角色信息管理", link: "identity/role", icon: "people" },
        { text: "用户角色管理", link: "identity/user-role", icon: "people_outline" }
    ]
};
const Security = {
    text: "权限安全",
    icon: "verified_user",
    submenus: [
        { text: '模块信息管理', link: 'security/module', icon: 'extension' },
        { text: '功能信息管理', link: 'security/function', icon: 'games' },
        { text: '角色功能管理', link: 'security/role-function', icon: 'assignment' },
        { text: '用户功能管理', link: 'security/user-function', icon: 'assignment_ind' },
        { text: '实体信息管理', link: 'security/entityinfo', icon: 'explicit' },
        { text: '角色数据管理', link: 'security/role-entityinfo', icon: 'speaker_group' },
        { text: '用户数据管理', link: 'security/user-entityinfo', icon: 'speaker' }
    ]
};

//系统模块
const headingSystem = {
    text: "系统模块",
    heading: true
};
const System = {
    text: '系统管理',
    icon: 'laptop_chromebook',
    submenus: [
        { text: '系统设置', link: 'system/settings', icon: 'settings' }
    ]
};

export const menuItems = [
    headingMain,
    Home,
    headingPermission,
    Identity,
    Security,
    headingSystem,
    System
];

export class MenuItem {
    text: string;
    heading?: boolean;
    link?: string;
    icon?: string;
    submenus?: Array<MenuItem>
}