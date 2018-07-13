import { Component, Inject, Injector, AfterViewInit } from '@angular/core';
import { SendMailDto, AjaxResult, AjaxResultType, AdResult, AuthConfig, VerifyCode } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService, ComponentBase } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';

@Component({
  selector: 'app-identity-send-confirm-mail',
  templateUrl: `../shared/send-mail.html`
})
export class SendConfirmMailComponent extends ComponentBase implements AfterViewInit {

  title = "重发注册邮箱激活邮件";
  dto: SendMailDto = new SendMailDto();
  code: VerifyCode = new VerifyCode();
  result: AdResult = new AdResult();
  canSubmit = true;
  canSend = false;

  constructor(
    public router: Router,
    public osharp: OsharpService,
    private identity: IdentityService,
    injector: Injector
  ) {
    super(injector);
    this.checkAuth().then(() => {
      this.canSend = this.auth.SendConfirmMail;
    });
  }

  ngAfterViewInit() {
    this.refreshVerifyCode();
  }

  protected AuthConfig() {
    return new AuthConfig("Root.Site.Identity", ["SendConfirmMail"]);
  }

  refreshVerifyCode() {
    this.osharp.refreshVerifyCode().subscribe(vc => {
      this.code = vc;
    });
  }

  submitForm() {
    this.dto.VerifyCode = this.code.code;
    this.dto.VerifyCodeId = this.code.id;
    this.canSubmit = false;
    this.identity.sendConfirmMail(this.dto).then(res => {
      res.show = true;
      this.result = res;
      this.canSubmit = true;
    });
  }
}
