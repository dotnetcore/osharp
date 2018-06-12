import { Component, Injector, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { IdentityService, } from '../shared/identity.service';
import { LoginDto, AjaxResultType } from '@shared/osharp/osharp.model';
import { NzMessageService } from 'ng-zorro-antd';
import { Router } from '@angular/router';
import { ComponentBase } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent extends ComponentBase implements OnInit {

  dto: LoginDto = new LoginDto();
  canSubmit = true;
  resendConfirmMail = false;
  error: string;

  constructor(
    private msgSrv: NzMessageService,
    private _service: IdentityService,
    private router: Router,
    injector: Injector
  ) {
    super(injector);
    this.authDict = {
      "Login": false
    };
  }

  async ngOnInit() {
    await this.checkAuth();
  }

  Position(): string {
    return 'Root.Site.Identity';
  }

  submitForm() {
    this.canSubmit = false;
    this._service.login(this.dto).then(result => {
      if (result.Type == AjaxResultType.Success) {
        this.msgSrv.success("用户登录成功");
        setTimeout(() => {
          this.router.navigate(['home']);
        }, 1000);
        return;
      }
      this.canSubmit = true;
      this.error = `登录失败：${result.Content}`;
      this.resendConfirmMail = result.Content.indexOf("邮箱未验证") > -1;
    }).catch(e => {
      this.canSubmit = true;
      this.error = `发生错误：${e.statusText}`;
    });
  }
}
