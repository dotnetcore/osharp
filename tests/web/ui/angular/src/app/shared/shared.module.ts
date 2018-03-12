import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";

import { LoggingService } from './services/logging.service';

@NgModule({
    declarations: [],
    imports: [CommonModule, FormsModule],
    exports: [CommonModule, FormsModule],
    providers: [LoggingService],
})
export class SharedModule { }