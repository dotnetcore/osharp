import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KendouiSplitterComponent } from './components/kendoui-splitter.component';
import { KendouiWindowComponent } from './components/kendoui-window.component';
import { KendouiTabstripComponent } from './components/kendoui-tabstrip.component';
import { KendouiTreeviewComponent } from './components/kendoui-treeview.component';
import { KendouiFunctionComponent } from './components/kendoui-function.component';

const COMPONENTS = [
  KendouiSplitterComponent,
  KendouiWindowComponent,
  KendouiTabstripComponent,
  KendouiTreeviewComponent,
  KendouiFunctionComponent
];

@NgModule({
  declarations: [
    ...COMPONENTS
  ],
  imports: [CommonModule],
  exports: [
    ...COMPONENTS
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
