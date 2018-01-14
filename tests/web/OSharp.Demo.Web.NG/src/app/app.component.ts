import { Component, HostBinding, OnInit } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from "@angular/router";
import { Title } from "@angular/platform-browser";
import { Http } from '@angular/http';

import { osharp } from "./shared/osharp";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'app';

  constructor(private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title) {
  }

  ngOnInit() {
    this.SetTitleFromRouter();
  }

  /** 从路由设置页面的Title */
  private SetTitleFromRouter(name?: string) {
    name = name || "osharpns";
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .map(() => this.activatedRoute)
      .map(route => {
        while (route.firstChild) route = route.firstChild;
        return route;
      })
      .filter(route => route.outlet == 'primary')
      .mergeMap(route => route.data)
      .subscribe(event => this.titleService.setTitle(event['title'] ? event['title'] + ' | ' + name : name));
  }
}
