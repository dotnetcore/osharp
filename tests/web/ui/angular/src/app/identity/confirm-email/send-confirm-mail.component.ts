import { Component, OnInit } from '@angular/core';
import { SendMailDto } from '../../shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'identity-send-confirm-mail',
  templateUrl: '../shared/send-mail.component.html'
})
export class SendConfirmMailComponent implements OnInit {

  title: string = "重发激活邮件";
  sendDto: SendMailDto = new SendMailDto();
  canSubmit: boolean = true;
  isSended: boolean = false;
  message: string;

  constructor(private http: HttpClient, public router: Router) { }

  ngOnInit(): void { }

  onSubmit() {
    this.canSubmit = false;
    this.http.post("/api/identity/SendConfirmMail", this.sendDto).subscribe((res: any) => {
      if (res.Type != "Success") {
        this.canSubmit = true;
        this.message = res.Content;
        return;
      }
      this.isSended = true;
      this.message = "邮箱激活邮件发送成功，请登录邮箱“" + this.sendDto.Email + "”收取邮件进行后续步骤。";
      setTimeout(() => {
        this.router.navigateByUrl('/home');
      }, 3000);
    });
  }
}
