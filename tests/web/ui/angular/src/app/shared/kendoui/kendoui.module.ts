import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KendouiSplitterComponent } from './components/kendoui-splitter.component';
import { KendouiWindowComponent } from './components/kendoui-window.component';
import { KendouiTabstripComponent } from './components/kendoui-tabstrip.component';
import { KendouiTreeviewComponent } from './components/kendoui-treeview.component';

@NgModule({
    declarations: [
        KendouiSplitterComponent,
        KendouiWindowComponent,
        KendouiTabstripComponent,
        KendouiTreeviewComponent
    ],
    imports: [CommonModule],
    exports: [
        KendouiSplitterComponent,
        KendouiWindowComponent,
        KendouiTabstripComponent,
        KendouiTreeviewComponent
    ],
    providers: [],
})
export class KendouiModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: KendouiModule
        };
    }
}