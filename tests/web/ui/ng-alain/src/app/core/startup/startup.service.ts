import { Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SettingsService, TitleService } from '@delon/theme';

/**
 * 用于应用启动时
 * 一般用来获取应用所需要的基础数据等
 */
@Injectable()
export class StartupService {
  constructor(
    private settingService: SettingsService,
    private titleService: TitleService,
    private httpClient: HttpClient,
  ) { }

  load(): Promise<any> {
    return this.httpClient.get('assets/osharp/app-data.json').map((data: any) => {
      if (!data) {
        return;
      }
      // 应用信息：包括站点名、描述、年份
      this.settingService.setApp(data.app);
      // 用户信息：包括姓名、头像、邮箱地址
      // this.settingService.setUser(data.user);
      // ACL：设置权限为全量
      // this.aclService.setFull(true);
      // 初始化菜单
      // this.menuService.add(data.menu);
      // 设置页面标题的后缀
      this.titleService.suffix = data.app.name;
    }).toPromise();
  }
}
