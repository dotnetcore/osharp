using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;


namespace OSharp.UnitTest.Infrastructure
{
    public class TestStartup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddLogging();
            services.AddOSharp<AspOsharpPackManager>();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseOSharp();
        }
    }
}
