import { Component, } from '@angular/core';
import { Router, RouteConfigLoadStart, NavigationError, NavigationEnd } from '@angular/router';
import { ScrollService, SettingsService, App } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'layout-default', templateUrl: './default.component.html', styleUrls: ['./default.component.scss']
}

) export class LayoutDefaultComponent {
  isFetching = false;
  mainNavs = [
    { text: "首页", icon: "home", link: '/#/home' },
    { text: "特性", icon: "ac_unit", link: '/#/', disabled: true },
    { text: "组件", icon: "widgets", link: '/#/', disabled: true },
    { text: "博客", icon: "assignment_ind", link: 'http://www.cnblogs.com/guomingfeng/category/1243352.html', target: '_blank' }
  ];
  mainActions = [];
  constructor(
    router: Router,
    scroll: ScrollService,
    public settings: SettingsService,
    _message: NzMessageService,
  ) {
    router.events.subscribe(evt => {
      if (!this.isFetching && evt instanceof RouteConfigLoadStart) {
        this.isFetching = true;
      }
      if (evt instanceof NavigationError) {
        this.isFetching = false;
        _message.error(`无法加载${evt.url}路由`, { nzDuration: 1000 * 3 });
        return;
      }
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      setTimeout(() => {
        scroll.scrollToTop();
        this.isFetching = false;
      }, 100);
    });
  }
}
