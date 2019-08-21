import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OsharpService } from './services/osharp.service';
import { RemoteValidator } from './directives/remote-validator.directive';
import { RemoteInverseValidator } from './directives/remote-inverse-validator.directive';
import { NgZorroAntdModule } from 'ng-zorro-antd';

const DIRECTIVES = [
  RemoteValidator,
  RemoteInverseValidator
];

const COMPONENTS = [];

const SERVICES = [
  OsharpService,
];

@NgModule({
  imports: [CommonModule, FormsModule, NgZorroAntdModule],
  declarations: [...DIRECTIVES, ...COMPONENTS,],
  providers: [
    ...SERVICES
  ],
  exports: [...COMPONENTS]
})
export class OsharpModule { }
