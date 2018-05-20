import { Component, HostBinding, OnInit } from '@angular/core';

import { SettingsService } from './shared/angle/core/settings/settings.service';
import { OsharpService } from './shared/osharp/services/osharp.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private osharp: OsharpService) { }

  ngOnInit() {
    this.osharp.SetTitleFromRouter();
    $(document).on('click', '[href="#"]', e => e.preventDefault());
  }
}
