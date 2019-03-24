import { Component, OnInit } from '@angular/core';
import { SFSchema, CustomWidget } from '@delon/form';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { SettingsService } from '@delon/theme';

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
        UserId: { type: 'number', default: user.id, ui: { hidden: true } },
        OldPassword: { title: '旧密码', type: 'string', ui: { widget: 'custom', spanLabel: 4, } },
        NewPassword: { title: '新密码', type: 'string', minLength: 6, ui: { widget: 'custom', spanLabel: 4, } },
        ConfirmNewPassword: {
          title: '确认密码', type: 'string', minLength: 6, ui: {
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

  submit(value) {
    this.identity.changePassword(value);
  }
}
