// -----------------------------------------------------------------------
//  <copyright file="ServiceProviderBootstrapper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-28 15:00</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Stylet;


namespace OSharp.Wpf.Stylet
{
    public class ServiceProviderBootstrapper<TRootViewModel> : BootstrapperBase where TRootViewModel : class
    {
        private object _rootViewModel;

        protected virtual object RootViewModel
        {
            get { return _rootViewModel ??= ServiceProvider.GetService(typeof(TRootViewModel)); }
        }

        protected IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Overridden from BootstrapperBase, this sets up the IoC container
        /// </summary>
        protected sealed override void ConfigureBootstrapper()
        {
            IServiceCollection services = new ServiceCollection();

            // Call DefaultConfigureIoC *after* ConfigureIoIC, so that they can customize builder.Assemblies
            this.DefaultConfigureIoC(services);
            this.ConfigureIoC(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        protected virtual void ConfigureIoC(IServiceCollection services)
        { }

        protected virtual void DefaultConfigureIoC(IServiceCollection services)
        {
            ViewManagerConfig viewManagerConfig = new ViewManagerConfig()
            {
                ViewFactory = this.GetInstance,
                ViewAssemblies = new List<Assembly>() { GetType().Assembly }
            };
            services.AddSingleton<IViewManager>(new ViewManager(viewManagerConfig));

            services.AddSingleton<IWindowManagerConfig>(this);
            services.AddSingleton<IWindowManager>(p => new WindowManager(p.GetService<IViewManager>(), p.GetService<IMessageBoxViewModel>, this));
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddTransient<IMessageBoxViewModel, MessageBoxViewModel>();
            services.AddTransient<MessageBoxView>();
        }

        /// <summary>
        /// Called when the application is launched. Should display the root view using <see cref="M:Stylet.BootstrapperBase.DisplayRootView(System.Object)" />
        /// </summary>
        protected override void Launch()
        {
            base.DisplayRootView(RootViewModel);
        }

        /// <summary>
        /// Given a type, use the IoC container to fetch an instance of it
        /// </summary>
        /// <param name="type">Type of instance to fetch</param>
        /// <returns>Fetched instance</returns>
        public override object GetInstance(Type type)
        {
            return ServiceProvider?.GetService(type);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            ScreenExtensions.TryDispose(_rootViewModel);
        }
    }
}