import { Component } from '@angular/core';

import { SettingsService, User } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent {

  user: User;
  inAdminModule: boolean;

  constructor(
    settings: SettingsService,
    public msgSrv: NzMessageService
  ) {
    this.user = settings.user || {};
  }

}
