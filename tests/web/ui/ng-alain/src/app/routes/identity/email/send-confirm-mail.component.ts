import { Component, Inject, Injector } from '@angular/core';
import { SendMailDto, AjaxResult, AjaxResultType, AdResult, AuthConfig } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService, ComponentBase } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '../shared/identity.service';

@Component({
  selector: 'app-identity-send-confirm-mail',
  templateUrl: `../shared/send-mail.html`
})
export class SendConfirmMailComponent extends ComponentBase {

  title = "重发注册邮箱激活邮件";
  dto: SendMailDto = new SendMailDto();
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

  protected AuthConfig() {
    return new AuthConfig("Root.Site.Identity", ["SendConfirmMail"]);
  }

  submitForm() {
    this.canSubmit = false;
    this.identity.sendConfirmMail(this.dto).then(res => {
      res.show = true;
      this.result = res;
      this.canSubmit = true;
    });
  }
}
