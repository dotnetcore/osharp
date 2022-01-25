// -----------------------------------------------------------------------
//  <copyright file="PropertyViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-07 1:27</last-date>
// -----------------------------------------------------------------------

using System;
using System.Windows;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Data;
using OSharp.CodeGenerator.Views.Entities;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using Stylet;


namespace OSharp.CodeGenerator.Views.Properties
{
    [MapTo(typeof(CodePropertyInputDto))]
    [MapFrom(typeof(CodeProperty))]
    public class PropertyViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// 初始化一个<see cref="PropertyViewModel"/>类型的新实例
        /// </summary>
        public PropertyViewModel(IModelValidator<PropertyViewModel> validator, IServiceProvider provider) : base(validator)
        {
            _provider = provider;
            Validate();
        }

        public EntityViewModel Entity { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Display { get; set; }

        public string TypeName { get; set; }

        public bool Listable { get; set; } = true;

        public bool Updatable { get; set; } = true;

        public bool Sortable { get; set; } = true;

        public bool Filterable { get; set; } = true;

        public bool IsRequired { get; set; }

        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }

        public bool IsNullable { get; set; }

        public bool IsReadonly { get; set; }

        public bool IsEnum { get; set; }

        public bool IsHide { get; set; }

        public bool IsVirtual { get; set; }

        public bool IsForeignKey { get; set; }

        public bool IsNavigation { get; set; }

        public string RelateEntity { get; set; }

        public bool IsInputDto { get; set; } = true;

        public bool IsOutputDto { get; set; } = true;

        public string DataAuthFlag { get; set; }

        public string DefaultValue { get; set; }

        public int Order { get; set; }

        public bool IsLocked { get; set; }

        public DateTime CreatedTime { get; set; }
        
        public void Up()
        {
            var list = IoC.Get<PropertyListViewModel>().Properties;
            if (list.SwapUp(this))
            {
                Helper.Output($"实体属性“{GetName()}”上移成功");
            }
        }

        public void Down()
        {
            var list = IoC.Get<PropertyListViewModel>().Properties;
            if (list.SwapDown(this))
            {
                Helper.Output($"实体属性“{GetName()}”下移成功");
            }
        }

        public async void Delete()
        {
            if (MessageBox.Show($"是否删除实体属性“{GetName()}”?", "是否确认", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }

            OperationResult result = null;
            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IDataContract contract = provider.GetDataContract();
                result = await contract.DeleteCodeProperties(Id);
            });
            Helper.Notify(result);
            if (!result.Succeeded)
            {
                return;
            }

            PropertyListViewModel list = IoC.Get<PropertyListViewModel>();
            list.Init();
        }

        private string GetName()
        {
            return $"{Display}[{Name}]";
        }
    }


    public class PropertyViewModelValidator : AbstractValidator<PropertyViewModel>
    {
        public PropertyViewModelValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("属性名称不能为空")
                .Matches("^[a-zA-Z_\u2E80-\u9FFF][0-9a-zA-Z_\u2E80-\u9FFF]*$").WithMessage("属性名称不符合标识符命名规则，只能是字母、数值、下划线、汉字，并且不能以数值开关");
            RuleFor(m => m.TypeName).NotEmpty().WithMessage("属性类型名不能为空");
        }
    }
}
