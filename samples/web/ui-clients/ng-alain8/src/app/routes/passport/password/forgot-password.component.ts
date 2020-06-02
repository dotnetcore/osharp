import { Component } from '@angular/core';
import { AdResult, SendMailDto } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';

@Component({
  selector: 'app-passport-forgot-password',
  template: `
  <div *ngIf="!result.show">
    <passport-send-mail [title]="title" (submited)="onSubmited($event)"></passport-send-mail>
  </div>
  <result *ngIf="result.show" type="{{result.type}}" [title]="result.title" description="{{result.description}}">
    <button nz-button [nzType]="'primary'" (click)="result.show=false;dto.VerifyCode=null;">返回</button>
    <button *ngIf="result.type=='success'" nz-button (click)="osharp.openMailSite(dto.Email)">进入邮箱</button>
    <button nz-button (click)="router.navigate(['home'])">返回首页</button>
  </result>
  `,
  styles: [`
  :host {
    display: block;
    width: 400px;
    margin: 0 auto;
  `]
})
export class ForgotPasswordComponent {

  title = '发送重置密码邮件';
  dto: SendMailDto = new SendMailDto();
  result: AdResult = new AdResult();

  constructor(public router: Router, public osharp: OsharpService, private identity: IdentityService) { }

  onSubmited(value: SendMailDto) {
    this.dto = value;
    this.identity.sendResetPasswordMail(value).then(res => {
      res.show = true;
      this.result = res;
    });
  }
}
