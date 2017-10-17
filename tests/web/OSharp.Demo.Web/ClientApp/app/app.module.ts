import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from "@angular/common";
import { FormsModule } from '@angular/forms';
import { HttpModule } from "@angular/http";
import { RouterModule } from "@angular/router";

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

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    FormsModule,
    HttpModule,
    RouterModule,

    //shared
    AngleSharedModule.forRoot(),
    MaterialSharedModule.forRoot(),
    KendouiSharedModule.forRoot(),

    //app
    AppRoutingModule,
    HomeModule,
    Demo01Module
  ],
  exports: [],
  providers: [
    LoggingService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }  