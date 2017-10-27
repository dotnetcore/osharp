import { NgModule, ModuleWithProviders } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

//kendoui
import { GridModule } from '@progress/kendo-angular-grid';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { LayoutModule } from '@progress/kendo-angular-layout';
import { RippleModule } from '@progress/kendo-angular-ripple';


@NgModule({
    declarations: [],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        GridModule,
        ChartsModule,
        DialogModule,
        InputsModule,
        ButtonsModule,
        LayoutModule,
        RippleModule,
    ],
    exports: [
        BrowserModule,
        BrowserAnimationsModule,
        GridModule,
        ChartsModule,
        DialogModule,
        InputsModule,
        ButtonsModule,
        LayoutModule,
        RippleModule,
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