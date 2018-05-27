import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from '@angular/router';
import { SettingsService } from "../../shared/angle/core/settings/settings.service";
import { LoginDto } from '../../shared/osharp/osharp.model';
import { AuthService } from '../../shared/osharp/services/auth.service';
import { AjaxResultType } from '../../shared/osharp/osharp.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginDto: LoginDto = new LoginDto();
  message: string;
  canSubmit: boolean = true;
  resendConfirmMail: boolean = false;
  verifyCodeUrl: string = "common/verifycode"

  constructor(private auth: AuthService, private router: Router) { }

  onVerifyCodeClick($event) {
    this.verifyCodeUrl = "/common"
  }

  onSubmit($event) {
    $event.preventDefault();
    this.canSubmit = false;
    this.auth.login(this.loginDto).then(result => {
      if (result.Type == AjaxResultType.Success) {
        this.message = "用户登录成功";
        setTimeout(() => {
          this.router.navigateByUrl('/home');
        }, 1000);
        return;
      }
      this.canSubmit = true;
      this.message = "登录失败：" + result.Content;
      if (result.Content.indexOf("邮箱未验证") > -1) {
        this.resendConfirmMail = true;
      }
    }).catch(error => {
      this.canSubmit = true;
      this.message = `发生错误：${error.statusText}`;
    });
  }
}
