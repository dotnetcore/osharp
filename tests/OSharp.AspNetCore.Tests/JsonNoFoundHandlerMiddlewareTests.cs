using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Xunit;


namespace OSharp.AspNetCore.Tests
{
    public class JsonNoFoundHandlerMiddlewareTests
    {
        [Fact()]
        public async Task InvokeAsyncTest1()
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.Path = new PathString("/foo");

            var middleware = ActivatorUtilities.CreateInstance<JsonNoFoundHandlerMiddleware>(null, new RequestDelegate(c =>
            {
                c.Response.StatusCode = 404;
                return Task.CompletedTask;
            }));

            await middleware.InvokeAsync(context);

            Assert.Equal("/index.html", context.Request.Path.Value);
        }

        [Fact()]
        public async Task InvokeAsyncTest2()
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.Path = new PathString("/foo.html");

            var middleware = ActivatorUtilities.CreateInstance<JsonNoFoundHandlerMiddleware>(null, new RequestDelegate(c =>
            {
                c.Response.StatusCode = 404;
                return Task.CompletedTask;
            }));

            await middleware.InvokeAsync(context);

            Assert.Equal("/foo.html", context.Request.Path.Value);
            Assert.Equal(404, context.Response.StatusCode);
        }

        [Fact()]
        public async Task InvokeAsyncTest3()
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.Path = new PathString("/api/foo");

            var middleware = ActivatorUtilities.CreateInstance<JsonNoFoundHandlerMiddleware>(null, new RequestDelegate(c =>
            {
                c.Response.StatusCode = 404;
                return Task.CompletedTask;
            }));

            await middleware.InvokeAsync(context);

            Assert.Equal("/api/foo", context.Request.Path.Value);
            Assert.Equal(404, context.Response.StatusCode);
        }
    }
}