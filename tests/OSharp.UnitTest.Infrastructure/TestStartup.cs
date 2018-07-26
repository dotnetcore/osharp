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

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseOSharp();
        }
    }
}
