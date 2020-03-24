// -----------------------------------------------------------------------
//  <copyright file="InfosService.Message.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-12 0:11</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Liuliu.Demo.Identity.Entities;
using Liuliu.Demo.Infos.Dtos;
using Liuliu.Demo.Infos.Entities;
using Liuliu.Demo.Infos.Events;

using OSharp.Collections;
using OSharp.Data;
using OSharp.Exceptions;


namespace Liuliu.Demo.Infos
{
    public partial class InfosService
    {
        /// <summary>
        /// 添加站内信信息
        /// </summary>
        /// <param name="dtos">要添加的站内信信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public override async Task<OperationResult> CreateMessages(params MessageInputDto[] dtos)
        {
            Check.Validate<MessageInputDto, Guid>(dtos, nameof(dtos));
            List<Message> messages = new List<Message>();

            OperationResult result = await MessageRepository.InsertAsync(dtos,
                dto =>
                {
                    if (dto.MessageType == MessageType.Private && dto.RecipientIds.IsNullOrEmpty())
                    {
                        throw new OsharpException("私有消息的接收用户不能为空");
                    }
                    return Task.CompletedTask;
                },
                async (dto, entity) =>
                {
                    User sender = await UserRepository.GetAsync(dto.SenderId);
                    if (sender == null)
                    {
                        throw new OsharpException($"编号为“{dto.SenderId}”的发件人不存在");
                    }
                    entity.Sender = sender;

                    if (entity.MessageType == MessageType.System)
                    {
                        entity.CanReply = false;
                    }
                    else
                    {
                        if (!dto.PublicRoleIds.IsNullOrEmpty())
                        {
                            entity.PublicRoles = RoleRepository.Query(m => dto.PublicRoleIds.Contains(m.Id)).ToList();
                        }

                        if (!dto.RecipientIds.IsNullOrEmpty())
                        {
                            entity.Recipients = UserRepository.Query(m => dto.RecipientIds.Contains(m.Id)).ToList();
                        }
                    }

                    messages.Add(entity);
                    return entity;
                });

            if (result.Succeeded)
            {
                MessageCreatedEventData eventData = new MessageCreatedEventData() { Messages = messages.ToArray() };
                await EventBus.PublishAsync(eventData);
            }

            return result;
        }

        /// <summary>
        /// 更新站内信信息
        /// </summary>
        /// <param name="dtos">包含更新信息的站内信信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public override Task<OperationResult> UpdateMessages(params MessageInputDto[] dtos)
        {
            throw new NotImplementedException();
        }
    }
}