import { Injectable, Inject } from '@angular/core';
import { DA_SERVICE_TOKEN, TokenService, JWTTokenModel } from '@delon/auth';
import { HttpClient } from '@angular/common/http';
import { SettingsService, User as NzUser } from '@delon/theme';
import { ACLService } from '@delon/acl';
import { LoginDto, AjaxResult, AjaxResultType, User, RegisterDto } from '@shared/osharp/osharp.model';

@Injectable()
export class IdentityService {
  constructor(
    private http: HttpClient,
    @Inject(DA_SERVICE_TOKEN) private tokenSrv: TokenService,
    private settingSrv: SettingsService,
    private aclSrv: ACLService
  ) { }

  login(dto: LoginDto): Promise<AjaxResult> {
    let url = "api/identity/jwtoken";
    return this.http.post<AjaxResult>(url, dto).map(result => {
      if (result.Type == AjaxResultType.Success) {
        // 设置Token
        this.tokenSrv.set({ token: result.Data });

        // 更新用户
        let user = new User(result.Data);
        let nzUser = { name: user.NickName, avatar: null, email: user.Email };
        this.settingSrv.setUser(nzUser);

        // 更新角色
        this.aclSrv.setRole(user.Roles);
      }
      return result;
    }).toPromise();
  }

  register(dto: RegisterDto): Promise<AjaxResult> {
    let url = 'api/identity/register';
    return this.http.post<AjaxResult>(url, dto).map(result => {
      return result;
    }).toPromise();
  }

  user(): User {
    let jwt = this.tokenSrv.get();
    if (!jwt || !jwt.token) {
      return null;
    }
    return new User(jwt.token);
  }
}
