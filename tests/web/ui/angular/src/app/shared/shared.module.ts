import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";

import { LoggingService } from './services/logging.service';
import { OsharpModule } from './osharp/osharp.module';

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        FormsModule,
        OsharpModule,
    ],
    exports: [
        CommonModule,
        FormsModule
    ],
    providers: [LoggingService],
})
export class SharedModule { }