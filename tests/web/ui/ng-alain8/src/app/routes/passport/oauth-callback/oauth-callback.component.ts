import { Component, OnInit, Inject } from '@angular/core';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { UserLoginInfoEx, AjaxResultType, JsonWebToken } from '@shared/osharp/osharp.model';
import { SFSchema, CustomWidget } from '@delon/form';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { NzMessageService } from 'ng-zorro-antd';
import { StartupService } from '@core';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'passport-oauth-callback',
  templateUrl: './oauth-callback.component.html',
  styles: [`
  :host {
    display: block;
    width: 450px;
    margin: 0 auto;
  `]
})
export class OauthCallbackComponent implements OnInit {

  error = '';
  provider: string;
  name: string;
  avatar: string;
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
        ProviderKey: { type: 'string', default: this.loginInfo.ProviderKey, ui: { hidden: true } }
      },
      ui: { grid: { gutter: 16, xs: 24 } }
    };
    this.bindSchema = {
      properties: {
        Account: { title: '账号', type: 'string', ui: { placeholder: '用户名/邮箱/手机号', prefixIcon: 'user', size: 'large', spanLabel: 6, spanControl: 16, grid: { xs: 24 } } },
        Password: { title: '密码', type: 'string', ui: { widget: 'custom', spanLabel: 6, spanControl: 16, grid: { xs: 24 } } },
        ...this.oneKeySchema.properties
      },
      required: ['Account', 'Password'],
      ui: { grid: { gutter: 16, xs: 24 } }
    };
  }

  private getUrlParams() {
    let url = window.location.hash;

    let id = this.osharp.getHashURLSearchParams(url, 'id');
    if (id) {
      this.provider = this.osharp.getHashURLSearchParams(url, 'type');
      this.name = this.osharp.getHashURLSearchParams(url, 'name');
      this.avatar = this.osharp.getHashURLSearchParams(url, 'avatar');
      this.loginInfo = new UserLoginInfoEx(id);
      return;
    }

    let token = this.osharp.getHashURLSearchParams(url, 'token');
    if (token) {
      let jwt: JsonWebToken = JSON.parse(token) as JsonWebToken;
      this.identity.loginEnd(jwt).subscribe(() => {
        this.message.success("第三方登录成功");
        this.startupSrv.load().then(() => {
          url = this.tokenService.referrer && this.tokenService.referrer.url || '/';
          if (url.includes('/passport')) url = '/';
          this.router.navigateByUrl(url);
        });
      });
    }
  }

  passwordChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }

  loginBind(value: UserLoginInfoEx) {
    this.identity.loginBind(value).then(result => {
      if (result.Type === AjaxResultType.Success) {
        this.message.success("登录并绑定成功");
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
        this.message.success("创建用户并登录成功");
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
