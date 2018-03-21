//导航菜单
const headingMain = {
    text: '导航菜单',
    heading: true
};
const Home = {
    text: '主页',
    link: './dashboard',
    icon: 'icon-home'
};

//权限模块
const headingPermission = {
    text: '权限模块',
    heading: true
};
const Identity = {
    text: '身份认证',
    icon: 'icon-key',
    submenu: [
        { text: '用户信息管理', link: './identity/user', icon: 'icon-user' },
        { text: '角色信息管理', link: './identity/role', icon: 'icon-badge' },
        { text: '用户角色管理', link: './identity/user-role', icon: 'icon-people' },
    ]
};
const Security = {
    text: '权限安全',
    icon: 'icon-shield',
    submenu: [
        { text: '模块信息管理', link: './security/module', icon: 'icon-directions' },
        { text: '功能信息管理', link: './security/function', icon: 'icon-direction' },
        { text: '角色功能管理', link: './security/role-function', icon: 'icon-graduation' },
        { text: '用户功能管理', link: './security/user-function', icon: 'icon-magnet' },
        { text: '实体信息管理', link: './security/entityinfo', icon: 'icon-puzzle' },
        { text: '角色数据管理', link: './security/role-entityinfo', icon: 'icon-vector' },
        { text: '用户数据管理', link: './security/user-entityinfo', icon: 'icon-social-reddit' }
    ]
};

//系统模块
const headingSystem = {
    text: '系统模块',
    heading: true
};
const System = {
    text: '系统管理',
    icon: 'fa fa-desktop',
    submenu: [
        { text: '系统设置', link: './system/settings', icon: 'icon-settings' }
    ]
};

//Metrial
const Other = {
    text: '其他',
    heading: true
};
const Material = {
    text: "Material",
    link: "/material",
    icon: "fa fa-shield",
    submenu: [
        { text: "Widgets", link: "./material/widgets" },
        { text: "Cards", link: "./material/cards" },
        { text: "Forms", link: "./material/forms" },
        { text: "Inputs", link: "./material/inputs" },
        { text: "Lists", link: "./material/lists" },
        { text: "Whiteframe", link: "./material/whiteframe" },
        { text: "Colors", link: "./material/colors" },
        { text: "ng2Material", link: "./material/ngmaterial" }
    ],
};

export const menu = [
    headingMain,
    Home,
    headingPermission,
    Identity,
    Security,
    headingSystem,
    System,
    Other,
    Material
];