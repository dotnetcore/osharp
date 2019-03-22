import { Component, OnInit, Inject } from '@angular/core';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { UserLoginInfoEx, AjaxResultType } from '@shared/osharp/osharp.model';
import { SFSchema, CustomWidget } from '@delon/form';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { NzMessageService } from 'ng-zorro-antd';
import { StartupService } from '@core';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-oauth-callback',
  templateUrl: './oauth-callback.component.html',
  styles: [`
  :host {
    display: block;
    width: 400px;
    margin: 0 auto;
  `]
})
export class OauthCallbackComponent implements OnInit {

  error = '';
  loginInfo: UserLoginInfoEx;
  bindSchema: SFSchema;
  oneKeySchema: SFSchema;

  constructor(
    private router: Router,
    private osharp: OsharpService,
    private identity: IdentityService,
    private message: NzMessageService,
    private startupSrv: StartupService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService) { }

  ngOnInit() {
    this.getUrlParams();
    if (!this.loginInfo) {
      return;
    }
    this.oneKeySchema = {
      properties: {
        LoginProvider: { title: '登录方式', type: 'string', default: this.loginInfo.LoginProvider, ui: { widget: 'text', size: 'large', spanLabel: 6, spanControl: 16, grid: { xs: 24 } } },
        ProviderKey: { type: 'string', default: this.loginInfo.ProviderKey, ui: { hidden: true } },
        ProviderDisplayName: { title: '昵称', type: 'string', default: this.loginInfo.ProviderDisplayName, ui: { widget: 'text', size: 'large', spanLabel: 6, spanControl: 16, grid: { xs: 24 } } },
        AvatarUrl: { type: 'string', default: this.loginInfo.AvatarUrl, ui: { hidden: true } }
      },
      ui: { grid: { gutter: 16, xs: 24 } }
    };
    this.bindSchema = {
      properties: {
        Email: { title: '邮箱', type: 'string', ui: { placeholder: '登录账号：用户名/邮箱/手机号', prefixIcon: 'mail', size: 'large', spanLabel: 6, spanControl: 16, grid: { xs: 24 } } },
        Password: { title: '登录密码', type: 'string', ui: { widget: 'custom', spanLabel: 6, spanControl: 16, grid: { xs: 24 } } },
        ...this.oneKeySchema.properties
      },
      required: ['Email', 'Password'],
      ui: { grid: { gutter: 16, xs: 24 } }
    };
  }

  private getUrlParams() {
    let url = window.location.hash;
    let json = this.osharp.getHashURLSearchParams(url, 'data');
    if (json) {
      this.loginInfo = JSON.parse(json);
    }
    let token = this.osharp.getHashURLSearchParams(url, 'token');
    if (token) {
      this.identity.loginEnd(token);
      this.startupSrv.load().then(() => {
        url = this.tokenService.referrer && this.tokenService.referrer.url || '/';
        if (url.includes('/passport')) url = '/';
        this.router.navigateByUrl(url);
      });
    }
  }

  passwordChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }

  loginBind(value: UserLoginInfoEx) {
    this.identity.loginBind(value).then(result => {
      if (result.Type === AjaxResultType.Success) {
        this.message.success("用户登录成功");
        this.startupSrv.load().then(() => {
          let url = this.tokenService.referrer && this.tokenService.referrer.url || '/';
          if (url.includes('/passport')) url = '/';
          this.router.navigateByUrl(url);
        });
        return;
      }
      this.error = `登录失败：${result.Content}`;
    });
  }

  loginOneKey(value: UserLoginInfoEx) {
    this.identity.loginOneKey(value).then(result => {
      if (result.Type === AjaxResultType.Success) {
        this.message.success("用户登录成功");
        this.startupSrv.load().then(() => {
          let url = this.tokenService.referrer && this.tokenService.referrer.url || '/';
          if (url.includes('/passport')) url = '/';
          this.router.navigateByUrl(url);
        });
        return;
      }
      this.error = `登录失败：${result.Content}`;
    });
  }
}
