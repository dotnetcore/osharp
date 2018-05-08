import { Component, OnInit, AfterViewInit } from '@angular/core';
import { URLSearchParams } from "@angular/http";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'identity-confirm-email',
  template: `<p>{{message}}</p>`,
  styles: [``]
})
export class ConfirmEmailComponent implements OnInit, AfterViewInit {

  message: string = "正在激活注册邮箱，请稍候……";
  userId: string;
  code: string;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUrlParams();
  }

  ngAfterViewInit(): void {
    this.confirmEmail();
  }

  private getUrlParams() {
    let hash = window.location.hash;
    let index = hash.indexOf("?") + 1;
    if (index == 0) {
      return;
    }
    let query = hash.substr(index, hash.length - index);
    let params = new URLSearchParams(query);
    this.userId = (params.get('userId'));
    this.code = (params.get('code'));
  }

  private confirmEmail() {
    this.http.post("/api/identity/ConfirmEmail", { userId: this.userId, code: this.code }).subscribe((res: any) => {
      this.message = res.Content;
    });
  }
}
