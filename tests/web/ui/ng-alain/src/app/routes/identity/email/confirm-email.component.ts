import { Component, OnInit, } from '@angular/core';
import { ConfirmEmailDto, AjaxResult, AjaxResultType, AdResultDto } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-confirm-email',
  template: `
  <result type="{{result.type}}" [title]="result.title" description="{{result.description}}">
    <button nz-button [nzType]="'primary'" (click)="router.navigate(['identity/login'])">立即登录</button>
    <button nz-button (click)="router.navigate(['home'])">返回首页</button>
  </result>
  `,
})
export class ConfirmEmailComponent implements OnInit {

  dto: ConfirmEmailDto = new ConfirmEmailDto();
  result: AdResultDto = new AdResultDto();

  constructor(
    private http: HttpClient,
    public router: Router,
    private osharp: OsharpService
  ) { }

  ngOnInit(): void {
    this.getUrlParams();
    this.result.title = "正在激活注册邮箱……";
    this.confirmEmail();
  }

  private getUrlParams() {
    let url = window.location.hash;
    this.dto.UserId = this.osharp.getHashURLSearchParams(url, "userId");
    this.dto.Code = this.osharp.getHashURLSearchParams(url, "code");
  }

  private confirmEmail() {
    this.http.post('api/identity/ConfirmEmail', this.dto).subscribe((res: AjaxResult) => {
      if (res.Type != AjaxResultType.Success) {
        this.result.type = "error";
        this.result.title = "注册邮箱激活失败";
        if (res.Type == AjaxResultType.Info) {
          this.result.type = 'minus-circle-o';
        }
        this.result.title = "注册邮箱激活取消";
        this.result.description = res.Content;
        return;
      }
      this.result.type = "success";
      this.result.title = "注册邮箱激活成功";
      this.result.description = res.Content;
    });
  }
}
