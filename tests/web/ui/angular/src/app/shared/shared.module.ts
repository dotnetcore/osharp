import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { LoggingService } from './services/logging.service';
import { OsharpModule } from './osharp/osharp.module';

import { RemoteValidator } from "./osharp/directives/remote-validator.directive";

@NgModule({
  declarations: [RemoteValidator],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    OsharpModule,
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RemoteValidator
  ],
  providers: [LoggingService],
})
export class SharedModule { }
