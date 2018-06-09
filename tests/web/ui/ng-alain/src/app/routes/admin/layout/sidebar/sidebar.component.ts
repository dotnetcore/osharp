import { Component } from '@angular/core';

import { SettingsService, MenuService } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent {

  constructor(
    public settings: SettingsService,
    public msgSrv: NzMessageService
  ) { }

}
