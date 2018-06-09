import { Component } from '@angular/core';
import { SendMailDto, AdResult, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '../shared/identity.service';

@Component({
  selector: 'app-identity-forgot-password',
  templateUrl: `../shared/send-mail.html`,
})
export class ForgotPasswordComponent {

  title = '发送重置密码邮件';
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
    this.identity.sendResetPasswordMail(this.dto).then(res => {
      this.sended = true;
      this.canSubmit = true;
      this.result = res;
    });
  }
}
