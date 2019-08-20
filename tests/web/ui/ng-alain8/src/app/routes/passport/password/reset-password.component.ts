import { Component, OnInit } from '@angular/core';
import { ResetPasswordDto, AdResult } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { SFSchema, CustomWidget, FormProperty, PropertyGroup } from '@delon/form';
import { OsharpService } from '@shared/osharp/services/osharp.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styles: [
    `
  :host {
    display: block;
    width: 400px;
    margin: 0 auto;
  `,
  ],
})
export class ResetPasswordComponent implements OnInit {
  title = '重置登录密码';
  dto: ResetPasswordDto = new ResetPasswordDto();
  result: AdResult = new AdResult();
  schema: SFSchema;

  constructor(
    public router: Router,
    private osharp: OsharpService,
    public identity: IdentityService,
  ) {
    this.getUrlParams();
  }

  ngOnInit() {
    this.schema = {
      properties: {
        NewPassword: { title: '新密码', type: 'string', minLength: 6, ui: { widget: 'custom', spanLabel: 6, spanControl: 16, grid: { xs: 24 }, errors: { minLength: '密码至少6位' } } },
        ConfirmPassword: {
          title: '确认密码', type: 'string', ui: {
            widget: 'custom', spanLabel: 6, spanControl: 16, grid: { xs: 24 },
            validator: (value: any, formProperty: FormProperty, form: PropertyGroup) => form.value && value !== form.value.NewPassword ? [{ keyword: 'equalto', message: '两次输入的密码不一致' }] : []
          }
        },
      },
      required: ["NewPassword", "ConfirmPassword"],
      ui: { grid: { gutter: 16, xs: 24 } }
    };
  }

  private getUrlParams() {
    let url = window.location.hash;
    this.dto.UserId = this.osharp.getHashURLSearchParams(url, 'userId');
    this.dto.Token = this.osharp.getHashURLSearchParams(url, 'token');
  }

  newPasswordChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }

  confirmPasswordChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }

  submit(value: ResetPasswordDto) {
    this.dto.NewPassword = value.NewPassword;
    this.dto.ConfirmPassword = value.ConfirmPassword;
    this.identity.resetPassword(this.dto).then(res => {
      res.show = true;
      this.result = res;
    });
  }
}
