import { Component, OnInit, Inject } from '@angular/core';
import { ResetPasswordDto, AdResult, AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '../shared/identity.service';

@Component({
  selector: 'app-identity-reset-password',
  templateUrl: `./reset-password.component.html`
})
export class ResetPasswordComponent implements OnInit {

  dto: ResetPasswordDto = new ResetPasswordDto();
  result: AdResult = new AdResult();
  canSubmit = true;
  sended = false;

  constructor(
    public router: Router,
    private osharp: OsharpService,
    private identity: IdentityService
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
    this.identity.resetPassword(this.dto).then(res => {
      this.sended = true;
      this.canSubmit = true;
      this.result = res;
    });
  }
}
