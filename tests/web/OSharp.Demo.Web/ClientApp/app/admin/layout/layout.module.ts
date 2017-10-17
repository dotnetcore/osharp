import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';

//app
import { LayoutRoutingModule } from "./layout.routing";
import { LayoutComponent } from './layout.component';
import { HeaderComponent } from "./header/header.component";
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
        TranslateModule,
        LayoutRoutingModule
    ],
    providers: [
        // TODO: and services
        // DemoService
    ]
})
export class LayoutModule { }
