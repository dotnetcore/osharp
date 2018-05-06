import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { SettingsService } from "../../shared/angle/core/settings/settings.service";
import { LoginDto } from "../identity.model";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginDto: LoginDto = new LoginDto();
  verifyCodeUrl: string = "common/verifycode"

  constructor(public settings: SettingsService, private http: HttpClient) {
  }

  onVerifyCodeClick($event) {
    this.verifyCodeUrl = "/common"
  }

  onSubmitForm($event) {
    $event.preventDefault();
    this.http.post("/api/identity/login", this.loginDto).subscribe(res => {
      console.log(res);
    });
  }
}
