import { Component, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { SettingsService, _HttpClient } from '@delon/theme';
import { UploadChangeParam } from 'ng-zorro-antd';
import { AjaxResult, ProfileEditDto } from '@shared/osharp/osharp.model';
import { IdentityService } from '@shared/osharp/services/identity.service';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './edit.component.html',
  styles: []
})
export class ProfileEditComponent implements OnInit {

  schema: SFSchema;
  headImgUrl: string;

  constructor(
    private settings: SettingsService,
    private identity: IdentityService
  ) { }

  ngOnInit() {
    let user = this.settings.user;
    let userNameEdit = ['QQ_', 'Microsoft_', 'GitHub_'].some(m => user.name.startsWith(m));
    this.headImgUrl = user.avatar;
    this.schema = {
      properties: {
        UserName: { title: '用户名', type: 'string', default: user.name, readOnly: !userNameEdit, ui: { spanLabel: 4, placeholder: '可由字母、数字、下划线、点组成，全局唯一' } },
        NickName: { title: '昵称', type: 'string', default: user.nickName, description: '昵称用于显示名称', ui: { spanLabel: 4, placeholder: '推荐使用中文' } },
        Email: { title: '电子邮箱', type: 'string', default: user.email, description: '邮箱是接收通知，重置密码的主要途径', ui: { spanLabel: 4, placeholder: '邮箱格式为：xxx@xxx.xxx' } },
        // TODO: 修改头像待优化，应能预览能裁剪
        HeadImg: {
          title: '头像', type: 'string', default: user.avatar, ui: {
            spanLabel: 4, widget: 'upload', action: 'api/common/UploadImage', resReName: 'Data', urlReName: 'Data', change: (args: UploadChangeParam) => {
              if (args.type === 'success' && args.file.response.Data) {
                this.headImgUrl = args.file.response.Data;
              }
            }
          }
        }
      },
      required: ["UserName", "NickName"]
    };
  }

  submit(value: ProfileEditDto) {
    value.HeadImg = this.headImgUrl;
    this.identity.profileEdit(value);
  }
}
