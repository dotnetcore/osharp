import { Component, ChangeDetectionStrategy, Inject } from '@angular/core';
import { SettingsService } from '@delon/theme';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { TokenService, DA_SERVICE_TOKEN } from '@delon/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SidebarComponent {
  constructor(
    public settings: SettingsService,
    private identity: IdentityService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: TokenService,
    private router: Router) { }

  logout() {
    this.identity.logout().then(res => {
      this.tokenService.clear();
      this.router.navigateByUrl(this.tokenService.login_url);
    });
  }
}
