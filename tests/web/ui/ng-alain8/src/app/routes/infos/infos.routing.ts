// -----------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="infos.routing.ts" company="OSharp开源团队">
//      Copyright (c) 2014-2019 Liuliu. All rights reserved.
//  </copyright>
//  <site>https://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
// -----------------------------------------------------------------------

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ACLGuard } from '@delon/acl';
import { MessageComponent } from './message/message.component';
import { MessageReceiveComponent } from './message-receive/message-receive.component';
import { MessageReplyComponent } from './message-reply/message-reply.component';

const routes: Routes = [
  { path: 'message', component: MessageComponent, canActivate: [ACLGuard], data: { title: '站内信管理', reuse: true, guard: 'Root.Admin.Infos.Message.Read' } },
  { path: 'message-receive', component: MessageReceiveComponent, canActivate: [ACLGuard], data: { title: '站内信接收记录管理', reuse: true, guard: 'Root.Admin.Infos.MessageReceive.Read' } },
  { path: 'message-reply', component: MessageReplyComponent, canActivate: [ACLGuard], data: { title: '站内信回复管理', reuse: true, guard: 'Root.Admin.Infos.MessageReply.Read' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InfosRoutingModule { }
