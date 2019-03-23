import { Component, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'app-profile-password',
  templateUrl: './password.component.html',
  styles: []
})
export class ProfilePasswordComponent implements OnInit {

  schema: SFSchema;

  constructor() { }

  ngOnInit() {
    this.schema = {
      properties: {
        OldPassword: { title: '旧密码', type: 'string', ui: { spanLabel: 6, spanControl: 16, grid: { xs: 24 } } },
        NewPassword: { title: '新密码', type: 'string', ui: { spanLabel: 6, spanControl: 16, grid: { xs: 24 } } },
        ComfirePassword: { title: '确认密码', type: 'string', ui: { spanLabel: 6, spanControl: 16, grid: { xs: 24 } } }
      },
      required: ['NewPassword', 'ComfirePassword'],
      ui: {}
    };
  }

}
