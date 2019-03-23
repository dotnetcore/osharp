import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { ProfileRoutingModule } from './profile.routing';
import { ProfileSettingsComponent } from './settings/settings.component';
import { ProfileComponent } from './profile.component';
import { ProfileEditComponent } from './edit/edit.component';
import { ProfilePasswordComponent } from './password/password.component';
import { ProfileOauth2Component } from './oauth2/oauth2.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    ProfileRoutingModule,
  ],
  declarations: [
    ProfileComponent,
    ProfileEditComponent,
    ProfilePasswordComponent,
    ProfileSettingsComponent,
    ProfileOauth2Component
  ]
})
export class ProfileModule { }
