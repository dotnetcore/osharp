// -----------------------------------------------------------------------
//  <copyright file="PropertyListViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 13:44</last-date>
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
using OSharp.CodeGenerator.Views.Entities;
using OSharp.Data;
using OSharp.Exceptions;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using PropertyChanged;

using Stylet;


namespace OSharp.CodeGenerator.Views.Properties
{
    [Singleton]
    public class PropertyListViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public PropertyListViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public EntityViewModel Entity { get; set; }

        public IObservableCollection<PropertyViewModel> Properties { get; set; } = new BindableCollection<PropertyViewModel>();

        public bool IsShow { get; set; }
        
        public async void Init()
        {
            if (Entity == null)
            {
                Helper.Notify("当前实体为空，请在左侧菜单选择一个“实体”节点", NotificationType.Error);
                return;
            }
            
            List<CodeProperty> properties = new List<CodeProperty>();
            await _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                properties = contract.CodeProperties.Where(m => m.EntityId == Entity.Id).OrderBy(m => m.Order).ToList();
                return Task.CompletedTask;
            });
            Properties.Clear();
            foreach (CodeProperty property in properties)
            {
                PropertyViewModel model = _provider.GetRequiredService<PropertyViewModel>();
                model = property.MapTo(model);
                model.Entity = Entity;
                Properties.Add(model);
            }
            
            Helper.Output($"实体“{Entity.Display}”的属性列表刷新成功，共{Properties.Count}个属性");
        }

        public void New()
        {
            if (Entity == null)
            {
                Helper.Notify("当前实体为空，请在左侧菜单选择一个“实体”节点", NotificationType.Error);
                return;
            }

            PropertyViewModel property = _provider.GetRequiredService<PropertyViewModel>();
            property.Entity = Entity;
            Properties.Add(property);
        }

        public bool CanSave => Properties.All(m => !m.HasErrors);

        public async void Save()
        {
            if (!CanSave)
            {
                Helper.Notify("属性信息验证失败", NotificationType.Warning);
                return;
            }

            for (int i = 0; i < Properties.Count; i++)
            {
                Properties[i].Order = i + 1;
            }

            CodePropertyInputDto[] dtos = Properties.Select(m => m.MapTo<CodePropertyInputDto>()).ToArray();
            OperationResult result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                result = await contract.UpdateCodeProperties(dtos);
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
