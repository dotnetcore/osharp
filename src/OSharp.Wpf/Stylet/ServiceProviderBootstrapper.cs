// -----------------------------------------------------------------------
//  <copyright file="ServiceProviderBootstrapper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-28 15:00</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Wpf.Stylet;

public abstract class ServiceProviderBootstrapper<TRootViewModel> : BootstrapperBase where TRootViewModel : class
{
    private TRootViewModel _rootViewModel;
    protected virtual TRootViewModel RootViewModel => this._rootViewModel ??= (TRootViewModel)this.GetInstance(typeof(TRootViewModel));

    private ServiceProvider _serviceProvider;

    protected IServiceProvider ServiceProvider => _serviceProvider;

    /// <summary>
    /// Overridden from BootstrapperBase, this sets up the IoC container
    /// </summary>
    protected override void ConfigureBootstrapper()
    {
        IServiceCollection services = new ServiceCollection();

        // Call DefaultConfigureIoC *after* ConfigureIoIC, so that they can customize builder.Assemblies
        this.DefaultConfigureIoC(services);
        this.ConfigureIoC(services);

        _serviceProvider = services.BuildServiceProvider();
    }

    protected virtual void ConfigureIoC(IServiceCollection services)
    { }

    protected virtual void DefaultConfigureIoC(IServiceCollection services)
    {
        var viewManagerConfig = new ViewManagerConfig()
        {
            ViewFactory = this.GetInstance,
            ViewAssemblies = new List<Assembly>(){ this.GetType().Assembly}
        };

        services.AddSingleton<IViewManager>(new ViewManager(viewManagerConfig));
        services.AddTransient<MessageBoxView>();

        services.AddSingleton<IWindowManagerConfig>(this);
        services.AddSingleton<IWindowManager, WindowManager>();
        services.AddSingleton<IEventAggregator, EventAggregator>();
        services.AddTransient<IMessageBoxViewModel, MessageBoxViewModel>(); // Not singleton!
        // Also need a factory
        services.AddSingleton<Func<IMessageBoxViewModel>>(() => new MessageBoxViewModel());
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
        _serviceProvider?.Dispose();
    }
}
