import { Component, OnInit, } from '@angular/core';
import { ConfirmEmailDto, AjaxResult, AjaxResultType, AdResult } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';

@Component({
  selector: 'passport-confirm-email',
  template: `
  <result type="{{result.type}}" [title]="result.title" description="{{result.description}}">
    <button nz-button [nzType]="'primary'" (click)="router.navigate(['passport/login'])">立即登录</button>
    <button nz-button (click)="router.navigate(['home'])">返回首页</button>
  </result>
  `,
})
export class ConfirmEmailComponent implements OnInit {

  dto: ConfirmEmailDto = new ConfirmEmailDto();
  result: AdResult = new AdResult();

  constructor(
    public router: Router,
    private osharp: OsharpService,
    private identity: IdentityService
  ) { }

  ngOnInit(): void {
    this.getUrlParams();
    this.result.title = "正在激活注册邮箱……";
    this.confirmEmail();
  }

  private getUrlParams() {
    const url = window.location.hash;
    this.dto.UserId = this.osharp.getHashURLSearchParams(url, "userId");
    this.dto.Code = this.osharp.getHashURLSearchParams(url, "code");
  }

  private confirmEmail() {
    this.identity.confirmEmail(this.dto).then(res => {
      this.result = res;
    });
  }
}
