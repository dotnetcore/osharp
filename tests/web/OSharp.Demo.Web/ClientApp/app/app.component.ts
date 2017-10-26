import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from "@angular/router";
import { Title } from "@angular/platform-browser";
import { Http } from '@angular/http';

import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';

import { OsharpService } from './shared/osharp/osharp.service';
import { OsharpSettingsService } from './shared/osharp/osharp.settings.service';
import { LoggingService } from './shared/services/logging.services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  apiValues: string[];

  constructor(private http: Http, private osharp: OsharpService) {
    osharp.settings.title = "osharp demo";
  };

  ngOnInit() {
    this.osharp.SetTitleFromRouter();

    this.http.get('/api/values').subscribe(values => {
      this.apiValues = values.json() as string[];
    });
  }
}
