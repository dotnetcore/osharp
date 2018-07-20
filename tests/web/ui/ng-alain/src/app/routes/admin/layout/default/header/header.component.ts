import { Component, } from '@angular/core';
import { SettingsService, User, App, Layout } from '@delon/theme';
import { Router } from '@angular/router';

@Component({
  selector: 'layout-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  searchToggleStatus: boolean;
  app: App;

  constructor(
    public settings: SettingsService,
    router: Router
  ) {
    this.app = settings.app;
  }

  get user() {
    return this.settings.user;
  }

  toggleCollapsedSideabar() {
    this.settings.setLayout('collapsed', !this.settings.layout.collapsed);
  }

  searchToggleChange() {
    this.searchToggleStatus = !this.searchToggleStatus;
  }
}
