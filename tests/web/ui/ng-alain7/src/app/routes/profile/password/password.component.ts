import { Component, OnInit } from '@angular/core';
import { SFSchema, CustomWidget } from '@delon/form';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { SettingsService } from '@delon/theme';
import { ChangePasswordDto } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-profile-password',
  templateUrl: './password.component.html',
  styles: []
})
export class ProfilePasswordComponent implements OnInit {

  schema: SFSchema;

  constructor(
    private settings: SettingsService,
    private identity: IdentityService
  ) { }

  ngOnInit() {
    let user = this.settings.user;
    this.schema = {
      properties: {
        OldPassword: { title: '旧密码', type: 'string', description: '初次设置密码时，旧密码留空', ui: { widget: 'custom', spanLabel: 4, } },
        NewPassword: { title: '新密码', type: 'string', minLength: 6, description: '密码至少6位，且包含字母数字', ui: { widget: 'custom', spanLabel: 4, } },
        ConfirmNewPassword: {
          title: '确认密码', type: 'string', minLength: 6, description: '确认密码需与新密码一致', ui: {
            widget: 'custom', spanLabel: 4,
            validator: (value, prop, form) => form.value && value !== form.value.NewPassword ? [{ keyword: 'equalto', message: '两次输入的密码不一致' }] : []
          }
        }
      },
      required: ['NewPassword', 'ConfirmNewPassword'],
      ui: {}
    };
  }

  oldPasswordChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }
  newPasswordChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }
  comfirePasswordChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }

  submit(value: ChangePasswordDto) {
    this.identity.changePassword(value);
  }
}
