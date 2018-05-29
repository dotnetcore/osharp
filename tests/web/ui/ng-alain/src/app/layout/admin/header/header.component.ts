import { Component } from '@angular/core';
import { SettingsService } from '@delon/theme';

@Component({
  selector: 'admin-header',
  templateUrl: './header.component.html'
})
export class AdminHeaderComponent {
  searchToggleStatus: boolean;

  constructor(public settings: SettingsService) { }

  toggleCollapsedSideabar() {
    this.settings.setLayout('collapsed', !this.settings.layout.collapsed);
  }

  searchToggleChange() {
    this.searchToggleStatus = !this.searchToggleStatus;
  }
}
