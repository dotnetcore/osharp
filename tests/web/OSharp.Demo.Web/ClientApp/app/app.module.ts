import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from "@angular/common";
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from "@angular/common/http";
import { HttpModule } from "@angular/http";
import { RouterModule } from "@angular/router";
import { TranslateModule, TranslateService, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

//shared
import { AngleSharedModule } from "./shared/angle.shared.module";
import { MaterialSharedModule } from './shared/material.shared.module';
import { KendouiSharedModule } from "./shared/kendoui.shared.module";

//app
import { AppComponent } from './app.component';
import { AppRoutingModule } from "./app.routing";
import { HomeModule } from './home/home.module';
import { Demo01Module } from "./demo01/demo01.module";
import { LoggingService } from './shared/services/logging.services';
import { OsharpModule } from './shared/osharp/osharp.module';
import { OsharpService } from './shared/osharp/osharp.service';

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    HttpModule,
    RouterModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),

    //shared
    AngleSharedModule.forRoot(),
    MaterialSharedModule.forRoot(),
    KendouiSharedModule.forRoot(),
    OsharpModule,

    //app
    AppRoutingModule,
    HomeModule,
    Demo01Module
  ],
  exports: [],
  providers: [
    HttpClient,
    TranslateService,
    LoggingService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }  