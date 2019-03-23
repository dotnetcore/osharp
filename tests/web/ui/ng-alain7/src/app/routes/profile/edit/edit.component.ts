import { Component, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { SettingsService } from '@delon/theme';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './edit.component.html',
  styles: []
})
export class ProfileEditComponent implements OnInit {

  schema: SFSchema;

  constructor(
    private settings: SettingsService
  ) { }

  ngOnInit() {
    let user = this.settings.user;
    this.schema = {
      properties: {
        UserName: { title: '用户名', type: 'string', default: user.name, ui: { spanLabel: 4, placeholder: '可由字母、数字、下划线、点组成，全局唯一' } },
        NickName: { title: '昵称', type: 'string', default: user.nickName, description: '昵称用于显示名称', ui: { spanLabel: 4, placeholder: '推荐使用中文' } },
        Email: { title: '电子邮箱', type: 'string', default: user.email, description: '邮箱是接收通知，重置密码的主要途径', ui: { spanLabel: 4, placeholder: '邮箱格式为：xxx@xxx.xxx' } },
        PhoneNumber: { title: '手机号', type: 'string', ui: { spanLabel: 4 } },
        HeadImg: { title: '头像', type: 'string', ui: { spanLabel: 4, widget: 'upload', action: 'api/common/UploadImage', resReName: 'Data', grid: { xs: 12 } } }
      },
      required: ["UserName", "NickName"]
    };
  }

  submit(value: any) {
    console.log(value);
  }
}
