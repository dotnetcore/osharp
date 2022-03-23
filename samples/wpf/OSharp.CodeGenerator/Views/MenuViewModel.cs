// -----------------------------------------------------------------------
//  <copyright file="NavViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-10 11:52</last-date>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;

using Microsoft.Extensions.DependencyInjection;

using Notifications.Wpf.Core;

using OSharp.CodeGeneration.Generates;
using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Data;
using OSharp.CodeGenerator.Views.Entities;
using OSharp.CodeGenerator.Views.Modules;
using OSharp.CodeGenerator.Views.Projects;
using OSharp.CodeGenerator.Views.Properties;
using OSharp.IO;
using OSharp.Wpf.Stylet;

using Stylet;

using Screen = Stylet.Screen;


namespace OSharp.CodeGenerator.Views
{
    [Singleton]
    public class MenuViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public MenuViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ProjectViewModel Project { get; set; }

        public IObservableCollection<MenuItem> MenuItems { get; set; } = new BindableCollection<MenuItem>();

        public Screen SelectModel { get; set; }

        public void Init()
        {
            if (Project == null)
            {
                Helper.Notify("当前项目为空，请先通过菜单“项目-项目管理”加载项目", NotificationType.Error);
                return;
            }

            CodeProject[] projects = null;
            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                projects = contract.GetCodeProject(m => m.Name == Project.Name);
            });
            if (projects == null)
            {
                Helper.Notify($"名称为“{Project.Name}”的项目信息不存在", NotificationType.Error);
                return;
            }

            MenuItems.Clear();
            foreach (CodeProject project in projects)
            {
                MenuItems.Add(ToMenu(project));
            }
        }

        public void Select(RoutedPropertyChangedEventArgs<object> args)
        {
            if (args.NewValue is not MenuItem item)
            {
                return;
            }

            SelectModel = item.Screen;
            MainViewModel main = IoC.Get<MainViewModel>();
            main.ModuleList.IsShow = SelectModel is ProjectViewModel;
            main.EntityList.IsShow = SelectModel is ModuleViewModel;
            main.PropertyList.IsShow = SelectModel is EntityViewModel;
            switch (SelectModel)
            {
                case ProjectViewModel project:
                    ModuleListViewModel list1 = IoC.Get<ModuleListViewModel>();
                    list1.Project = project;
                    list1.Init();
                    break;
                case ModuleViewModel module:
                    EntityListViewModel list2 = IoC.Get<EntityListViewModel>();
                    list2.Module = module;
                    list2.Init();
                    break;
                case EntityViewModel entity:
                    PropertyListViewModel list3 = IoC.Get<PropertyListViewModel>();
                    list3.Entity = entity;
                    list3.Init();
                    break;
            }
        }

        public async void Generate()
        {
            if (Project == null)
            {
                Helper.Notify("当前项目为空，无法生成代码，请先通过菜单“项目-项目管理”加载项目", NotificationType.Error);
                return;
            }
            CodeProject project = null;
            CodeTemplate[] templates = Array.Empty<CodeTemplate>();
            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                project = contract.GetCodeProject(m => m.Name == Project.Name).FirstOrDefault();
                templates = contract.CodeProjectTemplates.Where(m => m.ProjectId == project.Id && !m.IsLocked && !m.Template.IsLocked).Select(m => m.Template).ToArray();
            });
            if (project == null)
            {
                Helper.Notify($"名称为“{Project.Name}”的项目信息不存在", NotificationType.Error);
                return;
            }

            if (templates.Length == 0)
            {
                Helper.Notify($"项目“{project.GetName()}”的模板数量为0，请先通过菜单“项目-项目模板管理”添加模板", NotificationType.Error);
                return;
            }

            CodeFile[] codeFiles = Array.Empty<CodeFile>();
            var progress = await Helper.Main.ShowProgressAsync("请稍候", "正在生成代码，请稍候");
            await Task.Run(async () =>
            {
                try
                {
                    ICodeGenerator generator = _provider.GetRequiredService<ICodeGenerator>();
                    codeFiles = await generator.GenerateCodes(templates, project);
                    await progress.CloseAsync();
                }
                catch (Exception ex)
                {
                    await progress.CloseAsync();
                    Helper.Notify($"代码生成失败：{ex.Message}", NotificationType.Error);
                }
            });
            SaveToFiles(codeFiles);
        }

        private void SaveToFiles(CodeFile[] codeFiles)
        {
            if (codeFiles.Length == 0)
            {
                Helper.Notify("生成的代码文件数量为0", NotificationType.Information);
                return;
            }
            string rootPath;
            if (string.IsNullOrEmpty(Project.RootPath) || !Path.IsPathFullyQualified(Project.RootPath))
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog()
                {
                    UseDescriptionForTitle = true,
                    Description = $"项目“{Project.Name}({Project.NamespacePrefix})”生成了{codeFiles.Length}个代码文件，请选择保存文件夹：",
                    RootFolder = Environment.SpecialFolder.MyDocuments,
                    ShowNewFolderButton = true
                };

                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    rootPath = dialog.SelectedPath;
                }
                else
                {
                    return;
                }
            }
            else
            {
                rootPath = Project.RootPath;
                DirectoryHelper.CreateIfNotExists(rootPath);
            }

            foreach (CodeFile codeFile in codeFiles)
            {
                if (codeFile.SaveToFile(rootPath))
                {
                    Helper.Output($"代码文件：{codeFile.FileName} 保存成功");
                }
            }
            Helper.Output($"项目“{Project.Name}”代码生成成功，输出{codeFiles.Length}个文件");
            Helper.Notify($"{codeFiles.Length}个代码文件已输出到：{rootPath}", NotificationType.Success, null, () => Helper.OpenFolder(rootPath));
        }

        private MenuItem ToMenu(CodeProject project)
        {
            MenuItem projectMenu = _provider.GetRequiredService<MenuItem>();
            projectMenu.Id = project.Id;
            projectMenu.Text = $"{project.Name}[{project.NamespacePrefix}]";
            projectMenu.Type = MenuItemType.Project;
            projectMenu.Icon = PackIconMaterialKind.AlphaPBoxOutline;
            ProjectViewModel projectModel = project.ToViewModel();
            projectMenu.Screen = projectModel;
            foreach (CodeModule module in project.Modules)
            {
                MenuItem moduleMenu = _provider.GetRequiredService<MenuItem>();
                moduleMenu.Id = module.Id;
                moduleMenu.Text = $"{module.Display}[{module.Name}]";
                moduleMenu.Type = MenuItemType.Module;
                moduleMenu.Icon = PackIconMaterialKind.AlphaMBoxOutline;
                ModuleViewModel moduleModel = module.ToViewModel(projectModel);
                moduleMenu.Screen = moduleModel;
                foreach (CodeEntity entity in module.Entities)
                {
                    MenuItem entityMenu = _provider.GetRequiredService<MenuItem>();
                    entityMenu.Id = entity.Id;
                    entityMenu.Text = $"{entity.Display}[{entity.Name}]";
                    entityMenu.Type = MenuItemType.Entity;
                    entityMenu.Icon = PackIconMaterialKind.AlphaEBoxOutline;
                    EntityViewModel entityModel = entity.ToViewModel(moduleModel);
                    entityMenu.Screen = entityModel;
                    moduleMenu.ItemMenus.Add(entityMenu);
                }
                projectMenu.ItemMenus.Add(moduleMenu);
            }

            return projectMenu;
        }
    }


    public class MenuItem : Screen
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 显示
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 获取或设置 图标
        /// </summary>
        public PackIconMaterialKind Icon { get; set; }

        /// <summary>
        /// 获取或设置 节点类型
        /// </summary>
        public MenuItemType Type { get; set; }

        /// <summary>
        /// 获取或设置 当前关联视图模型
        /// </summary>
        public Screen Screen { get; set; }

        /// <summary>
        /// 获取或设置 子节点集合
        /// </summary>
        public IObservableCollection<MenuItem> ItemMenus { get; set; } = new BindableCollection<MenuItem>();
    }


    public enum MenuItemType
    {
        Project,

        Module,

        Entity,

        Property
    }
}
