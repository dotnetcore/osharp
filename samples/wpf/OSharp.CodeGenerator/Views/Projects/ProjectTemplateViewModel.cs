// -----------------------------------------------------------------------
//  <copyright file="ProjectTemplateViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-12 20:02</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Data;
using OSharp.CodeGenerator.Views.Templates;
using OSharp.Data;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using Stylet;


namespace OSharp.CodeGenerator.Views.Projects
{
    [MapTo(typeof(CodeProjectTemplateInputDto))]
    public class ProjectTemplateViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public ProjectTemplateViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Guid Id { get; set; }

        public bool IsLocked { get; set; }

        public Guid ProjectId { get; set; }

        public Guid TemplateId { get; set; }

        public string TemplateName { get; set; }

        public TemplateViewModel Template { get; set; }

        public IObservableCollection<TemplateViewModel> TemplateSource { get; set; } = new BindableCollection<TemplateViewModel>();

        public void TemplateChange(SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count > 0 && args.AddedItems[0] is TemplateViewModel template)
            {
                TemplateId = template.Id;
                TemplateName = template.Name;
            }
        }

        public async void Delete()
        {
            if (MessageBox.Show($"是否删除模板“[{TemplateName}]”?", "请确认", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }

            OperationResult result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                result = await contract.DeleteCodeProjectTemplates(Id);
            });
            Helper.Notify(result);
            if (!result.Succeeded)
            {
                return;
            }

            ProjectTemplateListViewModel list = IoC.Get<ProjectTemplateListViewModel>();
            list.Init();
        }
    }
}
