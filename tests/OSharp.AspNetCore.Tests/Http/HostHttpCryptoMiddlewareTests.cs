using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Extensions;
using OSharp.Http;
using OSharp.Security;

using Shouldly;

using Xunit;


namespace OSharp.AspNetCore.Http.Tests
{
    public class HostHttpCryptoMiddlewareTests
    {
        private readonly IServiceProvider _provider;

        public HostHttpCryptoMiddlewareTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IHostHttpCrypto, HostHttpCrypto>();
            services.AddSingleton<IClientHttpCrypto, ClientHttpCrypto>();
            services.AddTransient<ClientHttpCryptoHandler>();

            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            services.AddOptions();
            services.Configure<OsharpOptions>(configuration.GetSection("OSharp"));
            _provider = services.BuildServiceProvider();
        }

        [Fact()]
        public void HostHttpCryptoMiddlewareTest()
        {
            var middleware = _provider.GetService<HostHttpCryptoMiddleware>();
            middleware.ShouldBeNull();
        }

        [Fact()]
        public async Task InvokeAsyncTest()
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.Body = new MemoryStream("test-request".ToBytes());
            RsaHelper rsa = new RsaHelper();
            context.Request.Headers.Add(HttpHeaderNames.ClientPublicKey, rsa.PublicKey);
            context.Response.Body = new MemoryStream();

            var middleware = ActivatorUtilities.CreateInstance<HostHttpCryptoMiddleware>(_provider,
                new RequestDelegate(c =>
                {
                    c.Response.Body = new MemoryStream("test-response".ToBytes());
                    return Task.CompletedTask;
                }));
            await middleware.InvokeAsync(context);
        }
    }
}