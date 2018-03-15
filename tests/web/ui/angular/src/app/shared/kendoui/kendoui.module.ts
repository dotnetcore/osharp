import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { KendoGridDirective } from './directives/kendoGrid.directive';


@NgModule({
    declarations: [KendoGridDirective],
    imports: [CommonModule,],
    exports: [KendoGridDirective],
    providers: [],
})
export class KendouiModule { }