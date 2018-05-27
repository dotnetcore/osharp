import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';


import { HomeComponent } from './home.component';
import { HomeRoutingModule, } from './home.routing';
import { SharedModule } from '../shared/shared.module';

@NgModule({
    declarations: [
        HomeComponent,
    ],
    imports: [
        TranslateModule,
        HomeRoutingModule,
        SharedModule
    ],
    providers: [

    ]
})
export class HomeModule { }
