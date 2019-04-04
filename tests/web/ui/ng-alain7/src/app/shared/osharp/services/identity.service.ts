import { Injectable, Inject } from '@angular/core';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { SettingsService, _HttpClient } from '@delon/theme';
import { ACLService } from '@delon/acl';
import {
  LoginDto,
  AjaxResult,
  AjaxResultType,
  RegisterDto,
  ConfirmEmailDto,
  User,
  SendMailDto,
  AdResult,
  ResetPasswordDto,
  UserLoginInfoEx,
  ChangePasswordDto,
  ProfileEditDto,
} from '@shared/osharp/osharp.model';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { Observable, of } from 'rxjs';
import { ReuseTabService } from '@delon/abc';

@Injectable({
  providedIn: 'root',
})
export class IdentityService {
  constructor(
    public http: _HttpClient,
    private osharp: OsharpService,
    @Inject(DA_SERVICE_TOKEN) private tokenSrv: ITokenService,
    private settingSrv: SettingsService,
    private aclSrv: ACLService,
    private reuseTabSrv: ReuseTabService,
  ) {}

  get user() {
    return this.settingSrv.user;
  }

  login(dto: LoginDto): Promise<AjaxResult> {
    let url = 'api/identity/jwtoken';
    return this.http
      .post<AjaxResult>(url, dto)
      .map(result => {
        if (result.Type === AjaxResultType.Success) {
          this.loginEnd(result.Data).subscribe();
        }
        return result;
      })
      .toPromise();
  }

  logout() {
    let url = 'api/identity/logout';
    return this.http
      .post<AjaxResult>(url, {})
      .map(res => {
        if (res.Type === AjaxResultType.Success) {
          this.loginEnd(null).subscribe();
        }
        return res;
      })
      .toPromise();
  }

  register(dto: RegisterDto): Promise<AdResult> {
    let url = 'api/identity/register';
    return this.http
      .post<AjaxResult>(url, dto)
      .map(res => {
        let result = new AdResult();
        if (res.Type === AjaxResultType.Success) {
          let data: any = res.Data;
          result.type = 'success';
          result.title = '新用户注册成功';
          result.description = `你的账户：${data.UserName}[${
            data.NickName
          }] 注册成功，请及时登录邮箱 ${data.Email} 接收邮件激活账户。`;
          return result;
        }
        result.type = 'error';
        result.title = '用户注册失败';
        result.description = res.Content;
        return result;
      })
      .toPromise();
  }

  loginBind(info: UserLoginInfoEx) {
    let url = 'api/identity/LoginBind';
    return this.http
      .post<AjaxResult>(url, info)
      .map(result => {
        if (result.Type === AjaxResultType.Success) {
          this.loginEnd(result.Data).subscribe();
        }
        return result;
      })
      .toPromise();
  }

  loginOneKey(info: UserLoginInfoEx) {
    let url = 'api/identity/LoginOneKey';
    return this.http
      .post<AjaxResult>(url, info)
      .map(result => {
        if (result.Type === AjaxResultType.Success) {
          this.loginEnd(result.Data).subscribe();
        }
        return result;
      })
      .toPromise();
  }

  loginEnd(token: string) {
    // 清空路由复用信息
    this.reuseTabSrv.clear();
    // 设置Token
    if (token) {
      this.tokenSrv.set({ token });
    } else {
      this.tokenSrv.clear();
      this.settingSrv.setUser({});
    }
    // 刷新用户信息
    return this.refreshUser();
  }

  removeOAuth2(id: string) {
    let url = 'api/identity/RemoveOAuth2';
    return this.http
      .post<AjaxResult>(url, [id])
      .map(res => {
        this.osharp.ajaxResult(res);
        return res;
      })
      .toPromise();
  }

  /** 刷新用户信息 */
  refreshUser(): Observable<User> {
    let url = 'api/identity/profile';
    return this.http.get(url).map((res: any) => {
      if (!res || res === {}) {
        this.settingSrv.setUser({});
        this.aclSrv.setRole([]);
        return {};
      }
      let user: User = {
        id: res.Id,
        name: res.UserName,
        nickName: res.NickName,
        avatar: res.HeadImg,
        email: res.Email,
        roles: res.Roles,
        isAdmin: res.IsAdmin,
      };
      this.settingSrv.setUser(user);
      // 更新角色
      this.aclSrv.setRole(user.roles);
      return user;
    });
  }

  sendConfirmMail(dto: SendMailDto): Promise<AdResult> {
    let url = 'api/identity/SendConfirmMail';
    return this.http
      .post<AjaxResult>(url, dto)
      .map(res => {
        let result = new AdResult();
        if (res.Type !== AjaxResultType.Success) {
          result.type = 'error';
          result.title = '重发激活邮件失败';
          result.description = res.Content;
          return result;
        }
        result.type = 'success';
        result.title = '重发激活邮件成功';
        result.description = `注册邮箱激活邮件发送成功，请登录邮箱“${
          dto.Email
        }”收取邮件进行后续步骤`;
        return result;
      })
      .toPromise();
  }

  confirmEmail(dto: ConfirmEmailDto): Promise<AdResult> {
    let url = 'api/identity/ConfirmEmail';
    return this.http
      .post<AjaxResult>(url, dto)
      .map(res => {
        let result = new AdResult();
        if (res.Type != AjaxResultType.Success) {
          result.type = 'error';
          result.title = '注册邮箱激活失败';
          if (res.Type == AjaxResultType.Info) {
            result.type = 'minus-circle-o';
          }
          result.title = '注册邮箱激活取消';
          result.description = res.Content;
          return result;
        }
        result.type = 'success';
        result.title = '注册邮箱激活成功';
        result.description = res.Content;
        return result;
      })
      .toPromise();
  }

  sendResetPasswordMail(dto: SendMailDto): Promise<AdResult> {
    let url = 'api/identity/SendResetPasswordMail';
    return this.http
      .post<AjaxResult>(url, dto)
      .map(res => {
        let result = new AdResult();
        if (res.Type !== AjaxResultType.Success) {
          result.type = 'error';
          result.title = '重置密码邮件发送失败';
          result.description = res.Content;
          return result;
        }
        result.type = 'success';
        result.title = '重置密码邮件发送成功';
        result.description = `重置密码邮件发送成功，请登录邮箱“${
          dto.Email
        }”收取邮件进行后续步骤`;
        return result;
      })
      .toPromise();
  }

  resetPassword(dto: ResetPasswordDto): Promise<AdResult> {
    let url = 'api/identity/ResetPassword';
    return this.http
      .post<AjaxResult>(url, dto)
      .map(res => {
        let result = new AdResult();
        if (res.Type !== AjaxResultType.Success) {
          result.type = 'error';
          result.title = '登录密码重置失败';
          result.description = res.Content;
          return result;
        }
        result.type = 'success';
        result.title = '登录密码重置成功';
        result.description = '登录密码重置成功，请使用新密码登录系统。';
        return result;
      })
      .toPromise();
  }

  profileEdit(dto: ProfileEditDto) {
    let url = 'api/identity/ProfileEdit';
    return this.http.post<AjaxResult>(url, dto).subscribe(res => {
      this.osharp.ajaxResult(res);
      this.refreshUser().subscribe();
    });
  }

  changePassword(dto: ChangePasswordDto) {
    let url = 'api/identity/ChangePassword';
    return this.http.post<AjaxResult>(url, dto).subscribe(res => {
      this.osharp.ajaxResult(res);
    });
  }
}
