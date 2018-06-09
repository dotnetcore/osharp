import { Component, OnInit } from '@angular/core';
import { SettingsService, User, ScrollService } from '@delon/theme';
import { Router, RouteConfigLoadStart, NavigationError, NavigationEnd } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'layout-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.css']
})
export class LayoutDefaultComponent implements OnInit {
  user: User;

  constructor(
    private setting: SettingsService
  ) { }

  ngOnInit(): void {
    this.user = this.setting.user;
  }
}
