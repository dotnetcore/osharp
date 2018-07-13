import { Component, OnInit } from '@angular/core';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { SettingsService } from '../../../../../../node_modules/@delon/theme';

@Component({
  selector: 'layout-user-menu',
  templateUrl: './user-menu.component.html',
  styles: []
})
export class UserMenuComponent {

  constructor(
    public identity: IdentityService,
    public settings: SettingsService
  ) { }
}
