import { Component, Injector, AfterViewInit, } from '@angular/core';
import { IdentityService } from '@shared/osharp/services/identity.service';

import { RegisterDto, AdResult, AuthConfig, VerifyCode } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService, ComponentBase } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less']
})
export class RegisterComponent extends ComponentBase implements AfterViewInit {

  dto: RegisterDto = new RegisterDto();
  code: VerifyCode = new VerifyCode();
  result: AdResult = new AdResult();
  canSubmit = true;
  msg: string;

  constructor(
    private _service: IdentityService,
    public osharp: OsharpService,
    private router: Router,
    injector: Injector
  ) {
    super(injector);
    super.checkAuth();
    if (!this.auth.Register) {
      this.result.show = true;
      this.result.type = 'minus-circle-o';
      this.result.title = "新用户注册尚未开放";
      this.result.description = "当前系统未开放新用户注册功能，请稍后重试……";
    }
  }

  ngAfterViewInit() {
    this.refreshVerifyCode();
  }

  protected AuthConfig(): AuthConfig {
    return new AuthConfig("Root.Site.Identity", ["Register", "Login"]);
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
    this._service.register(this.dto).then(res => {
      res.show = true;
      this.result = res;
      this.canSubmit = true;
    }).catch(e => {
      this.canSubmit = true;
      console.error(e);
      this.router.navigate(['500']);
    });
  }
}
