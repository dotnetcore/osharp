// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="message-reply.module.ts" company="OSharp开源团队">
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
  selector: 'app-message-reply',
  templateUrl: './message-reply.component.html',
  styles: []
})
export class MessageReplyComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'messageReply';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      {
        title: '操作', fixed: 'left', width: 65, buttons: [{
          text: '操作', children: [
            { text: '编辑', icon: 'edit', acl: 'Root.Admin.Infos.MessageReply.Update', iif: row => row.Updatable, click: row => this.edit(row) },
            { text: '删除', icon: 'delete', type: 'del', acl: 'Root.Admin.Infos.MessageReply.Delete', iif: row => row.Deletable, click: row => this.delete(row) },
          ]
        }]
      },
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'string' },
      { title: '消息内容', index: 'Content', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '是否已读', index: 'IsRead', sort: true, editable: true, filterable: true, type: 'yn' },
      { title: ' 消息回复人编号', index: 'UserId', sort: true, editable: true, filterable: true, type: 'number' },
      { title: '回复所属主消息，用于避免递归查询', index: 'BelongMessageId', sort: true, filterable: true, ftype: 'string' },
      { title: '回复的主消息，当回复主消息时有效', index: 'ParentMessageId', sort: true, filterable: true, ftype: 'string' },
      { title: '回复的回复消息，当回复回复消息时有效', index: 'ParentReplyId', sort: true, filterable: true, ftype: 'string' },
      { title: '是否锁定', index: 'IsLocked', sort: true, editable: true, filterable: true, type: 'yn' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
    ];
    return columns;
  }

  protected GetSFSchema(): SFSchema {
    let schema: SFSchema = {
      properties: this.ColumnsToSchemas(this.columns),
      required: ['Content']
    };
    return schema;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Content: { grid: { span: 24 } }
    };
    return ui;
  }
}

