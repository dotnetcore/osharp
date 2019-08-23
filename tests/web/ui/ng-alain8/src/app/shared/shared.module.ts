import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
// delon
import { AlainThemeModule } from '@delon/theme';
import { DelonABCModule } from '@delon/abc';
import { DelonACLModule } from '@delon/acl';
import { DelonFormModule } from '@delon/form';
// i18n
import { TranslateModule } from '@ngx-translate/core';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/catch';

// #region third libs
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { CountdownModule } from 'ngx-countdown';
import { OsharpModule } from './osharp/osharp.module';

import { ModalTreeComponent } from './components/modal-tree/modal-tree.component';
import { FilterGroupComponent } from './components/filter-group/filter-group.component';
import { FilterRuleComponent } from './components/filter-group/filter-rule.component';
import { AdSearchComponent } from './components/ad-search/ad-search.component';
import { AdSearchModalComponent } from './components/ad-search/modal/modal.component';
import { FunctionViewComponent } from './components/function-view/function-view.component';

const THIRDMODULES = [
  NgZorroAntdModule,
  CountdownModule,
  OsharpModule
];
// #endregion

// #region your componets & directives
const COMPONENTS = [
  ModalTreeComponent,
  FilterGroupComponent,
  FilterRuleComponent,
  AdSearchComponent,
  AdSearchModalComponent,
  FunctionViewComponent
];
const DIRECTIVES = [];
// #endregion

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    AlainThemeModule.forChild(),
    DelonABCModule,
    DelonACLModule,
    DelonFormModule,
    // third libs
    ...THIRDMODULES
  ],
  declarations: [
    // your components
    ...COMPONENTS,
    ...DIRECTIVES
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    AlainThemeModule,
    DelonABCModule,
    DelonACLModule,
    DelonFormModule,
    // i18n
    TranslateModule,
    // third libs
    ...THIRDMODULES,
    // your components
    ...COMPONENTS,
    ...DIRECTIVES,
  ]
})
export class SharedModule { }
