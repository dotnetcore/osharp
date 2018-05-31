import { NgModule, Component, } from '@angular/core';
import { SystemRoutingModule, } from './system.routing';
import { SettingsComponent } from './settings/settings.component';

const COMPONENTS = [
  SettingsComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [
    SystemRoutingModule,
  ],
  providers: [
  ]
})
export class SystemModule { }
