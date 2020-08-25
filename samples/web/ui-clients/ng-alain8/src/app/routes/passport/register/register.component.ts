import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { AdResult, RegisterDto, VerifyCode } from '@shared/osharp/osharp.model';
import { SFSchema, FormProperty, PropertyGroup, CustomWidget } from '@delon/form';

@Component({
  selector: 'app-passport-register',
  templateUrl: './register.component.html',
  styles: [`
  :host {
    display: block;
    width: 400px;
    margin: 0 auto;
  `]
})
export class RegisterComponent implements OnInit, AfterViewInit {

  schema: SFSchema;
  dto: RegisterDto = new RegisterDto();
  code: VerifyCode = new VerifyCode();
  result: AdResult = new AdResult();

  constructor(public router: Router, public osharp: OsharpService, public identity: IdentityService) { }

  ngOnInit(): void {
    this.schema = {
      properties: {
        Email: {
          title: '电子邮箱', type: 'string', format: 'regex', pattern: '^[\\w\\._-]+@[\\w_\-]+\\.[A-Za-z]{2,4}$', ui: {
            placeholder: '请输入电子邮箱', size: 'large', prefixIcon: 'mail', spanLabel: 6, spanControl: 16, grid: { xs: 24 },
            errors: { pattern: '电子邮箱格式不正确，应形如xxx@xxx.xxx' }, validator: (value) => {
              if (!value) {
                return [{ keyword: 'required', message: '电子邮箱不能为空' }];
              }
              return this.osharp.remoteSFValidator(`api/identity/CheckEmailExists?email=${value}`, { keyword: 'remote', message: '输入的电子邮箱已存在，请更换' });
            }
          }
        },
        Password: { title: '新密码', type: 'string', minLength: 6, ui: { widget: 'custom', spanLabel: 6, spanControl: 16, grid: { xs: 24 }, errors: { minLength: '密码至少6位' } } },
        ConfirmPassword: {
          title: '确认密码', type: 'string', ui: {
            widget: 'custom', spanLabel: 6, spanControl: 16, grid: { xs: 24 },
            validator: (value: any, formProperty: FormProperty, form: PropertyGroup) => form.value && value !== form.value.Password ? [{ keyword: 'equalto', message: '两次输入的密码一致' }] : []
          }
        },
        VerifyCode: {
          title: '验证码', type: 'string', ui: {
            widget: 'custom', spanLabel: 6, spanControl: 16, grid: { xs: 24 },
            validator: (value: any) => {
              if (!value || !this.code.id) {
                return [{ keyword: 'required', message: '验证码不能为空' }];
              }
              return this.osharp.remoteInverseSFValidator(`api/common/CheckVerifyCode?code=${value}&id=${this.code.id}`, { keyword: 'remote', message: '验证码不正确，请刷新重试' });
            }
          }
        },
        VerifyCodeId: { type: 'string', ui: { hidden: true } }
      },
      required: ["Email", "Password", "ConfirmPassword", "VerifyCode"],
      ui: { grid: { gutter: 16, xs: 24 } }
    };
  }

  ngAfterViewInit() {
    this.refreshCode();
  }

  passwordChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }

  confirmPasswordChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }

  refreshCode() {
    this.osharp.refreshVerifyCode().subscribe(vc => {
      this.code = vc;
      this.dto.VerifyCodeId = vc.id;
    });
  }
  verifyCodeChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }

  submit(value: RegisterDto) {
    this.dto.Email = value.Email;
    this.dto.Password = value.Password;
    this.dto.ConfirmPassword = value.ConfirmPassword;
    this.dto.VerifyCode = value.VerifyCode;
    this.identity.register(this.dto).then(res => {
      res.show = true;
      this.result = res;
    });
  }
}
