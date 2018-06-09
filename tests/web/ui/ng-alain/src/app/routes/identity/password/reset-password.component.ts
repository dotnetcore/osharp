import { Component, OnInit } from '@angular/core';
import { ResetPasswordDto, AdResultDto, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-reset-password',
  templateUrl: `./reset-password.component.html`
})
export class ResetPasswordComponent implements OnInit {

  dto: ResetPasswordDto = new ResetPasswordDto();
  result: AdResultDto = new AdResultDto();
  canSubmit = true;
  sended = false;

  constructor(
    private http: HttpClient,
    public router: Router,
    private osharp: OsharpService
  ) { }

  ngOnInit(): void {
    this.getUrlParams();
  }

  private getUrlParams() {
    let url = window.location.hash;
    this.dto.UserId = this.osharp.getHashURLSearchParams(url, "userId");
    this.dto.Token = this.osharp.getHashURLSearchParams(url, "token");
  }

  submitForm() {
    this.canSubmit = false;
    this.http.post('api/identity/ResetPassword', this.dto).subscribe((res: AjaxResult) => {
      this.sended = true;
      this.canSubmit = true;
      if (res.Type != AjaxResultType.Success) {
        this.result.type = 'error';
        this.result.title = '登录密码重置失败';
        this.result.description = res.Content;
        return;
      }
      this.result.type = "success";
      this.result.title = '登录密码重置成功';
      this.result.description = "登录密码重置成功，请使用新密码登录系统。";
    });
  }
}
