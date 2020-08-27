// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="message.module.ts" company="OSharp开源团队">
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
  selector: 'app-message',
  templateUrl: './message.component.html',
  styles: []
})
export class MessageComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'message';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      {
        title: '操作', fixed: 'left', width: 65, buttons: [{
          text: '操作', children: [
            { text: '编辑', icon: 'edit', acl: 'Root.Admin.Infos.Message.Update', iif: row => row.Updatable, click: row => this.edit(row) },
            { text: '删除', icon: 'delete', type: 'del', acl: 'Root.Admin.Infos.Message.Delete', iif: row => row.Deletable, click: row => this.delete(row) },
          ]
        }]
      },
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'string' },
      { title: '标题', index: 'Title', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '内容', index: 'Content', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '消息类型', index: 'MessageType', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '新回复数', index: 'NewReplyCount', sort: true, filterable: true, type: 'number' },
      { title: '是否发送', index: 'IsSended', sort: true, editable: true, filterable: true, type: 'yn' },
      { title: '是否允许回复', index: 'CanReply', sort: true, editable: true, filterable: true, type: 'yn' },
      { title: '生效时间', index: 'BeginDate', sort: true, editable: true, filterable: true, type: 'date' },
      { title: '过期时间', index: 'EndDate', sort: true, editable: true, filterable: true, type: 'date' },
      { title: '发送人编号', index: 'SenderId', sort: true, filterable: true, type: 'number' },
      { title: '是否锁定', index: 'IsLocked', sort: true, editable: true, filterable: true, type: 'yn' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
    ];
    return columns;
  }

  protected GetSFSchema(): SFSchema {
    let schema: SFSchema = {
      properties: this.ColumnsToSchemas(this.columns),
      required: ['Title', 'Content']
    };
    return schema;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Title: { grid: { span: 24 } },
      $Content: { grid: { span: 24 } }
    };
    return ui;
  }
}

