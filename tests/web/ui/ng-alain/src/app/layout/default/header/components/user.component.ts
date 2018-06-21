import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SettingsService } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';
import { AjaxResultType } from '@shared/osharp/osharp.model';
import { IdentityService } from '@shared/osharp/services/identity.service';

@Component({
  selector: 'header-user',
  template: `
  <nz-dropdown nzPlacement="bottomRight">
    <div class="item d-flex align-items-center px-sm" nz-dropdown>
      <nz-avatar [nzSrc]="settings.user.avatar" nzSize="small" class="mr-sm"></nz-avatar>
      {{settings.user.name}}
    </div>
    <div nz-menu class="width-sm">
      <div nz-menu-item [nzDisabled]="true"><i class="anticon anticon-user mr-sm"></i>个人中心</div>
      <div nz-menu-item [nzDisabled]="true"><i class="anticon anticon-setting mr-sm"></i>设置</div>
      <li nz-menu-divider></li>
      <div nz-menu-item (click)="logout()"><i class="anticon anticon-logout mr-sm"></i>退出登录</div>
    </div>
  </nz-dropdown>
  `,
})
export class HeaderUserComponent {
  constructor(
    public settings: SettingsService,
    private msgSrv: NzMessageService,
    private identity: IdentityService,
    private router: Router
  ) { }

  logout() {

    this.identity.logout().then(res => {
      if (res.Type == AjaxResultType.Success) {
        this.msgSrv.success("用户退出成功");
        let url = this.router.url || "/home";
        this.router.navigateByUrl(url);
        return;
      }
      this.msgSrv.error(`用户登出失败：${res.Content}`);
    });
  }
}
