import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { CustomValidators } from 'ng2-validation';
import { RegisterDto } from "../identity.model";
import { Router } from '@angular/router';

@Component({
  selector: 's-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent {

  registerDto: RegisterDto = new RegisterDto();
  message: string;

  constructor(private http: HttpClient, private router: Router) { }

  onSubmit(e) {
    e.preventDefault();
    this.http.post("/api/identity/register", this.registerDto).subscribe(response => {
      var res: any = response;
      if (res.Type == "Success") {
        this.message = "用户注册成功";
        this.router.navigateByUrl('/home');
        return;
      }
      this.message = "用户注册失败：" + res.Content;
    });
  }
}
