/**
 * 进一步对基础模块的导入提炼
 * 有关模块注册指导原则请参考：https://github.com/ng-alain/ng-alain/issues/180
 */
import { NgModule, Optional, SkipSelf, ModuleWithProviders } from '@angular/core';
import { throwIfAlreadyLoaded } from '@core';

import { AlainThemeModule } from '@delon/theme';

// #region mock
import { DelonMockModule } from '@delon/mock';
import * as MOCKDATA from '../../_mock';
import { environment } from '@env/environment';
const MOCK_MODULES = !environment.production
  ? [DelonMockModule.forRoot({ data: MOCKDATA })]
  : [];
// #endregion

// #region reuse-tab
/**
 * 若需要[路由复用](https://ng-alain.com/components/reuse-tab)需要：
 * 1、增加 `REUSETAB_PROVIDES`
 * 2、在 `src/app/layout/default/default.component.html` 修改：
 *  ```html
 *  <section class="alain-default__content">
 *    <reuse-tab></reuse-tab>
 *    <router-outlet></router-outlet>
 *  </section>
 *  ```
 */
import { RouteReuseStrategy } from '@angular/router';
import { ReuseTabService, ReuseTabStrategy } from '@delon/abc/reuse-tab';
const REUSETAB_PROVIDES = [
  {
    provide: RouteReuseStrategy,
    useClass: ReuseTabStrategy,
    deps: [ReuseTabService],
  },
];
// #endregion

// #region global config functions

import { PageHeaderConfig } from '@delon/abc';
export function fnPageHeaderConfig(): PageHeaderConfig {
  return {
    ...new PageHeaderConfig(),
    ...{ homeI18n: 'home' } as PageHeaderConfig
  };
}

import { DelonAuthConfig } from '@delon/auth';
export function fnDelonAuthConfig(): DelonAuthConfig {
  return {
    ...new DelonAuthConfig(),
    ...{ login_url: '/passport/login', ignores: [/\/login/, /assets\//, /api\/(?!admin)[\w_-]+\/\S*/] } as DelonAuthConfig
  };
}

import { DelonACLConfig, ACLCanType, ACLType } from "@delon/acl";
export function fnDelonACLConfig(): DelonACLConfig {
  return {
    guard_url: '/exception/403',
    preCan: (roleOrAbility: ACLCanType) => {
      function isAbility(val: string) {
        return val && val.startsWith('Root.');
      }

      // 单个字符串，可能是角色也可能是功能点
      if (typeof roleOrAbility === 'string') {
        return isAbility(roleOrAbility) ? { ability: [roleOrAbility] } : { role: [roleOrAbility] };
      }
      // 字符串集合，每项可能是角色或是功能点，逐个处理每项
      if (Array.isArray(roleOrAbility) && roleOrAbility.length > 0 && typeof roleOrAbility[0] === 'string') {
        let abilities: string[] = [], roles: string[] = [];
        let type: ACLType = {};
        (roleOrAbility as string[]).forEach((val: string) => {
          if (isAbility(val)) abilities.push(val);
          else roles.push(val);
        });
        type.role = roles.length > 0 ? roles : null;
        type.ability = abilities.length > 0 ? abilities : null;
        return type;
      }
      return roleOrAbility;
    }
  } as DelonACLConfig;
}

import { STConfig } from '@delon/abc';
export function fnSTConfig(): STConfig {
  return {
    ...new STConfig(),
    ...{
      modal: { size: 'lg' }
    } as STConfig
  };
}

const GLOBAL_CONFIG_PROVIDES = [
  // TIPS：@delon/abc 有大量的全局配置信息，例如设置所有 `st` 的页码默认为 `20` 行
  { provide: STConfig, useFactory: fnSTConfig },
  { provide: PageHeaderConfig, useFactory: fnPageHeaderConfig },
  { provide: DelonAuthConfig, useFactory: fnDelonAuthConfig },
  { provide: DelonACLConfig, useFactory: fnDelonACLConfig },
  // { provide: ACLService, useClass: OsharpACLService }
];

// #endregion

@NgModule({
  imports: [
    AlainThemeModule.forRoot(),
    ...MOCK_MODULES,
  ],
})
export class DelonModule {
  constructor(@Optional() @SkipSelf() parentModule: DelonModule) {
    throwIfAlreadyLoaded(parentModule, 'DelonModule');
  }

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: DelonModule,
      providers: [...REUSETAB_PROVIDES, ...GLOBAL_CONFIG_PROVIDES],
    };
  }
}
