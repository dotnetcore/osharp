import { Component, OnInit, Injector, AfterViewInit } from '@angular/core';
import { SendMailDto, VerifyCode, AdResult, AuthConfig } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService, ComponentBase } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'app-passport-forgot-password',
  templateUrl: '../shared/send-mail.html',
  styleUrls: ['../shared/send-mail.less']
})
export class ForgotPasswordComponent extends ComponentBase implements OnInit, AfterViewInit {

  title = '发送重置密码邮件';
  mailSchema: SFSchema;
  dto: SendMailDto = new SendMailDto();
  code: VerifyCode = new VerifyCode();
  result: AdResult = new AdResult();

  constructor(
    public router: Router,
    public osharp: OsharpService,
    private identity: IdentityService,
    injector: Injector
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.mailSchema = {
      properties: {
        Email: { title: '电子邮箱', type: 'string', format: 'email', ui: { placeholder: '请输入电子邮箱' } },
        VerifyCode: { title: '验证码', type: 'string', ui: { placeholder: '验证码' } },
        VerifyCodeId: { type: 'string', ui: { hidden: true } }
      },
      required: ['Email', 'VerifyCode']
    };


  }

  ngAfterViewInit(): void { }

  protected AuthConfig(): AuthConfig {
    return { position: 'Root.Site.Identity', funcs: ['SendResetPasswordMail'] };
  }

}
