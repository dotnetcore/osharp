import { Component, OnInit } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';
import { SettingsService, MenuService } from '@delon/theme';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent implements OnInit {
  constructor(
    public settings: SettingsService,
    public msgSrv: NzMessageService,
    private menuSrv: MenuService
  ) { }

  ngOnInit(): void {
    this.menuSrv.add([
      { text: '修改密码', link: '/admin/change-password', icon: 'antion antion-user' }
    ]);
  }
}
