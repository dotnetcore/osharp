// -----------------------------------------------------------------------
//  <copyright file="ProjectList.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-04 0:32</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Data;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;

using Stylet;

using Screen = Stylet.Screen;


namespace OSharp.CodeGenerator.Views.Projects
{
    [Singleton]
    public class ProjectListViewModel : Screen
    {
        private readonly IServiceProvider _provider;

        public ProjectListViewModel(IServiceProvider provider)
        {
            _provider = provider;
        }

        public bool IsShow { get; set; }

        public IObservableCollection<ProjectViewModel> Projects { get; } = new BindableCollection<ProjectViewModel>();

        public string EditTitle { get; set; }

        public bool IsShowEdit { get; set; }

        public ProjectViewModel EditingModel { get; set; }

        public void Show()
        {
            Init();
            IsShow = true;
        }

        public void Init()
        {
            List<CodeProject> projects = new List<CodeProject>();
            _provider.ExecuteScopedWork(provider =>
            {
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                projects = contract.CodeProjects.ToList();
            });
            Projects.Clear();
            foreach (CodeProject project in projects)
            {
                ProjectViewModel model = _provider.GetRequiredService<ProjectViewModel>();
                model = project.MapTo(model);
                Projects.Add(model);
            }
        }

        public void New()
        {
            ProjectListViewModel model = IoC.Get<ProjectListViewModel>();
            model.EditingModel = IoC.Get<ProjectViewModel>();
            model.EditTitle = "新增项目";
            model.IsShowEdit = true;
        }

        public async void Import()
        {
            FileDialog dialog = new OpenFileDialog()
            {
                Title = "打开一个保存的JSON配置文件",
                CheckFileExists = true,
                Multiselect = false,
                Filter = "JSON文件|*.json"
            };
            
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            string file = dialog.FileName;
            await ImportProject(file);
        }

        private async Task ImportProject(string file)
        {
            string json = await File.ReadAllTextAsync(file);
            CodeProject proj = json.FromJsonString<CodeProject>();
            if (proj == null)
            {
                return;
            }

            await _provider.ExecuteScopedWorkAsync(async provider =>
            {
                IUnitOfWork unitOfWork = provider.GetUnitOfWork(true);
                IDataContract contract = provider.GetRequiredService<IDataContract>();
                CodeProject project = await GetOrAddProject(contract, proj);
                foreach (var mod in proj.Modules)
                {
                    CodeModule module = await GetOrAddModule(contract, project.Id, mod);
                    foreach (var en in mod.Entities)
                    {
                        CodeEntity entity = await GetOrAddEntity(contract, module.Id, en);
                        foreach (var prop in en.Properties)
                        {
                            CodeProperty property = await GetOrAddProperty(contract, entity.Id, prop);
                        }

                        foreach (var fore in en.Foreigns)
                        {
                            CodeForeign foreign = await GetOrAddForeign(contract, entity.Id, fore);
                        }
                    }
                }

                foreach (CodeProjectTemplate projTpl in proj.ProjectTemplates)
                {
                    if (projTpl.Template == null)
                    {
                        continue;
                    }

                    CodeTemplate tpl = contract.CodeTemplates.FirstOrDefault(m => m.Name == projTpl.Template.Name);
                    if (tpl == null)
                    {
                        CodeTemplateInputDto dto = projTpl.Template.MapTo<CodeTemplateInputDto>();
                        dto.Id = default;
                        var result = await contract.UpdateCodeTemplates(dto);
                        if (result.Succeeded)
                        {
                            tpl = result.Data[0];
                        }
                    }
                    projTpl.TemplateId = tpl.Id;
                    project.ProjectTemplates.Add(projTpl);
                }

                await unitOfWork.CommitAsync();
            });
            Init();
            Helper.Output($"文件“{file}”导入完毕");
        }

        private static async Task<CodeForeign> GetOrAddForeign(IDataContract contract, Guid entityId, CodeForeign fore)
        {
            CodeForeign foreign = contract.CodeForeigns.FirstOrDefault(m => m.EntityId == entityId && m.SelfNavigation == fore.SelfNavigation);
            if (foreign != null)
            {
                return foreign;
            }

            CodeForeignInputDto dto = new CodeForeignInputDto()
            {
                SelfNavigation = fore.SelfNavigation,
                SelfForeignKey = fore.SelfForeignKey,
                OtherEntity = fore.OtherEntity,
                OtherNavigation = fore.OtherNavigation,
                IsRequired = fore.IsRequired,
                DeleteBehavior = fore.DeleteBehavior,
                ForeignRelation = fore.ForeignRelation,
                EntityId = entityId
            };
            await contract.UpdateCodeForeigns(dto);
            foreign = contract.CodeForeigns.First(m => m.EntityId == entityId && m.SelfNavigation == fore.SelfNavigation);

            return foreign;
        }

        private static async Task<CodeProperty> GetOrAddProperty(IDataContract contract, Guid entityId, CodeProperty prop)
        {
            CodeProperty property = contract.CodeProperties.FirstOrDefault(m => m.EntityId == entityId && m.Name == prop.Name && m.Display == prop.Display);
            if (property != null)
            {
                return property;
            }

            CodePropertyInputDto dto = new CodePropertyInputDto()
            {
                Name = prop.Name,
                Display = prop.Display,
                TypeName = prop.TypeName,
                Updatable = prop.Updatable,
                Sortable = prop.Sortable,
                Filterable = prop.Filterable,
                IsRequired = prop.IsRequired,
                MinLength = prop.MinLength,
                MaxLength = prop.MaxLength,
                IsNullable = prop.IsNullable,
                IsEnum = prop.IsEnum,
                IsReadonly = prop.IsReadonly,
                IsVirtual = prop.IsVirtual,
                IsForeignKey = prop.IsForeignKey,
                IsNavigation = prop.IsNavigation,
                RelateEntity = prop.RelateEntity,
                DataAuthFlag = prop.DataAuthFlag,
                IsInputDto = prop.IsInputDto,
                IsOutputDto = prop.IsOutputDto,
                DefaultValue = prop.DefaultValue,
                IsLocked = prop.IsLocked,
                EntityId = entityId
            };
            await contract.UpdateCodeProperties(dto);
            property = contract.CodeProperties.First(m => m.EntityId == entityId && m.Name == prop.Name && m.Display == prop.Display);

            return property;
        }

        private static async Task<CodeEntity> GetOrAddEntity(IDataContract contract, Guid moduleId, CodeEntity en)
        {
            CodeEntity entity = contract.CodeEntities.FirstOrDefault(m => m.ModuleId == moduleId && m.Name == en.Name && m.Display == en.Display);
            if (entity != null)
            {
                return entity;
            }

            CodeEntityInputDto dto = new CodeEntityInputDto()
            {
                Name = en.Name,
                Display = en.Display,
                PrimaryKeyTypeFullName = en.PrimaryKeyTypeFullName,
                Icon = en.Icon,
                Listable = en.Listable,
                Addable = en.Addable,
                Updatable = en.Updatable,
                Deletable = en.Deletable,
                IsDataAuth = en.IsDataAuth,
                HasCreatedTime = en.HasCreatedTime,
                HasLocked = en.HasLocked,
                HasSoftDeleted = en.HasSoftDeleted,
                HasCreationAudited = en.HasCreationAudited,
                HasUpdateAudited = en.HasUpdateAudited,
                IsLocked = en.IsLocked,
                ModuleId = moduleId
            };
            await contract.UpdateCodeEntities(dto);
            entity = contract.CodeEntities.First(m => m.ModuleId == moduleId && m.Name == en.Name && m.Display == en.Display);

            return entity;
        }

        private static async Task<CodeModule> GetOrAddModule(IDataContract contract, Guid projectId, CodeModule mod)
        {
            CodeModule module = contract.CodeModules.FirstOrDefault(m => m.ProjectId == projectId && m.Name == mod.Name && m.Display == mod.Display);
            if (module != null)
            {
                return module;
            }

            CodeModuleInputDto dto = new CodeModuleInputDto()
            {
                Name = mod.Name,
                Display = mod.Display ?? mod.Name,
                Icon = mod.Icon,
                ProjectId = projectId
            };
            await contract.UpdateCodeModules(dto);
            module = contract.CodeModules.First(m => m.ProjectId == projectId && m.Name == mod.Name && m.Display == mod.Display);

            return module;
        }

        private static async Task<CodeProject> GetOrAddProject(IDataContract contract, CodeProject proj)
        {
            CodeProject project = contract.CodeProjects.FirstOrDefault(m => m.Name == proj.Name && m.NamespacePrefix == proj.NamespacePrefix);
            if (project != null)
            {
                return project;
            }

            CodeProjectInputDto projDto = new CodeProjectInputDto()
            {
                Name = proj.Name,
                NamespacePrefix = proj.NamespacePrefix,
                Company = proj.Company,
                SiteUrl = proj.SiteUrl,
                Creator = proj.Creator,
                Copyright = proj.Copyright
            };
            await contract.CreateCodeProjects(projDto);
            project = contract.CodeProjects.First(m => m.Name == proj.Name && m.NamespacePrefix == proj.NamespacePrefix);

            return project;
        }

    }

}
