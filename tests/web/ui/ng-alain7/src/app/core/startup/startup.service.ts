import { Injectable, Injector, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { zip } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MenuService, SettingsService, TitleService, ALAIN_I18N_TOKEN } from '@delon/theme';
import { DA_SERVICE_TOKEN, ITokenService, JWTTokenModel } from '@delon/auth';
import { ACLService } from '@delon/acl';
import { TranslateService } from '@ngx-translate/core';
import { I18NService } from '../i18n/i18n.service';

import { NzIconService } from 'ng-zorro-antd';
import { ICONS_AUTO } from '../../../style-icons-auto';
import { ICONS } from '../../../style-icons';
import { ANT_ICONS } from "../../../ant-svg-icons";
import Heartbeats from 'heartbeats';
import { CacheService } from '@delon/cache';
import { TokenDto, AjaxResult, AjaxResultType, JsonWebToken } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';

/**
 * 用于应用启动时
 * 一般用来获取应用所需要的基础数据等
 */
@Injectable()
export class StartupService {

  private router: Router;

  constructor(
    iconSrv: NzIconService,
    private injector: Injector,
    private menuService: MenuService,
    private translate: TranslateService,
    @Inject(ALAIN_I18N_TOKEN) private i18n: I18NService,
    private settingService: SettingsService,
    private aclService: ACLService,
    private titleService: TitleService,
    @Inject(DA_SERVICE_TOKEN) private tokenSrv: ITokenService,
    private cache: CacheService,
    private httpClient: HttpClient,
  ) {
    iconSrv.addIcon(...ICONS_AUTO, ...ICONS, ...ANT_ICONS);
  }

  // 心跳业务
  private viaHeartbeats() {
    let heart = Heartbeats.createHeart(1000);
    heart.createEvent(5, (count, last) => {
      this.tryRefreshAccessToken();
    });
  }

  private tryRefreshAccessToken() {
    let at = this.tokenSrv.get<JWTTokenModel>(JWTTokenModel);
    if (at && at.token) {
      let diff = Math.round(at.payload.exp - new Date().getTime() / 1000);
      if (diff > 20) return;
    }
    let rt = this.cache.getNone<string>('refresh_token');
    if (!rt) {
      // 跳转到登录
      if (!this.router) {
        this.router = this.injector.get(Router);
      }
      let url = this.router.url;
      let loginUrl = '/passport/login';
      if (url !== loginUrl) {
        setTimeout(() => this.router.navigateByUrl(loginUrl));
      }
      return;
    }
    // 使用rt刷新at
    let dto: TokenDto = { RefreshToken: rt, GrantType: 'refresh_token' };
    this.httpClient.post<AjaxResult>('api/identity/token', dto).subscribe(result => {
      if (result.Type === AjaxResultType.Success) {
        let token = result.Data as JsonWebToken;
        if (token) {
          this.tokenSrv.set({ token: token.AccessToken });
          let seconds = new Date().getTime();
          seconds = Math.round((token.RefreshUctExpires - seconds) / 1000);
          if (seconds > 0) {
            this.cache.set('refresh_token', token.RefreshToken, { expire: seconds });
          }
        }
      }
    });
  }

  private viaHttp(resolve: any, reject: any) {
    zip(
      this.httpClient.get(`assets/osharp/i18n/${this.i18n.defaultLang}.json`),
      this.httpClient.get('assets/osharp/app-data.json'),
      this.httpClient.get('api/security/getauthinfo') // 获取当前用户的权限点string[]
    ).pipe(
      // 接收其他拦截器后产生的异常消息
      catchError(([langData, appData, authInfo]) => {
        resolve(null);
        return [langData, appData, authInfo];
      })
    ).subscribe(([langData, appData, authInfo]) => {
      // setting language data
      this.translate.setTranslation(this.i18n.defaultLang, langData);
      this.translate.setDefaultLang(this.i18n.defaultLang);

      // application data
      const res: any = appData;
      // 应用信息：包括站点名、描述、年份
      this.settingService.setApp(res.app);
      // 用户信息：包括姓名、头像、邮箱地址
      // this.settingService.setUser(res.user);
      // ACL：刷新权限数据
      this.aclService.setAbility(authInfo);
      // this.aclService.setFull(true);
      // 初始化菜单
      this.menuService.add(res.menu);
      // 设置页面标题的后缀
      this.titleService.suffix = res.app.name;
    },
      () => { },
      () => {
        resolve(null);
      });
  }

  load(): Promise<any> {
    // only works with promises
    // https://github.com/angular/angular/issues/15088
    return new Promise((resolve, reject) => {
      // http
      this.viaHttp(resolve, reject);

      // heartbeats
      this.viaHeartbeats();

    });
  }
}
