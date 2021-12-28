using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;


namespace OSharp.CodeGeneration.Services
{
    public partial class DataService : IDataContract
    {
        private readonly IServiceProvider _serviceProvider;

        public DataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected IUnitOfWork UnitOfWork => _serviceProvider.GetUnitOfWork();

        protected IRepository<CodeProject, Guid> ProjectRepository => _serviceProvider.GetService<IRepository<CodeProject, Guid>>();

        protected IRepository<CodeModule, Guid> ModuleRepository => _serviceProvider.GetService<IRepository<CodeModule, Guid>>();

        protected IRepository<CodeEntity, Guid> EntityRepository => _serviceProvider.GetService<IRepository<CodeEntity, Guid>>();

        protected IRepository<CodeProperty, Guid> PropertyRepository => _serviceProvider.GetService<IRepository<CodeProperty, Guid>>();

        protected IRepository<CodeTemplate, Guid> TemplateRepository => _serviceProvider.GetService<IRepository<CodeTemplate, Guid>>();

        protected IRepository<CodeForeign, Guid> ForeignRepository => _serviceProvider.GetService<IRepository<CodeForeign, Guid>>();

        protected IRepository<CodeProjectTemplate, Guid> ProjectTemplateRepository =>
            _serviceProvider.GetService<IRepository<CodeProjectTemplate, Guid>>();
    }
}
