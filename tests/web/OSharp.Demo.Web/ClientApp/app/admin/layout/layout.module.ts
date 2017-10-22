import { NgModule, } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { TranslateModule, } from '@ngx-translate/core';

//app
import { LayoutRoutingModule } from "./layout.routing";
import { LayoutComponent } from './layout.component';
import { HeaderComponent } from "./header/header.component";
import { NavsearchComponent } from "./header/navsearch/navsearch.component";
import { OffsidebarComponent } from './offsidebar/offsidebar.component';
import { SidebarComponent } from "./sidebar/sidebar.component";
import { UserblockComponent } from "./sidebar/userblock/userblock.component";
import { UserblockService } from './sidebar/userblock/userblock.service';
import { FooterComponent } from './footer/footer.component';

@NgModule({
    declarations: [
        LayoutComponent,
        HeaderComponent,
        NavsearchComponent,
        OffsidebarComponent,
        SidebarComponent,
        UserblockComponent,
        FooterComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        TranslateModule,
        LayoutRoutingModule
    ],
    providers: [
        UserblockService
    ]
})
export class LayoutModule { }
