import { Component } from '@angular/core';

import { SettingsService, User } from '@delon/theme';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent {

  user: User;
  inAdminModule: boolean;

  constructor(
    settings: SettingsService
  ) {
    this.user = settings.user || {};
  }

}
