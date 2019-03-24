import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { SettingsService } from '@delon/theme';
import { Router, ActivationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.less']
})
export class ProfileComponent implements OnInit, OnDestroy {

  private router$: Subscription;
  pos = 0;
  tabs: any[] = [
    { key: 'edit', text: '基本信息' },
    { key: 'password', text: '修改密码' },
    { key: 'oauth2', text: '第三方账号绑定' },
  ];

  constructor(
    private router: Router,
    public settings: SettingsService
  ) { }

  ngOnInit(): void {
    this.router$ = this.router.events.pipe(filter(e => e instanceof ActivationEnd))
      .subscribe(() => this.setActive());
    this.setActive();
  }

  private setActive() {
    let key = this.router.url.substr(this.router.url.lastIndexOf('/') + 1);
    let idx = this.tabs.findIndex(m => m.key === key);
    if (idx !== -1) this.pos = idx;
  }

  to(item: any) {
    this.router.navigateByUrl(`/profile/${item.key}`);
  }

  ngOnDestroy(): void {
    this.router$.unsubscribe();
  }
}
