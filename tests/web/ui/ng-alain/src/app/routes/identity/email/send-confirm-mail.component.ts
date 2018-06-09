import { Component } from '@angular/core';
import { SendMailDto, AjaxResult, AjaxResultType, AdResultDto } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-send-confirm-mail',
  templateUrl: `../shared/send-mail.html`
})
export class SendConfirmMailComponent {

  title = "重发注册邮箱激活邮件";
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
    this.http.post('api/identity/SendConfirmMail', this.dto).subscribe((res: AjaxResult) => {
      this.sended = true;
      this.canSubmit = true;
      if (res.Type != AjaxResultType.Success) {
        this.result.type = 'error';
        this.result.title = '重发激活邮件失败';
        this.result.description = res.Content;
        return;
      }
      this.result.type = "success";
      this.result.title = '重发激活邮件成功';
      this.result.description = `注册邮箱激活邮件发送成功，请登录邮箱“${this.dto.Email}”收取邮件进行后续步骤`;
    });
  }
}
