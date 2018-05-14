import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { CustomValidators } from 'ng2-validation';
import { RegisterDto } from '../../shared/osharp/osharp.model';
import { Router } from '@angular/router';

@Component({
  selector: 's-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent {

  registerDto: RegisterDto = new RegisterDto();
  canSubmit: boolean = true;
  message: string;


  constructor(private http: HttpClient, private router: Router) { }

  onSubmit(e) {
    e.preventDefault();
    this.canSubmit = false;
    this.http.post("/api/identity/register", this.registerDto).subscribe((res: any) => {
      if (res.Type == "Success") {
        this.message = "用户注册成功";
        setTimeout(() => {
          this.router.navigateByUrl('/home');
        }, 2000);
        return;
      }
      this.canSubmit = true;
      this.message = "用户注册失败：" + res.Content;
    });
  }
}
