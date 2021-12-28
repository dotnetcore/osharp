// -----------------------------------------------------------------------
//  <copyright file="ForeignViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-13 11:09</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Data;
using OSharp.Data;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using Stylet;


namespace OSharp.CodeGenerator.Views.Entities
{
    [MapTo(typeof(CodeForeignInputDto))]
    [MapFrom(typeof(CodeForeign))]
    public class ForeignViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public ForeignViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Guid Id { get; set; }

        public string SelfNavigation { get; set; }

        public string SelfForeignKey { get; set; }

        public string OtherEntity { get; set; }

        public string OtherNavigation { get; set; }

        public ForeignRelation ForeignRelation { get; set; }

        public DeleteBehavior? DeleteBehavior { get; set; }

        public bool IsRequired { get; set; } = true;

        public Guid EntityId { get; set; }
        
        public void SelfNavigationChanged(SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count == 0 || args.AddedItems[0] is not string propName)
            {
                return;
            }

            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                CodeProperty selfNav = contract.CodeProperties.First(m => m.Name == propName && m.IsNavigation && m.EntityId == EntityId);
                CodeProperty selfKey = contract.CodeProperties
                    .FirstOrDefault(m => m.EntityId == EntityId && m.IsForeignKey && m.RelateEntity == selfNav.RelateEntity);
                SelfForeignKey = selfKey?.Name;
                OtherEntity = selfKey?.RelateEntity;
            });
        }

        public void SelfForeignKeyChanged(SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count == 0 || args.AddedItems[0] is not string propName)
            {
                return;
            }

            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                CodeProperty selfKey = contract.CodeProperties.First(m => m.Name == propName && m.IsForeignKey && m.EntityId == EntityId);
                CodeProperty selfNav = contract.CodeProperties
                    .FirstOrDefault(m => m.EntityId == EntityId && m.IsNavigation && m.RelateEntity == selfKey.RelateEntity);
                SelfNavigation = selfNav?.Name;
                OtherEntity = selfNav?.RelateEntity;
            });
        }

        public async void Delete()
        {
            if (MessageBox.Show($"是否删除实体外键“{SelfNavigation??SelfForeignKey}”？", "请确认", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            OperationResult result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                result = await contract.DeleteCodeForeigns(Id);
            });
            Helper.Notify(result);
            if (!result.Succeeded)
            {
                return;
            }

            ForeignListViewModel list = IoC.Get<ForeignListViewModel>();
            list.Init();
        }
    }
}
