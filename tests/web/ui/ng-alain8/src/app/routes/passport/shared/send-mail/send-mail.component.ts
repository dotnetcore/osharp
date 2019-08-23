import { Component, OnInit, AfterViewInit, Input, Output, EventEmitter, ViewChild, } from '@angular/core';
import { SFSchema, CustomWidget, SFComponent } from '@delon/form';
import { SendMailDto, VerifyCode, AdResult } from '@shared/osharp/osharp.model';
import { Router } from '@angular/router';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { _HttpClient } from '@delon/theme';

@Component({
  selector: 'passport-send-mail',
  templateUrl: './send-mail.component.html',
  styles: [`
  :host {
    display: block;
    width: 400px;
    margin: 0 auto;
  `]
})
export class SendMailComponent implements OnInit, AfterViewInit {
  schema: SFSchema;
  dto: SendMailDto = new SendMailDto();
  code: VerifyCode = new VerifyCode();
  result: AdResult = new AdResult();

  @Input() title: string;
  @Output() submited = new EventEmitter<SendMailDto>();
  @ViewChild('sf', { static: false }) sf: SFComponent;

  constructor(
    public router: Router,
    public osharp: OsharpService,
    public http: _HttpClient
  ) { }

  ngOnInit() {
    this.schema = {
      properties: {
        Email: {
          title: '邮箱', type: 'string', format: 'regex', pattern: '^[\\w\\._-]+@[\\w_\-]+\\.[A-Za-z]{2,4}$', ui: {
            placeholder: '请输入电子邮箱', prefixIcon: 'mail', spanLabel: 6, spanControl: 16, grid: { xs: 24 },
            errors: { pattern: '电子邮箱格式不正确，应形如xxx@xxx.xxx' }, validator: (value: any) => this.osharp.remoteInverseSFValidator(`api/identity/CheckEmailExists?email=${value}`, { keyword: 'remote', message: '输入的电子邮箱不存在' })
          }
        },
        VerifyCode: {
          title: '验证码', type: 'string', ui: {
            widget: 'custom', spanLabel: 6, spanControl: 16, grid: { xs: 24 },
            validator: (value: any) => this.osharp.remoteInverseSFValidator(`api/common/CheckVerifyCode?code=${value}&id=${this.code.id}`, { keyword: 'remote', message: '验证码不正确，请刷新重试' })
          }
        },
        VerifyCodeId: { type: 'string', ui: { hidden: true } }
      },
      required: ['Email', 'VerifyCode'],
      ui: { grid: { gutter: 16, xs: 24 } }
    };
  }

  ngAfterViewInit() {
    this.refreshCode();
  }

  refreshCode() {
    this.osharp.refreshVerifyCode().subscribe(vc => {
      this.code = vc;
    });
  }

  emailChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }
  verifyCodeChange(me: CustomWidget, value: string) {
    me.setValue(value);
  }

  submit(value: SendMailDto) {
    if (!this.sf.valid) {
      return;
    }
    value.VerifyCodeId = this.code.id;
    this.submited.emit(value);
  }

  back() {
    if (window.history) {
      window.history.back();
    }
  }
}
