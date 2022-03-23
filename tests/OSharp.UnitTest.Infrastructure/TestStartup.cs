using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace OSharp.UnitTest.Infrastructure
{
    public class TestStartup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddLogging();
            services.AddOSharp();
        }

#if NET6_0_OR_GREATER
        public virtual void Configure(WebApplication app)
#else
        public virtual void Configure(IApplicationBuilder app)
#endif
        {
            app.UseOSharp();
        }
    }
}
