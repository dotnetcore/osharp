// -----------------------------------------------------------------------
//  <copyright file="ForeignListViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-13 11:10</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Notifications.Wpf.Core;

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
    [Singleton]
    public class ForeignListViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public ForeignListViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public bool IsShow { get; set; }

        public string Title { get; set; }

        public IObservableCollection<ForeignViewModel> Foreigns { get; set; } = new BindableCollection<ForeignViewModel>();

        public EntityViewModel Entity { get; set; }

        public IObservableCollection<string> SelfNavigations
        {
            get
            {
                string[] props = GetPropNames(m => m.EntityId == Entity.Id && m.IsNavigation);
                return new BindableCollection<string>(props);
            }
        }

        public IObservableCollection<string> SelfForeignKeys
        {
            get
            {
                string[] props = GetPropNames(m => m.EntityId == Entity.Id && m.IsForeignKey);
                return new BindableCollection<string>(props);
            }
        }

        public IObservableCollection<string> OtherEntities { get; } = new BindableCollection<string>();

        public IObservableCollection<string> OtherNavigations { get; } = new BindableCollection<string>();

        public ForeignRelation[] ForeignRelations { get; } = { ForeignRelation.ManyToOne, ForeignRelation.OneToMany, ForeignRelation.OneToOne, ForeignRelation.OwnsOne, ForeignRelation.OwnsMany };

        public DeleteBehavior?[] DeleteBehaviors { get; } = { null, DeleteBehavior.ClientSetNull, DeleteBehavior.Restrict, DeleteBehavior.SetNull, DeleteBehavior.Cascade };

        public void Init()
        {
            CodeForeign[] foreigns = new CodeForeign[0];
            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                foreigns = contract.CodeForeigns.Where(m => m.EntityId == Entity.Id).ToArray();
            });
            Foreigns.Clear();
            foreach (CodeForeign foreign in foreigns)
            {
                ForeignViewModel model = _provider.GetRequiredService<ForeignViewModel>();
                model = foreign.MapTo(model);
                Foreigns.Add(model);
            }
        }

        public void New()
        {
            ForeignViewModel model = IoC.Get<ForeignViewModel>();
            model.EntityId = Entity.Id;
            Foreigns.Add(model);
        }

        public bool CanSave => Foreigns.All(m => !m.HasErrors);
        public async void Save()
        {
            if (!CanSave)
            {
                Helper.Notify("实体外键信息验证失败", NotificationType.Warning);
                return;
            }

            CodeForeignInputDto[] dtos = Foreigns.Select(m => m.MapTo<CodeForeignInputDto>()).ToArray();
            OperationResult result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                result = await contract.UpdateCodeForeigns(dtos);
            });
            Helper.Notify(result);
            if (!result.Succeeded)
            {
                return;
            }

            Init();
        }

        private string[] GetPropNames(Expression<Func<CodeProperty, bool>> exp)
        {
            string[] props = new string[0];
            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                props = contract.CodeProperties.Where(exp).Select(m => m.Name).ToArray();
            });
            return props;
        }
    }
}
