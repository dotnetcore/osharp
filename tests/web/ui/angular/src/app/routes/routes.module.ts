import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslatorService } from '../shared/angle/core/translator/translator.service';
import { MenuService } from '../shared/angle/core/menu/menu.service';
import { AngleModule } from '../shared/angle/angle.module';

import { menu } from './menu';
import { routes } from './routes';

@NgModule({
    imports: [
        AngleModule,
        RouterModule.forRoot(routes)
    ],
    declarations: [],
    exports: [
        RouterModule
    ]
})

export class RoutesModule {
    constructor(public menuService: MenuService, tr: TranslatorService) {
        menuService.addMenu(menu);
    }
}
