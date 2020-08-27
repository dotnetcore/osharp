// tslint:disable: no-duplicate-imports
import { NgModule, LOCALE_ID, APP_INITIALIZER, Injector } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// #region default language
// Reference: https://ng-alain.com/docs/i18n
import { default as ngLang } from '@angular/common/locales/zh';
import { NZ_I18N, zh_CN as zorroLang } from 'ng-zorro-antd';
import { DELON_LOCALE, zh_CN as delonLang } from '@delon/theme';
const LANG = {
  abbr: 'zh',
  ng: ngLang,
  zorro: zorroLang,
  delon: delonLang,
};
// register angular
import { registerLocaleData } from '@angular/common';
registerLocaleData(LANG.ng, LANG.abbr);
const LANG_PROVIDES = [
  { provide: LOCALE_ID, useValue: LANG.abbr },
  { provide: NZ_I18N, useValue: LANG.zorro },
  { provide: DELON_LOCALE, useValue: LANG.delon },
];
// #endregion
// #region i18n services
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { I18NService } from '@core/i18n/i18n.service';

export function I18nHttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, `assets/osharp/i18n/`, '.json');
}

const I18NSERVICE_MODULES = [
  TranslateModule.forRoot({
    loader: {
      provide: TranslateLoader,
      useFactory: I18nHttpLoaderFactory,
      deps: [HttpClient]
    }
  })
];

const I18NSERVICE_PROVIDES = [
  { provide: ALAIN_I18N_TOKEN, useClass: I18NService, multi: false }
];
// #region

// #region JSON Schema form (using @delon/form)
import { JsonSchemaModule } from '@shared/json-schema/json-schema.module';
const FORM_MODULES = [JsonSchemaModule];
// #endregion


// #region Http Interceptors
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { RefreshJWTInterceptor } from '@core/net/refresh-jwt.interceptor';
import { DefaultInterceptor } from '@core/net/default.interceptor';
// Default 和 RefreshJWT 的顺序不能换
const INTERCEPTOR_PROVIDES = [
  { provide: HTTP_INTERCEPTORS, useClass: DefaultInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: RefreshJWTInterceptor, multi: true },
];
// #endregion

// #region global third module
const GLOBAL_THIRD_MODULES = [
];
// #endregion

// #region Startup Service
import { StartupService } from '@core/startup/startup.service';
export function StartupServiceFactory(startupService: StartupService) {
  return () => startupService.load();
}
const APPINIT_PROVIDES = [
  StartupService,
  {
    provide: APP_INITIALIZER,
    useFactory: StartupServiceFactory,
    deps: [StartupService],
    multi: true
  }
];
// #endregion

import { DelonModule } from './delon.module';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { AppComponent } from './app.component';
import { RoutesModule } from './routes/routes.module';
import { LayoutModule } from './layout/layout.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    DelonModule.forRoot(),
    CoreModule,
    SharedModule,
    LayoutModule,
    RoutesModule,
    ...I18NSERVICE_MODULES,
    ...FORM_MODULES,
    ...GLOBAL_THIRD_MODULES
  ],
  providers: [
    ...LANG_PROVIDES,
    ...INTERCEPTOR_PROVIDES,
    ...I18NSERVICE_PROVIDES,
    ...APPINIT_PROVIDES
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
