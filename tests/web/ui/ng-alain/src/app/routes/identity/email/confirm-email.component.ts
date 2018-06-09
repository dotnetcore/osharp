import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ConfirmEmailDto, AjaxResult, AjaxResultType, AdResultDto } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-identity-confirm-email',
  // template: `
  // <h3 class="text-center" style="font-size: 20px; margin-bottom: 20px;">激活邮箱</h3>
  // <nz-alert nzType="{{messageType}}" nzMessage="{{message}}" [nzShowIcon]="true" class="mb-lg"></nz-alert>
  // `,
  templateUrl: './confirm-email.component.html'
})
export class ConfirmEmailComponent implements OnInit, AfterViewInit {

  dto: ConfirmEmailDto = new ConfirmEmailDto();
  result: AdResultDto = new AdResultDto();

  constructor(
    private http: HttpClient,
    private router: Router,
    private osharp: OsharpService
  ) { }

  ngOnInit(): void {
    this.getUrlParams();
    this.result.title = "正在激活注册邮箱……";
  }

  ngAfterViewInit(): void {
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
          this.result.type = 'info-circle-o';
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
