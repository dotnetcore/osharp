using Xunit;

using Microsoft.AspNetCore.Http;

using System.IO;
using System.Threading.Tasks;

using OSharp.Extensions;


namespace OSharp.AspNetCore.Http.Tests
{
    public class HttpExtensionsTests
    {
        [Fact()]
        public async Task ReadAsStringAsyncTest()
        {
            HttpContext context = new DefaultHttpContext();
            string value = await context.Request.ReadAsStringAsync();
            Assert.True(string.IsNullOrEmpty(value));

            context.Request.Body = new MemoryStream("test".ToBytes());
            value = await context.Request.ReadAsStringAsync();
            Assert.Equal("test", value);
        }

        [Fact()]
        public async Task ReadAsStringAsyncTest1()
        {
            HttpContext context = new DefaultHttpContext();
            string value = await context.Response.ReadAsStringAsync();
            Assert.True(string.IsNullOrEmpty(value));

            context.Response.Body = new MemoryStream("test".ToBytes());
            value = await context.Response.ReadAsStringAsync();
            Assert.Equal("test", value);
        }

        [Fact()]
        public async Task WriteBodyAsyncTest()
        {
            HttpContext context = new DefaultHttpContext();
            await context.Request.WriteBodyAsync("test");
            string value = await context.Request.ReadAsStringAsync();
            Assert.Equal("test", value);
        }

        [Fact()]
        public async Task WriteBodyAsyncTest1()
        {
            HttpContext context = new DefaultHttpContext();
            await context.Response.WriteBodyAsync("test");
            string value = await context.Response.ReadAsStringAsync();
            Assert.Equal("test", value);
        }

        [Fact()]
        public void IsSuccessStatusCodeTest()
        {
            HttpContext context = new DefaultHttpContext();
            Assert.True(context.Response.IsSuccessStatusCode());
            context.Response.StatusCode = 401;
            Assert.False(context.Response.IsSuccessStatusCode());
            context.Response.StatusCode = 100;
            Assert.False(context.Response.IsSuccessStatusCode());
            context.Response.StatusCode = 500;
            Assert.False(context.Response.IsSuccessStatusCode());
        }
    }
}