using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Views.Entities;
using OSharp.CodeGenerator.Views.Modules;
using OSharp.CodeGenerator.Views.Projects;
using OSharp.CodeGenerator.Views.Properties;
using OSharp.Mapping;
using OSharp.Wpf.Stylet;


namespace OSharp.CodeGenerator.Data
{
    public static class Extensions
    {
        public static ProjectViewModel ToViewModel(this CodeProject project)
        {
            ProjectViewModel model = IoC.Get<ProjectViewModel>();
            model = project.MapTo(model);
            return model;
        }

        public static ModuleViewModel ToViewModel(this CodeModule module, ProjectViewModel projectModel = null)
        {
            ModuleViewModel model = IoC.Get<ModuleViewModel>();
            model = module.MapTo(model);
            model.Project = projectModel;
            return model;
        }

        public static EntityViewModel ToViewModel(this CodeEntity entity, ModuleViewModel moduleModel = null)
        {
            EntityViewModel model = IoC.Get<EntityViewModel>();
            model = entity.MapTo(model);
            model.Module = moduleModel;
            return model;
        }

        public static PropertyViewModel ToViewModel(this CodeProperty property, EntityViewModel entityModel = null)
        {
            PropertyViewModel model = IoC.Get<PropertyViewModel>();
            model = property.MapTo(model);
            model.Entity = entityModel;
            return model;
        }

        public static IDataContract GetDataContract(this IServiceProvider provider)
        {
            return provider.GetRequiredService<IDataContract>();
        }
    }
}
