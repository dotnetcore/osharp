import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from '@angular/router';
import { SettingsService } from "../../shared/angle/core/settings/settings.service";
import { LoginDto } from "../identity.model";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginDto: LoginDto = new LoginDto();
  message: string;
  verifyCodeUrl: string = "common/verifycode"

  constructor(public settings: SettingsService, private http: HttpClient, private router: Router) {
  }

  onVerifyCodeClick($event) {
    this.verifyCodeUrl = "/common"
  }

  onSubmit($event) {
    $event.preventDefault();
    this.http.post("/api/identity/login", this.loginDto).subscribe(response => {
      var res: any = response;
      if (res.Type == "Success") {
        this.message = "用户登录成功";
        this.router.navigateByUrl('/home');
        return;
      }
      this.message = "登录失败：" + res.Content;
    });
  }
}
