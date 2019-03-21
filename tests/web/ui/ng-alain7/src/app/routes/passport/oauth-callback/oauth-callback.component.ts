import { Component, OnInit } from '@angular/core';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { UserLoginInfoEx } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-oauth-callback',
  templateUrl: './oauth-callback.component.html',
  styles: []
})
export class OauthCallbackComponent implements OnInit {

  loginInfo: UserLoginInfoEx = new UserLoginInfoEx();

  constructor(private osharp: OsharpService) { }

  ngOnInit() {
    this.getUrlParams();
  }

  private getUrlParams() {
    let url = window.location.hash;
    this.loginInfo = {
      LoginProvider: this.osharp.getHashURLSearchParams(url, "provider"),
      ProviderKey: this.osharp.getHashURLSearchParams(url, "key"),
      ProviderDisplayName: this.osharp.getHashURLSearchParams(url, "name"),
      AvatarUrl: this.osharp.getHashURLSearchParams(url, "avatar")
    };

  }
}
