import { Component } from '@angular/core';
import { SendMailDto, AdResultDto, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-forgot-password',
  templateUrl: `../shared/send-mail.html`,
})
export class ForgotPasswordComponent {

  title = '发送重置密码邮件';
  dto: SendMailDto = new SendMailDto();
  result: AdResultDto = new AdResultDto();
  canSubmit = true;
  sended = false;

  constructor(
    private http: HttpClient,
    public router: Router,
    public osharp: OsharpService
  ) { }

  submitForm() {
    this.canSubmit = false;
    this.http.post('api/identity/SendResetPasswordMail', this.dto).subscribe((res: AjaxResult) => {
      this.sended = true;
      this.canSubmit = true;
      if (res.Type != AjaxResultType.Success) {
        this.result.type = 'error';
        this.result.title = '重置密码邮件发送失败';
        this.result.description = res.Content;
        return;
      }
      this.result.type = 'success';
      this.result.title = '重置密码邮件发送成功';
      this.result.description = `重置密码邮件发送成功，请登录邮箱“${this.dto.Email}”收取邮件进行后续步骤`;
    });
  }
}
