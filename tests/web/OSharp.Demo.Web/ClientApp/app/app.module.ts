import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from "@angular/common";
import { FormsModule } from '@angular/forms';
import { HttpModule } from "@angular/http";
import { RouterModule } from "@angular/router";

//shared
import { MaterialSharedModule } from './shared/material.shared.module';
import { KendouiSharedModule } from "./shared/kendoui.shared.module";

//app
import { AppComponent } from './app.component';
import { AppRoutingModule } from "./app.routing";
import { HomeModule } from './home/home.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    FormsModule,
    HttpModule,
    RouterModule,

    MaterialSharedModule,
    KendouiSharedModule,

    HomeModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }  