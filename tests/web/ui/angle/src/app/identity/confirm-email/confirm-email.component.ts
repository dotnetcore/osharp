import { Component, OnInit, AfterViewInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { osharp } from '../../shared/osharp';
import { ConfirmEmailDto } from '../../shared/osharp/osharp.model';
import { Router } from '@angular/router';

@Component({
  selector: 'identity-confirm-email',
  template: `
  <div class="text-center">{{message}}</div>
  `,
  styles: [``]
})
export class ConfirmEmailComponent implements OnInit, AfterViewInit {

  confirmEmailDto: ConfirmEmailDto = new ConfirmEmailDto();
  message: string = "正在激活注册邮箱，请稍候……";
  messageType: string = "success";

  constructor(private http: HttpClient, public router: Router) { }

  ngOnInit(): void {
    this.getUrlParams();
  }

  ngAfterViewInit(): void {
    this.confirmEmail();
  }

  private getUrlParams() {
    let url = window.location.hash;
    this.confirmEmailDto.UserId = osharp.Tools.getHashURLSearchParams(url, "userId");
    this.confirmEmailDto.Code = osharp.Tools.getHashURLSearchParams(url, "code");
  }

  private confirmEmail() {
    this.http.post("/api/identity/ConfirmEmail", this.confirmEmailDto).subscribe((res: any) => {
      this.message = res.Content;
      if (res.Type == "Error") {
        this.messageType = "danger";
      } else if (res.Type == "Info") {
        this.messageType = "info";
      }
      else {
        this.message = "注册邮箱激活成功";
        this.messageType = "success";
        setTimeout(() => {
          this.router.navigateByUrl('/home');
        }, 3000);
      }
    });
  }
}
