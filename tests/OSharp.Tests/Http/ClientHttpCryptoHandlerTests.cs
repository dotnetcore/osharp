using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;

using Xunit;


namespace OSharp.Http.Tests
{
    public class ClientHttpCryptoHandlerTests
    {
        private readonly IServiceProvider _provider;

        public ClientHttpCryptoHandlerTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IClientHttpCrypto, ClientHttpCrypto>();
            services.AddTransient<ClientHttpCryptoHandler>();

            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            services.AddOptions();
            services.Configure<OsharpOptions>(configuration.GetSection("OSharp"));
            _provider = services.BuildServiceProvider();
        }

        [Fact()]
        public void ClientHttpCryptoHandlerTest()
        {
            ClientHttpCryptoHandler handler = _provider.GetService<ClientHttpCryptoHandler>();
            Assert.NotNull(handler);
        }

        [Fact]
        public async Task SendAsyncTest()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://foo.com");
            
            ClientHttpCryptoHandler handler = _provider.GetService<ClientHttpCryptoHandler>();
            handler.InnerHandler = new TestHandler();
            HttpMessageInvoker invoker = new HttpMessageInvoker(handler);
            HttpResponseMessage result = await invoker.SendAsync(request, CancellationToken.None);
            Assert.Equal("test-response", await result.Content.ReadAsStringAsync());
        }
    }


    public class TestHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("test-response")
            };
            return Task.Factory.StartNew(() => response, cancellationToken);
        }
    }
}