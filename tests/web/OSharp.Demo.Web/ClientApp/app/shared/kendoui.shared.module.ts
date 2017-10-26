import { NgModule, ModuleWithProviders } from '@angular/core';

//kendoui
import { GridModule as KendoGridModule } from '@progress/kendo-angular-grid';
import { ChartsModule as KendoChartsModule } from '@progress/kendo-angular-charts';
import { DialogModule as KendoDialogModule } from '@progress/kendo-angular-dialog';
import { InputsModule as KendoInputsModule } from '@progress/kendo-angular-inputs';
import { ButtonsModule as KendoButtonsModule } from '@progress/kendo-angular-buttons';
import { LayoutModule as KendoLayoutModule } from '@progress/kendo-angular-layout';
import { RippleModule as KendoRippleModule } from '@progress/kendo-angular-ripple';

@NgModule({
    declarations: [],
    imports: [
        KendoGridModule,
        KendoChartsModule,
        KendoDialogModule,
        KendoInputsModule,
        KendoButtonsModule,
        KendoLayoutModule,
        KendoRippleModule
    ],
    exports: [
        KendoGridModule,
        KendoChartsModule,
        KendoDialogModule,
        KendoInputsModule,
        KendoButtonsModule,
        KendoLayoutModule,
        KendoRippleModule
    ],
    providers: []
})
export class KendouiSharedModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: KendouiSharedModule,
            providers: []
        }
    }
}