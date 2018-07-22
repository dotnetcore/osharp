import { Injectable, Inject } from '@angular/core';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { HttpClient } from '@angular/common/http';
import { SettingsService } from '@delon/theme';
import { ACLService } from '@delon/acl';
import { LoginDto, AjaxResult, AjaxResultType, RegisterDto, ConfirmEmailDto, User, SendMailDto, AdResult, ResetPasswordDto } from '@shared/osharp/osharp.model';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { Observable } from '../../../../../node_modules/rxjs';

@Injectable()
export class IdentityService {

  constructor(
    private http: HttpClient,
    private osharp: OsharpService,
    @Inject(DA_SERVICE_TOKEN) private tokenSrv: ITokenService,
    private settingSrv: SettingsService,
    private aclSrv: ACLService
  ) { }

  login(dto: LoginDto): Promise<AjaxResult> {
    let url = "api/identity/jwtoken";
    return this.http.post<AjaxResult>(url, dto).map(result => {
      if (result.Type == AjaxResultType.Success) {
        // 设置Token
        this.tokenSrv.set({ token: result.Data });
        // 刷新用户信息
        this.refreshUser().subscribe();
      }
      return result;
    }).toPromise();
  }

  logout() {
    let url = "api/identity/logout";
    return this.http.post<AjaxResult>(url, {}).map(res => {
      if (res.Type == AjaxResultType.Success) {
        //清除Token
        this.tokenSrv.clear();
        // 刷新用户信息
        this.refreshUser().subscribe();
      }
      return res;
    }).toPromise();
  }

  register(dto: RegisterDto): Promise<AdResult> {
    let url = 'api/identity/register';
    return this.http.post<AjaxResult>(url, dto).map(res => {
      let result = new AdResult();
      if (res.Type == AjaxResultType.Success) {
        result.type = "success";
        result.title = "新用户注册成功";
        result.description = `你的账户：${dto.UserName}[${dto.NickName}] 注册成功，请及时登录邮箱 ${dto.Email} 接收邮件激活账户。`;
        return result;
      }
      result.type = 'error';
      result.title = "用户注册失败";
      result.description = res.Content;
      return result;
    }).toPromise();
  }

  /** 刷新用户信息 */
  refreshUser(): Observable<User> {
    let url = "api/identity/profile";
    console.log(url);
    return this.http.get(url).map((res: any) => {
      if (!res || res == {}) {
        this.settingSrv.setUser({});
        this.aclSrv.setRole([]);
        // 更新权限
        this.osharp.refreshAuthInfo();
        return {};
      }
      let user: User = {
        id: res.Id, name: res.UserName, nickName: res.NickName, avatar: res.HeadImg, email: res.Email, roles: res.Roles, isAdmin: res.IsAdmin
      };
      console.log(user);
      this.settingSrv.setUser(user);
      // 更新角色
      this.aclSrv.setRole(user.roles);
      // 更新权限
      this.osharp.refreshAuthInfo();
      return user;
    });
  }

  sendConfirmMail(dto: SendMailDto): Promise<AdResult> {
    let url = 'api/identity/SendConfirmMail';
    return this.http.post<AjaxResult>(url, dto).map(res => {
      let result = new AdResult();
      if (res.Type != AjaxResultType.Success) {
        result.type = 'error';
        result.title = '重发激活邮件失败';
        result.description = res.Content;
        return result;
      }
      result.type = "success";
      result.title = '重发激活邮件成功';
      result.description = `注册邮箱激活邮件发送成功，请登录邮箱“${dto.Email}”收取邮件进行后续步骤`;
      return result;
    }).toPromise();
  }

  confirmEmail(dto: ConfirmEmailDto): Promise<AdResult> {
    let url = 'api/identity/ConfirmEmail';
    return this.http.post<AjaxResult>(url, dto).map(res => {
      let result = new AdResult();
      if (res.Type != AjaxResultType.Success) {
        result.type = "error";
        result.title = "注册邮箱激活失败";
        if (res.Type == AjaxResultType.Info) {
          result.type = 'minus-circle-o';
        }
        result.title = "注册邮箱激活取消";
        result.description = res.Content;
        return result;
      }
      result.type = "success";
      result.title = "注册邮箱激活成功";
      result.description = res.Content;
      return result;
    }).toPromise();
  }

  sendResetPasswordMail(dto: SendMailDto): Promise<AdResult> {
    let url = 'api/identity/SendResetPasswordMail';
    return this.http.post<AjaxResult>(url, dto).map(res => {
      let result = new AdResult();
      if (res.Type != AjaxResultType.Success) {
        result.type = 'error';
        result.title = '重置密码邮件发送失败';
        result.description = res.Content;
        return result;
      }
      result.type = 'success';
      result.title = '重置密码邮件发送成功';
      result.description = `重置密码邮件发送成功，请登录邮箱“${dto.Email}”收取邮件进行后续步骤`;
      return result;
    }).toPromise();
  }

  resetPassword(dto: ResetPasswordDto): Promise<AdResult> {
    let url = 'api/identity/ResetPassword';
    return this.http.post<AjaxResult>(url, dto).map(res => {
      let result = new AdResult();
      if (res.Type != AjaxResultType.Success) {
        result.type = 'error';
        result.title = '登录密码重置失败';
        result.description = res.Content;
        return result;
      }
      result.type = "success";
      result.title = '登录密码重置成功';
      result.description = "登录密码重置成功，请使用新密码登录系统。";
      return result;
    }).toPromise();
  }
}
