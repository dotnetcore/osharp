// -----------------------------------------------------------------------
//  <copyright file="MainViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-03 14:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;

using OSharp.CodeGenerator.Views.Entities;
using OSharp.CodeGenerator.Views.Helps;
using OSharp.CodeGenerator.Views.Modules;
using OSharp.CodeGenerator.Views.Projects;
using OSharp.CodeGenerator.Views.Properties;
using OSharp.CodeGenerator.Views.Templates;
using OSharp.Wpf.Stylet;

using Stylet;


namespace OSharp.CodeGenerator.Views
{
    [Singleton]
    public class MainViewModel : Screen
    {
        private readonly INotificationManager _notificationManager;

        /// <summary>
        /// 初始化一个<see cref="MainViewModel"/>类型的新实例
        /// </summary>
        public MainViewModel()
        {
            DisplayName = "OSharp代码生成器";
            _notificationManager = new NotificationManager(NotificationPosition.BottomRight);
        }

        public MainMenuViewModel MainMenu { get; set; } = IoC.Get<MainMenuViewModel>();

        public StatusBarViewModel StatusBar { get; set; } = IoC.Get<StatusBarViewModel>();

        public ProjectListViewModel ProjectList { get; set; } = IoC.Get<ProjectListViewModel>();

        public MenuViewModel Menu { get; set; } = IoC.Get<MenuViewModel>();

        public ModuleListViewModel ModuleList { get; set; } = IoC.Get<ModuleListViewModel>();

        public EntityListViewModel EntityList { get; set; } = IoC.Get<EntityListViewModel>();

        public ForeignListViewModel ForeignList { get; set; } = IoC.Get<ForeignListViewModel>();

        public PropertyListViewModel PropertyList { get; set; } = IoC.Get<PropertyListViewModel>();

        public TemplateListViewModel TemplateList { get; set; } = IoC.Get<TemplateListViewModel>();

        public ProjectTemplateListViewModel ProjectTemplateList { get; set; } = IoC.Get<ProjectTemplateListViewModel>();

        public AboutViewModel About { get; set; } = IoC.Get<AboutViewModel>();
        
        public async Task Notify(string message, NotificationType type = NotificationType.Information, string title = null, Action onClick = null)
        {
            title = title ?? "消息提示";
            NotificationContent content = new NotificationContent()
            {
                Title = title,
                Message = message,
                Type = type
            };
            await _notificationManager.ShowAsync(content, "MainNotifyArea", null, onClick);
        }

    }
}
