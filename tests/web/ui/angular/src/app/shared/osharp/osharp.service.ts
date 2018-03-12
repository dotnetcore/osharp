import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { OsharpSettingsService } from './osharp.settings.service';

@Injectable()
export class OsharpService {

    constructor(private router: Router,
        private activatedRoute: ActivatedRoute,
        private titleService: Title,
        public settings: OsharpSettingsService) { }

    /** 从路由设置页面的Title */
    SetTitleFromRouter(name?: string) {
        name = name || this.settings.title;
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