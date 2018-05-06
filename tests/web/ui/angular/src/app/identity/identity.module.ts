import { NgModule, } from '@angular/core';
import { IdentityComponent } from './identity.component';
import { IdentityRoutingModule, } from './identity.routing';
import { TranslateModule, } from '@ngx-translate/core';
import { CustomFormsModule } from "ng2-validation";
import { SharedModule } from "../shared/shared.module";

import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
  declarations: [
    IdentityComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    TranslateModule,
    IdentityRoutingModule,
    SharedModule,
    CustomFormsModule
  ],
  providers: [
  ]
})
export class IdentityModule { }
