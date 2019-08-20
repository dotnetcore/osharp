import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OsharpService } from './services/osharp.service';
import { RemoteValidator } from './directives/remote-validator.directive';
import { RemoteInverseValidator } from './directives/remote-inverse-validator.directive';

const DIRECTIVES = [
  RemoteValidator,
  RemoteInverseValidator
];

const COMPONENTS = [];

const SERVICES = [
  OsharpService,
];

@NgModule({
  imports: [CommonModule, FormsModule],
  declarations: [...DIRECTIVES, ...COMPONENTS],
  providers: [
    ...SERVICES
  ]
})
export class OsharpModule { }
