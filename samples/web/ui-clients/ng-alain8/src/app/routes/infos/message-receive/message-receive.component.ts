// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="message-receive.module.ts" company="OSharp开源团队">
//      Copyright (c) 2014-2019 Liuliu. All rights reserved.
//  </copyright>
//  <site>https://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector } from '@angular/core';
import { SFUISchema, SFSchema } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';

@Component({
  selector: 'app-message-receive',
  templateUrl: './message-receive.component.html',
  styles: []
})
export class MessageReceiveComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'messageReceive';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      {
        title: '操作', fixed: 'left', width: 65, buttons: [{
          text: '操作', children: [
            { text: '编辑', icon: 'edit', acl: 'Root.Admin.Infos.MessageReceive.Update', iif: row => row.Updatable, click: row => this.edit(row) },
            { text: '删除', icon: 'delete', type: 'del', acl: 'Root.Admin.Infos.MessageReceive.Delete', iif: row => row.Deletable, click: row => this.delete(row) },
          ]
        }]
      },
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'string' },
      { title: '接收时间', index: 'ReadDate', sort: true, filterable: true, type: 'date' },
      { title: '新回复数，接收者使用', index: 'NewReplyCount', sort: true, filterable: true, type: 'number' },
      { title: '接收的主消息编号', index: 'MessageId', sort: true, filterable: true, ftype: 'string' },
      { title: '消息接收人编号', index: 'UserId', sort: true, filterable: true, type: 'number' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
    ];
    return columns;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } }
    };
    return ui;
  }
}

