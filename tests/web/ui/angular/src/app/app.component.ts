import { Component, HostBinding, OnInit } from '@angular/core';
import { Http } from '@angular/http';

import { SettingsService } from './shared/angle/core/settings/settings.service';
import { OsharpService } from './shared/osharp/osharp.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private http: HttpClient, private osharp: OsharpService, public settings: SettingsService) { }

  ngOnInit() {
    this.osharp.SetTitleFromRouter();
    $(document).on('click', '[href="#"]', e => e.preventDefault());
  }
}
