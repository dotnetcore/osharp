import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SendMailDto } from '../../shared/osharp/osharp.model';
import { ContentType } from '@angular/http/src/enums';

@Component({
  selector: 'identity-forgot-password',
  templateUrl: '../shared/send-mail.component.html'
})
export class ForgotPasswordComponent implements OnInit {

  title: string = "找回密码";
  sendDto: SendMailDto = new SendMailDto();
  canSubmit: boolean = true;
  isSended: boolean = false;
  message: string;

  constructor(private http: HttpClient, public router: Router) { }

  ngOnInit(): void { }

  onSubmit() {
    this.canSubmit = false;
    this.http.post("/api/identity/SendResetPasswordMail", this.sendDto).subscribe((res: any) => {
      if (res.Type != "Success") {
        this.canSubmit = true;
        this.message = res.Content;
        return;
      }
      this.isSended = true;
      this.message = "重置密码邮件发送成功，请登录邮箱“" + this.sendDto.Email + "”收取邮件进行后续步骤。";
      setTimeout(() => {
        this.router.navigateByUrl('/home');
      }, 3000);
    });
  }
}
