import { Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SettingsService, TitleService } from '@delon/theme';
import { Observable } from 'rxjs';
import { User } from '@shared/osharp/osharp.model';
import { ACLService } from '@delon/acl';
import { OsharpService } from '@shared/osharp/services/osharp.service';

/**
 * 用于应用启动时
 * 一般用来获取应用所需要的基础数据等
 */
@Injectable()
export class StartupService {
  constructor(
    private settingSrv: SettingsService,
    private aclSrv: ACLService,
    private titleService: TitleService,
    private http: HttpClient,
  ) { }

  load(): Promise<any> {
    return this.http.get('assets/osharp/app-data.json').map((data: any) => {
      if (!data) {
        return;
      }
      // 应用信息：包括站点名、描述、年份
      this.settingSrv.setApp(data.app);
      // 初始化菜单
      // this.menuService.add(data.menu);
      // 设置页面标题的后缀
      this.titleService.suffix = data.app.name;
      // 刷新用户信息
      this.refreshUser().subscribe();
    }).toPromise();
  }

  /** 刷新用户信息 */
  private refreshUser(): Observable<User> {
    let url = "api/identity/profile";
    return this.http.get(url).map((res: any) => {
      if (!res || res == {}) {
        this.settingSrv.setUser({});
        this.aclSrv.setRole([]);
        return {};
      }
      let user: User = {
        id: res.Id, name: res.UserName, nickName: res.NickName, avatar: res.HeadImg, email: res.Email, roles: res.Roles, isAdmin: res.IsAdmin
      };
      this.settingSrv.setUser(user);
      // 更新角色
      this.aclSrv.setRole(user.roles);
      return user;
    });
  }
}
