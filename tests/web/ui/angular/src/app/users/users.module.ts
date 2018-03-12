import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';

import { UsersComponent } from './users.component';
import { UsersRoutingModule, } from './users.routing';

@NgModule({
    declarations: [
        UsersComponent,
    ],
    imports: [
        TranslateModule,
        UsersRoutingModule,
    ],
    providers: [
    ]
})
export class UsersModule { }
