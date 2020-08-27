import { Component } from '@angular/core';
import { SendMailDto, AdResult, ConfirmEmailDto } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { IdentityService } from '@shared/osharp/services/identity.service';

@Component({
  selector: 'passport-send-confirm-mail',
  template: `
  <div *ngIf="!result.show">
    <passport-send-mail [title]="title" (submited)="onSubmited($event)"></passport-send-mail>
  </div>
  <result *ngIf="result.show" type="{{result.type}}" [title]="result.title" description="{{result.description}}">
    <button nz-button [nzType]="'primary'" (click)="router.navigate(['passport/login'])">立即登录</button>
    <button nz-button (click)="router.navigate(['home'])">返回首页</button>
  </result>
  `,
  styles: [``]
})
export class SendConfirmMailComponent {

  title = '重发邮箱激活邮件';
  dto: SendMailDto = new SendMailDto();
  result: AdResult = new AdResult();

  constructor(public router: Router, public identity: IdentityService) { }

  onSubmited(value: SendMailDto) {
    this.dto = value;
    this.identity.sendConfirmMail(value).then(res => {
      res.show = true;
      this.result = res;
    });
  }
}
