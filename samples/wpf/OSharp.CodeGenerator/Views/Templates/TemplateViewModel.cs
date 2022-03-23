// -----------------------------------------------------------------------
//  <copyright file="TemplateViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-12 15:42</last-date>
// -----------------------------------------------------------------------

using System;
using System.Windows;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using Notifications.Wpf.Core;

using OSharp.CodeGeneration.Generates;
using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Data;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using Stylet;


namespace OSharp.CodeGenerator.Views.Templates
{
    [MapTo(typeof(CodeTemplateInputDto))]
    [MapFrom(typeof(CodeTemplate))]
    public class TemplateViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public TemplateViewModel(IModelValidator<TemplateViewModel> validator, IServiceProvider provider)
            : base(validator)
        {
            _provider = provider;
            Validate();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public MetadataType MetadataType { get; set; }

        public string TemplateFile { get; set; }

        public int Order { get; set; }

        public string OutputFileFormat { get; set; }

        public bool IsOnce { get; set; }

        public bool IsSystem { get; set; }

        public bool IsLocked { get; set; }
        
        public DateTime CreatedTime { get; set; }

        public void Up()
        {
            var list = IoC.Get<TemplateListViewModel>().Templates;
            if (list.SwapUp(this))
            {
                Helper.Output($"模板“{Name}”上移成功");
            }
        }

        public void Down()
        {
            var list = IoC.Get<TemplateListViewModel>().Templates;
            if (list.SwapDown(this))
            {
                Helper.Output($"模板“{Name}”下移成功");
            }
        }

        public async void Delete()
        {
            if (IsSystem)
            {
                Helper.Notify($"模板“{Name}”是系统模板，不能删除", NotificationType.Error);
                return;
            }

            if (MessageBox.Show($"是否删除模板“{Name}”？", "请确认", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }

            OperationResult result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                result = await contract.DeleteCodeTemplates(Id);
            });
            Helper.Notify(result);
            if (!result.Succeeded)
            {
                return;
            }

            TemplateListViewModel list = IoC.Get<TemplateListViewModel>();
            list.Init();
        }
    }


    public class TemplateViewModelValidator : AbstractValidator<TemplateViewModel>
    {
        public TemplateViewModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("模块名称不能为空");
            RuleFor(m => m.TemplateFile).NotEmpty().WithMessage("源代码文件不能为空");
            RuleFor(m => m.OutputFileFormat).NotEmpty().WithMessage("输出文件格式不能为空");
        }
    }
}
