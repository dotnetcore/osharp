import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STReq, STRes, STComponent } from '@delon/abc';
import { PageRequest } from '@shared/osharp/osharp.model';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';

@Component({
  selector: 'app-profile-oauth2',
  templateUrl: './oauth2.component.html',
  styles: []
})
export class ProfileOauth2Component implements OnInit {

  url = 'api/identity/ReadOAuth2';
  pageRequest: PageRequest;
  columns: STColumn[];
  req: STReq;
  res: STRes;
  @ViewChild("st") st: STComponent;

  constructor(private identity: IdentityService, private modal: NzModalService) { }

  ngOnInit() {
    this.pageRequest = new PageRequest();
    this.columns = [
      { title: '服务提供商', index: 'LoginProvider' },
      { title: '头像', index: 'Avatar', type: 'img', width: 50 },
      { title: '昵称', index: 'ProviderDisplayName' },
      { title: '绑定时间', index: 'CreatedTime', type: 'date' },
      {
        title: '操作', buttons: [{
          text: '解除绑定', icon: 'stop', click: (record) => {
            this.modal.confirm({
              nzTitle: '请确认', nzContent: `是否要解除 “${record.LoginProvider} - ${record.ProviderDisplayName}” 的第三方登录绑定？`, nzOnOk: () => {
                this.identity.removeOAuth2(record.Id).then(() => this.st.reload());
              }
            })
          }
        }]
      }
    ];
    this.req = { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: this.pageRequest };
    this.res = { reName: { list: 'Rows', total: 'Total' } };
  }

}
