import { Component, ChangeDetectionStrategy } from '@angular/core';
import { SettingsService, App } from '@delon/theme';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent {
  constructor(public settings: SettingsService) { }
}
