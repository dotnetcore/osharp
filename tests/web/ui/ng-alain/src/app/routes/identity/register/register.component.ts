import { Component, } from '@angular/core';
import { IdentityService } from '../shared/identity.service';

import { RegisterDto, AjaxResultType, AdResultDto } from '@shared/osharp/osharp.model';
import { NzMessageService } from 'ng-zorro-antd';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less']
})
export class RegisterComponent {

  dto: RegisterDto = new RegisterDto();
  result: AdResultDto = new AdResultDto();

  canSubmit = true;
  sended = false;

  constructor(
    private msgSrv: NzMessageService,
    private _service: IdentityService,
    private osharp: OsharpService,
    private router: Router
  ) { }

  submitForm() {
    this.canSubmit = false;
    this._service.register(this.dto).then(res => {
      this.sended = true;
      if (res.Type == AjaxResultType.Success) {
        this.result.type = "success";
        this.result.title = "新用户注册成功";
        this.result.description = `你的账户：${this.dto.UserName}[${this.dto.NickName}] 注册成功，请及时登录邮箱 ${this.dto.Email} 接收邮件激活账户。`;
        return;
      }
      this.canSubmit = true;
      this.result.type = 'error';
      this.result.title = "用户注册失败";
      this.result.description = res.Content;
    }).catch(e => {
      this.canSubmit = true;
      console.error(e);
      this.router.navigate(['500']);
    });
  }

  onBack() {
    this.sended = false;
  }
}
