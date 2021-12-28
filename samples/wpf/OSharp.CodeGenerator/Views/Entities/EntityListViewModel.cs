// -----------------------------------------------------------------------
//  <copyright file="EntityListViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-06 19:10</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Notifications.Wpf.Core;

using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Data;
using OSharp.CodeGenerator.Views.Modules;
using OSharp.Data;
using OSharp.Exceptions;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using PropertyChanged;

using Stylet;


namespace OSharp.CodeGenerator.Views.Entities
{
    [Singleton]
    public class EntityListViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public EntityListViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ModuleViewModel Module { get; set; }

        public IObservableCollection<EntityViewModel> Entities { get; } = new BindableCollection<EntityViewModel>();

        public bool IsShow { get; set; }

        public async void Init()
        {
            if (Module == null)
            {
                Helper.Notify("当前模块为空，请在左侧菜单选择一个“模块”节点", NotificationType.Error);
                return;
            }
            
            List<CodeEntity> entities = new List<CodeEntity>();
            await _provider.ExecuteScopedWorkAsync(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                entities = contract.CodeEntities.Where(m => m.ModuleId == Module.Id).OrderBy(m => m.Order).ToList();
                return Task.CompletedTask;
            });
            Entities.Clear();
            foreach (CodeEntity entity in entities)
            {
                EntityViewModel model = _provider.GetRequiredService<EntityViewModel>();
                model = entity.MapTo(model);
                model.Module = Module;
                Entities.Add(model);
            }
            
            Helper.Output($"模块“{Module.Display}”的实体列表刷新成功，共{Entities.Count}个实体");
        }

        public void New()
        {
            if (Module == null)
            {
                Helper.Notify("当前模块为空，请在左侧菜单选择一个“模块”节点", NotificationType.Error);
                return;
            }
            EntityViewModel entity = _provider.GetRequiredService<EntityViewModel>();
            entity.Module = Module;
            Entities.Add(entity);
        }

        public bool CanSave => Entities.All(m => !m.HasErrors);

        public async void Save()
        {
            if (!CanSave)
            {
                Helper.Notify("实体信息验证失败", NotificationType.Warning);
                return;
            }

            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Order = i + 1;
            }

            CodeEntityInputDto[] dtos = Entities.Select(m => m.MapTo<CodeEntityInputDto>()).ToArray();
            OperationResult result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                result = await contract.UpdateCodeEntities(dtos);
            });
            Helper.Notify(result);
            if (!result.Succeeded)
            {
                return;
            }
            Init();
        }

        /// <summary>
        /// Called whenever the error state of any properties changes. Calls NotifyOfPropertyChange("HasErrors") by default
        /// </summary>
        /// <param name="changedProperties">List of property names which have changed validation state</param>
        [SuppressPropertyChangedWarnings]
        protected override void OnValidationStateChanged(IEnumerable<string> changedProperties)
        {
            base.OnValidationStateChanged(changedProperties);
            NotifyOfPropertyChange(() => CanSave);
        }
    }
}
