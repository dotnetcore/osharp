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
  canSubmit: boolean = true;
  verifyCodeUrl: string = "common/verifycode"

  constructor(public settings: SettingsService, private http: HttpClient, private router: Router) {
  }

  onVerifyCodeClick($event) {
    this.verifyCodeUrl = "/common"
  }

  onSubmit($event) {
    $event.preventDefault();
    this.canSubmit = false;
    this.http.post("/api/identity/login", this.loginDto).subscribe((res: any) => {
      if (res.Type == "Success") {
        this.message = "用户登录成功";
        setTimeout(() => {
          this.router.navigateByUrl('/home');
        }, 1000);
        return;
      }
      this.canSubmit = true;
      this.message = "登录失败：" + res.Content;
    });
  }
}
