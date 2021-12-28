// -----------------------------------------------------------------------
//  <copyright file="TemplateListViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-12 15:41</last-date>
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
using OSharp.Data;
using OSharp.Entity;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using PropertyChanged;

using Stylet;


namespace OSharp.CodeGenerator.Views.Templates
{
    [Singleton]
    public class TemplateListViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public TemplateListViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public bool IsShow { get; set; }

        public IObservableCollection<TemplateViewModel> Templates { get; set; } = new BindableCollection<TemplateViewModel>();

        public void Init()
        {
            CodeTemplate[] templates = new CodeTemplate[0];
            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                templates = contract.CodeTemplates.OrderBy(m => !m.IsSystem).ThenBy(m => m.Order).ToArray();
            });
            Templates.Clear();
            foreach (CodeTemplate template in templates)
            {
                TemplateViewModel model = _provider.GetRequiredService<TemplateViewModel>();
                model = template.MapTo(model);
                Templates.Add(model);
            }
        }

        public void New()
        {
            TemplateViewModel model = IoC.Get<TemplateViewModel>();
            Templates.Add(model);
        }

        public bool CanSave => Templates.All(m => !m.HasErrors);
        public async void Save()
        {
            if (!CanSave)
            {
                Helper.Notify("模板信息验证失败", NotificationType.Warning);
                return;
            }

            for (int i = 0; i < Templates.Count; i++)
            {
                Templates[i].Order = i + 1;
            }

            CodeTemplateInputDto[] dtos = Templates.Select(m => m.MapTo<CodeTemplateInputDto>()).ToArray();
            OperationResult<CodeTemplate[]> result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                result = await contract.UpdateCodeTemplates(dtos);
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
