import { NgModule, } from '@angular/core';
import { TranslateModule, } from '@ngx-translate/core';

import { OsharpService } from './osharp.service';
import { OsharpSettingsService } from './osharp.settings.service';

// TODO: import components and services
// import { DemoComponent } from './demo/demo.component';
// import { DemoService } from './demo/demo.service';

@NgModule({
    declarations: [
    ],
    imports: [
        TranslateModule,
    ],
    providers: [
        OsharpService,
        OsharpSettingsService
    ]
})
export class OsharpModule { }

/*
请到 app.routing.ts 中添加如下路由（放在 { path: '**', redirectTo: 'layout/optimus-prime' } 之前）：

{
    path: 'osharp',
    loadChildren: './osharp/osharp.module#OsharpModule',
    canActivate: [AuthGuard]
},

 */
