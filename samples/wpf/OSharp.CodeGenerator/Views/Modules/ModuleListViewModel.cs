// -----------------------------------------------------------------------
//  <copyright file="ModuleListViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-06 12:33</last-date>
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
using OSharp.CodeGenerator.Views.LoadFromEntities;
using OSharp.CodeGenerator.Views.Projects;
using OSharp.Data;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using PropertyChanged;

using Stylet;


namespace OSharp.CodeGenerator.Views.Modules
{
    [Singleton]
    public class ModuleListViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public ModuleListViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ProjectViewModel Project { get; set; }

        public IObservableCollection<ModuleViewModel> Modules { get; } = new BindableCollection<ModuleViewModel>();

        public bool IsShow { get; set; } = true;

        public async void Init()
        {
            if (Project == null)
            {
                Helper.Notify("当前项目为空，请在左侧菜单选择“项目”节点", NotificationType.Error);
                return;
            }
            
            List<CodeModule> entities = new List<CodeModule>();
            await _provider.ExecuteScopedWorkAsync(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                entities = contract.CodeModules.Where(m => m.ProjectId == Project.Id).OrderBy(m => m.Order).ToList();
                return Task.CompletedTask;
            });
            Modules.Clear();
            foreach (CodeModule entity in entities)
            {
                ModuleViewModel model = _provider.GetRequiredService<ModuleViewModel>();
                model = entity.MapTo(model);
                model.Project = Project;
                Modules.Add(model);
            }

            Helper.Output($"模块列表刷新成功，共{Modules.Count}个模块");
        }

        public void New()
        {
            if (Project == null)
            {
                Helper.Notify("当前项目为空，请在左侧菜单选择“项目”节点", NotificationType.Error);
                return;
            }
            ModuleViewModel model = IoC.Get<ModuleViewModel>();
            model.Project = Project;
            Modules.Add(model);
        }

        public bool CanSave => Modules.All(m => !m.HasErrors);

        public async void Save()
        {
            if (!CanSave)
            {
                Helper.Notify("模块信息验证失败", NotificationType.Warning);
                return;
            }

            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].Order = i + 1;
            }

            CodeModuleInputDto[] dtos = Modules.Select(m => m.MapTo<CodeModuleInputDto>()).ToArray();
            OperationResult result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                result = await contract.UpdateCodeModules(dtos);
            });
            Helper.Notify(result);
            if (!result.Succeeded)
            {
                return;
            }
            Init();
        }

        public void LoadFromEntities()
        {
            LoadFromEntitiesViewModel model = IoC.Get<LoadFromEntitiesViewModel>();
            model.IsShow = true;
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
