import { Component, OnInit, AfterViewInit } from '@angular/core';
import { SettingsService } from '@delon/theme';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.less']
})
export class ProfileComponent implements AfterViewInit {

  index = 0;
  constructor(
    public settings: SettingsService
  ) { }

  ngAfterViewInit(): void {
    let hash = window.location.hash;
    console.log(hash);
  }
}
