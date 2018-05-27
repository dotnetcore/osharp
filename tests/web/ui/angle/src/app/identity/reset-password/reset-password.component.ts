import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { osharp } from "../../shared/osharp";
import { ResetPasswordDto, AjaxResultType } from '../../shared/osharp/osharp.model';
import { timeout } from 'q';
import { Router } from '@angular/router';

@Component({
  selector: 'identity-reset-password',
  templateUrl: './reset-password.component.html'
})
export class ResetPasswordComponent implements OnInit {

  resetDto: ResetPasswordDto = new ResetPasswordDto();
  canSubmit: boolean = true;
  message: string;
  messageType: string = "success";

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    let url = window.location.hash;
    this.resetDto.UserId = osharp.Tools.getHashURLSearchParams(url, "userId");
    this.resetDto.Token = osharp.Tools.getHashURLSearchParams(url, "token");
  }

  onSubmit(e) {
    this.canSubmit = false;
    this.http.post("/api/identity/ResetPassword", this.resetDto).subscribe((res: any) => {
      if (res.Type == AjaxResultType.Success) {
        this.message = "用户登录密码重置成功";
        setTimeout(() => {
          this.router.navigateByUrl('/home');
        }, 2000);
        return;
      }
      this.canSubmit = true;
      this.message = "用户登录密码重置失败：" + res.Content;
    });
  }

}
