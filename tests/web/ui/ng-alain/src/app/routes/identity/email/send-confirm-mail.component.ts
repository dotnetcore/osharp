import { Component } from '@angular/core';
import { SendMailDto, AjaxResult, AjaxResultType, AdResult } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '../shared/identity.service';

@Component({
  selector: 'app-identity-send-confirm-mail',
  templateUrl: `../shared/send-mail.html`
})
export class SendConfirmMailComponent {

  title = "重发注册邮箱激活邮件";
  dto: SendMailDto = new SendMailDto();
  result: AdResult = new AdResult();
  canSubmit = true;
  sended = false;

  constructor(
    public router: Router,
    public osharp: OsharpService,
    private identity: IdentityService
  ) { }

  submitForm() {
    this.canSubmit = false;
    this.identity.sendConfirmMail(this.dto).then(res => {
      this.canSubmit = true;
      this.sended = true;
      this.result = res;
    });
  }
}
