import { Component, } from '@angular/core';
import { IdentityService } from '../shared/identity.service';

import { RegisterDto, AdResult } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less']
})
export class RegisterComponent {

  dto: RegisterDto = new RegisterDto();
  result: AdResult = new AdResult();

  canSubmit = true;
  sended = false;

  constructor(
    private _service: IdentityService,
    public osharp: OsharpService,
    private router: Router
  ) { }

  submitForm() {
    this.canSubmit = false;
    this._service.register(this.dto).then(res => {
      this.sended = true;
      this.canSubmit = true;
      this.result = res;
    }).catch(e => {
      this.canSubmit = true;
      console.error(e);
      this.router.navigate(['500']);
    });
  }

  onBack() {
    this.sended = false;
  }
}
