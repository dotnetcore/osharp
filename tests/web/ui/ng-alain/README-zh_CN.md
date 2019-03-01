[[![Build Status](https://travis-ci.org/cipchk/ng-alain.svg?branch=master)](https://travis-ci.org/cipchk/ng-alain)
[![Dependency Status](https://david-dm.org/cipchk/ng-alain/status.svg)](https://david-dm.org/cipchk/ng-alain)
[![NPM version](https://img.shields.io/npm/v/ng-alain.svg)](https://www.npmjs.com/package/ng-alain)
[![prettier](https://img.shields.io/badge/code_style-prettier-ff69b4.svg?style=flat-square)](https://prettier.io/)

# ng-alain

一套基于 [Ng-zorro-antd](https://github.com/NG-ZORRO/ng-zorro-antd)【ANT DESIGN】 的企业后台模板。

[README in English](README.md)

[DEMO](https://cipchk.github.io/ng-alain/)

## 快速入门

有二种方式进行安装：

### 命令行工具

```bash
ng new demo --style less
cd demo
ng add ng-alain
ng serve
```

请参考[命令行工具](http://ng-alain.com/docs/cli)了解更多细节。

### 直接 clone git 仓库

```bash
$ git clone --depth=1 https://github.com/cipchk/ng-alain.git my-project

cd my-project

# 安装依赖包
npm install

# 启动
npm start

# 使用HMR启动
npm run serve:hmr
```

> [vscode] 建议安装 [ng-zorro-vscode](https://marketplace.visualstudio.com/items?itemName=cipchk.ng-zorro-vscode) 插件，含 `nz-alain-*` 代码片断。


## Links

+ [文档](http://ng-alain.com)
+ [@delon](https://github.com/cipchk/delon)
+ [DEMO](https://cipchk.github.io/ng-alain/)

## Delon

[delong](https://github.com/cipchk/delon) 是基于 Ant Design 设计理念的企业级中后台前端业务型组件库。

[![Build Status](https://travis-ci.org/cipchk/delon.svg?branch=master)](https://travis-ci.org/cipchk/delon)
[![Dependency Status](https://david-dm.org/cipchk/delon/status.svg)](https://david-dm.org/cipchk/delon)
[![DevDependency Status](https://david-dm.org/cipchk/delon/dev-status.svg)](https://david-dm.org/cipchk/delon?type=dev)

[![npm](https://img.shields.io/npm/l/@delon/theme.svg)](https://www.npmjs.com/package/@delon/theme)
[![npm](https://img.shields.io/npm/dm/@delon/theme.svg)](https://www.npmjs.com/package/@delon/theme)

| package name |                                                   version                                                   |                                                   next version                                                   |
|--------------|:-----------------------------------------------------------------------------------------------------------:|:----------------------------------------------------------------------------------------------------------------:|
| @delon/theme | [![NPM version](https://img.shields.io/npm/v/@delon/theme.svg)](https://www.npmjs.com/package/@delon/theme) | [![NPM version](https://img.shields.io/npm/v/@delon/theme/next.svg)](https://www.npmjs.com/package/@delon/theme) |
| @delon/abc   |   [![NPM version](https://img.shields.io/npm/v/@delon/abc.svg)](https://www.npmjs.com/package/@delon/abc)   |   [![NPM version](https://img.shields.io/npm/v/@delon/abc/next.svg)](https://www.npmjs.com/package/@delon/abc)   |
| @delon/form  |  [![NPM version](https://img.shields.io/npm/v/@delon/form.svg)](https://www.npmjs.com/package/@delon/form)  |  [![NPM version](https://img.shields.io/npm/v/@delon/form/next.svg)](https://www.npmjs.com/package/@delon/form)  |
| @delon/acl   |   [![NPM version](https://img.shields.io/npm/v/@delon/acl.svg)](https://www.npmjs.com/package/@delon/acl)   |   [![NPM version](https://img.shields.io/npm/v/@delon/acl/next.svg)](https://www.npmjs.com/package/@delon/acl)   |
| @delon/auth  |  [![NPM version](https://img.shields.io/npm/v/@delon/auth.svg)](https://www.npmjs.com/package/@delon/auth)  |  [![NPM version](https://img.shields.io/npm/v/@delon/auth/next.svg)](https://www.npmjs.com/package/@delon/auth)  |
| @delon/mock  |  [![NPM version](https://img.shields.io/npm/v/@delon/mock.svg)](https://www.npmjs.com/package/@delon/mock)  |  [![NPM version](https://img.shields.io/npm/v/@delon/mock/next.svg)](https://www.npmjs.com/package/@delon/mock)  |
| @delon/cache | [![NPM version](https://img.shields.io/npm/v/@delon/cache.svg)](https://www.npmjs.com/package/@delon/cache) | [![NPM version](https://img.shields.io/npm/v/@delon/cache/next.svg)](https://www.npmjs.com/package/@delon/cache) |
| @delon/util  |  [![NPM version](https://img.shields.io/npm/v/@delon/util.svg)](https://www.npmjs.com/package/@delon/util)  |  [![NPM version](https://img.shields.io/npm/v/@delon/util/next.svg)](https://www.npmjs.com/package/@delon/util)  |

## Architecture

![Architecture](https://github.com/cipchk/delon/blob/master/_screenshot/architecture.png)

## 特性

+ 基于 `ng-zorro-antd`
+ 响应式
+ 国际化
+ ACL访问控制
+ 延迟加载及良好的启用画面
+ 良好的UI路由设计
+ 十种颜色版本
+ Less预编译
+ 良好的目录组织结构
+ 简单升级
+ 模块热替换
+ 支持Docker部署
+ 支持[Electron](http://ng-alain.com/docs/cli#electron)打包（限cli构建）

## 应用截图

![desktop](https://github.com/cipchk/delon/blob/master/_screenshot/desktop.png)
![ipad](https://github.com/cipchk/delon/blob/master/_screenshot/ipad.png)
![iphone](https://github.com/cipchk/delon/blob/master/_screenshot/iphone.png)

## Troubleshooting

Please follow this guidelines when reporting bugs and feature requests:

1. Use [GitHub Issues](https://github.com/cipchk/ng-alain/issues) board to report bugs and feature requests (not our email address)
2. Please **always** write steps to reproduce the error. That way we can focus on fixing the bug, not scratching our heads trying to reproduce it.

Thanks for understanding!

### License

The MIT License (see the [LICENSE](https://github.com/cipchk/ng-alain/blob/master/LICENSE) file for the full text)
](<p align="center">
  <a href="https://ng-alain.com">
    <img width="100" src="https://ng-alain.com/assets/img/logo-color.svg">
  </a>
</p>

<h1 align="center">
Ng Alain
</h1>

<div align="center">

  一个基于 Antd 中后台前端解决方案，提供更多通用性业务模块，让开发者更加专注于业务。

  [![Build Status](https://img.shields.io/travis/ng-alain/ng-alain/master.svg?style=flat-square)](https://travis-ci.org/ng-alain/ng-alain)
  [![Dependency Status](https://david-dm.org/ng-alain/ng-alain/status.svg?style=flat-square)](https://david-dm.org/ng-alain/ng-alain)
  [![GitHub Release Date](https://img.shields.io/github/release-date/ng-alain/ng-alain.svg?style=flat-square)](https://github.com/ng-alain/ng-alain/releases)
  [![NPM version](https://img.shields.io/npm/v/ng-alain.svg?style=flat-square)](https://www.npmjs.com/package/ng-alain)
  [![prettier](https://img.shields.io/badge/code_style-prettier-ff69b4.svg?style=flat-square)](https://prettier.io/)
  [![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg?style=flat-square)](https://github.com/ng-alain/ng-alain/blob/master/LICENSE)
  [![Gitter](https://img.shields.io/gitter/room/ng-alain/ng-alain.svg?style=flat-square)](https://gitter.im/ng-alain/ng-alain)
  [![ng-zorro-vscode](https://img.shields.io/badge/ng--zorro-VSCODE-brightgreen.svg?style=flat-square)](https://marketplace.visualstudio.com/items?itemName=cipchk.ng-zorro-vscode)
  [![ng-alain-vscode](https://img.shields.io/badge/ng--alain-VSCODE-brightgreen.svg?style=flat-square)](https://marketplace.visualstudio.com/items?itemName=cipchk.ng-alain-vscode)

</div>

[English](README.md) | 简体中文

## 快速入门

```bash
# 确保使用的是最新版本 Angular cli
ng new demo --style less
cd demo
ng add ng-alain
ng serve
```

> 请参考[命令行工具](https://ng-alain.com/cli)了解更多细节。
>
> [vscode] 建议安装 [ng-zorro-vscode](https://marketplace.visualstudio.com/items?itemName=cipchk.ng-zorro-vscode) 和 [ng-alain-vscode](https://marketplace.visualstudio.com/items?itemName=cipchk.ng-alain-vscode) 插件，开发更爽。


## 链接

+ [文档](https://ng-alain.com)
+ [@delon](https://github.com/ng-alain/delon)
+ [DEMO](https://ng-alain.github.io/ng-alain/)

## 特性

+ 基于 `ng-zorro-antd`
+ 响应式
+ 国际化
+ 基建类库 [@delon](https://github.com/ng-alain/delon)（包括：业务组件、ACL访问控制、缓存、授权、动态表单等）
+ 延迟加载及良好的启用画面
+ 良好的UI路由设计
+ 定制主题
+ Less预编译
+ 良好的目录组织结构
+ 简单升级
+ 支持Docker部署

## Architecture

![Architecture](https://raw.githubusercontent.com/ng-alain/delon/master/_screenshot/architecture.png)

> [delon](https://github.com/ng-alain/delon) 是基于 Ant Design 设计理念的企业级中后台前端业务型组件库。

## 应用截图

![desktop](https://raw.githubusercontent.com/ng-alain/delon/master/_screenshot/desktop.png)
![ipad](https://raw.githubusercontent.com/ng-alain/delon/master/_screenshot/ipad.png)
![iphone](https://raw.githubusercontent.com/ng-alain/delon/master/_screenshot/iphone.png)

## 赞助

ng-alain是MIT协议的开源项目。为了项目能够更好的持续的发展，我们期望获得更多的支持者，你可以通过如下任何一种方式支持我们：

- [patreon](https://www.patreon.com/cipchk)
- [opencollective](https://opencollective.com/ng-alain)
- [paypal](https://www.paypal.me/cipchk)
- [支付宝或微信](https://ng-alain.com/assets/donate.png)

或购买我们 [商品主题](https://e.ng-alain.com/)。

## Backers

Thank you to all our backers! 🙏

<a href="https://opencollective.com/ng-alain#backers" target="_blank"><img src="https://opencollective.com/ng-alain/backers.svg?width=890"></a>

### License

The MIT License (see the [LICENSE](https://github.com/ng-alain/ng-alain/blob/master/LICENSE) file for the full text)
)
