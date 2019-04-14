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

// #region third libs
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { CountdownModule } from 'ngx-countdown';
import { AngularSplitModule } from "angular-split";
const THIRDMODULES = [
  NgZorroAntdModule,
  CountdownModule,
  AngularSplitModule,
];

// #endregion

import { OsharpModule } from "./osharp/osharp.module";

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/distinctUntilChanged';

import { ModalTreeComponent } from './components/modal-tree/modal-tree.component';
import { FilterGroupComponent } from './components/filter-group/filter-group.component';
import { FilterRuleComponent } from './components/filter-group/filter-rule.component';
import { AdSearchComponent } from './components/ad-search/ad-search.component';
import { AdSearchModalComponent } from './components/ad-search/modal/modal.component';

// #region your componets & directives
const COMPONENTS = [
  ModalTreeComponent,
  FilterGroupComponent,
  FilterRuleComponent,
  AdSearchComponent,
  AdSearchModalComponent
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
    OsharpModule,
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
    OsharpModule,
    // i18n
    TranslateModule,
    // third libs
    ...THIRDMODULES,
    // your components
    ...COMPONENTS,
    ...DIRECTIVES
  ]
})
export class SharedModule { }
