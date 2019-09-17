// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="infos.module.ts" company="OSharp开源团队">
//      Copyright (c) 2014-2019 Liuliu. All rights reserved.
//  </copyright>
//  <site>https://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { InfosRoutingModule } from './infos.routing';
import { MessageComponent } from './message/message.component';
import { MessageReceiveComponent } from './message-receive/message-receive.component';
import { MessageReplyComponent } from './message-reply/message-reply.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    InfosRoutingModule
  ],
  declarations: [
    MessageComponent,
    MessageReceiveComponent,
    MessageReplyComponent,
  ]
})
export class InfosModule { }
