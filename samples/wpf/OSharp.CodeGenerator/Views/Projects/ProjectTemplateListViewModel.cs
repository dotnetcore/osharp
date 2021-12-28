// -----------------------------------------------------------------------
//  <copyright file="ProjectTemplateListViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-12 20:02</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using Notifications.Wpf.Core;

using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Data;
using OSharp.CodeGenerator.Views.Templates;
using OSharp.Data;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using PropertyChanged;

using Stylet;


namespace OSharp.CodeGenerator.Views.Projects
{
    [Singleton]
    public class ProjectTemplateListViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public ProjectTemplateListViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ProjectViewModel Project { get; set; }

        public string Title { get; set; }

        public IObservableCollection<ProjectTemplateViewModel> ProjectTemplates { get; set; } = new BindableCollection<ProjectTemplateViewModel>();

        public bool IsShow { get; set; }

        public IObservableCollection<TemplateViewModel> TemplateSource { get; set; } = new BindableCollection<TemplateViewModel>();

        public void Init()
        {
            if (Project == null)
            {
                Helper.Notify("当前项目为空，请先通过菜单“项目-项目管理”加载项目", NotificationType.Error);
                return;
            }
            ProjectTemplateViewModel[] models = new ProjectTemplateViewModel[0];
            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                models = contract.CodeProjectTemplates.Where(m => m.ProjectId == Project.Id).OrderBy(m => m.Template.Order).Select(m => new ProjectTemplateViewModel(_provider)
                {
                    Id = m.Id,
                    ProjectId = m.ProjectId,
                    TemplateId = m.TemplateId,
                    IsLocked = m.IsLocked,
                    TemplateName = m.Template.Name
                }).ToArray();
            });
            ProjectTemplates.Clear();
            foreach (ProjectTemplateViewModel model in models)
            {
                ProjectTemplates.Add(model);
            }
        }

        public void New()
        {
            ProjectTemplateViewModel model = IoC.Get<ProjectTemplateViewModel>();
            model.ProjectId = Project.Id;
            model.TemplateSource = GetTemplates(Project.Id);
            ProjectTemplates.Add(model);
        }

        private BindableCollection<TemplateViewModel> GetTemplates(Guid projectId)
        {
            CodeTemplate[] templates = new CodeTemplate[0];
            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                Guid[] existIds = contract.CodeProjectTemplates.Where(m => m.ProjectId == projectId).Select(m => m.TemplateId).ToArray();
                templates = contract.CodeTemplates.Where(m => !existIds.Contains(m.Id)).ToArray();
            });
            BindableCollection<TemplateViewModel> models = new BindableCollection<TemplateViewModel>();
            foreach (CodeTemplate template in templates)
            {
                TemplateViewModel model = _provider.GetRequiredService<TemplateViewModel>();
                model = template.MapTo(model);
                models.Add(model);
            }

            return models;
        }

        public bool CanSave => ProjectTemplates.All(m => !m.HasErrors);
        public async void Save()
        {
            if (!CanSave)
            {
                Helper.Notify("项目模板验证失败", NotificationType.Warning);
                return;
            }

            CodeProjectTemplateInputDto[] dtos = ProjectTemplates.Select(m => m.MapTo<CodeProjectTemplateInputDto>()).ToArray();
            OperationResult result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                result = await contract.UpdateCodeProjectTemplates(dtos);
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

        public void BeginningEdit(DataGridBeginningEditEventArgs args)
        {
            if (args.Row.DataContext is ProjectTemplateViewModel model)
            {
                model.TemplateSource = GetTemplates(model.ProjectId);
            }
        }
    }
}
