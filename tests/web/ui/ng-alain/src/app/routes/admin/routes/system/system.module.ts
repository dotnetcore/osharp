import { NgModule, Component, } from '@angular/core';
import { SystemRoutingModule, } from './system.routing';
import { SettingsComponent } from './settings/settings.component';

const Components = [
  SettingsComponent
];

@NgModule({
  declarations: [
    ...Components
  ],
  imports: [
    SystemRoutingModule,
  ],
  providers: [
    // TODO: and services
    // DemoService
  ]
})
export class SystemModule { }
