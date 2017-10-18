import { NgModule, } from '@angular/core';
import { CommonModule } from "@angular/common";
import { TranslateModule, } from '@ngx-translate/core';

//app
import { LayoutRoutingModule } from "./layout.routing";
import { LayoutComponent } from './layout.component';
import { HeaderComponent } from "./header/header.component";
import { NavsearchComponent } from "./header/navsearch/navsearch.component";
import { OffsidebarComponent } from './offsidebar/offsidebar.component';
import { SidebarComponent } from "./sidebar/sidebar.component";
import { FooterComponent } from './footer/footer.component';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
    declarations: [
        LayoutComponent,
        HeaderComponent,
        OffsidebarComponent,
        SidebarComponent,
        FooterComponent,
        // TODO: add components
        // DemoComponent
    ],
    imports: [
        CommonModule,
        TranslateModule,
        LayoutRoutingModule
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class LayoutModule { }
