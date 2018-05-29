import { Component } from '@angular/core';

import { SettingsService, MenuService } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'admin-sidebar',
  templateUrl: './sidebar.component.html'
})
export class AdminSidebarComponent {

  constructor(
    public settings: SettingsService,
    public msgSrv: NzMessageService
  ) { }

}
