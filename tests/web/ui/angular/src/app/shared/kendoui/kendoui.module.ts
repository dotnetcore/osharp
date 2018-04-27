import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KendouiSplitterComponent } from './components/kendoui-splitter.component';
import { KendouiWindowComponent } from './components/kendoui-window.component';
import { KendouiTabstripComponent } from './components/kendoui-tabstrip.component';
import { KendouiTreeviewComponent } from './components/kendoui-treeview.component';
import { KendouiFunctionComponent } from './components/kendoui-function.component';

@NgModule({
  declarations: [
    KendouiSplitterComponent,
    KendouiWindowComponent,
    KendouiTabstripComponent,
    KendouiTreeviewComponent,
    KendouiFunctionComponent
  ],
  imports: [CommonModule],
  exports: [
    KendouiSplitterComponent,
    KendouiWindowComponent,
    KendouiTabstripComponent,
    KendouiTreeviewComponent,
    KendouiFunctionComponent
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
