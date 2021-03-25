using Xunit;
using OSharp.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Tests.Mvc;


namespace OSharp.AspNetCore.Mvc.Tests
{
    public class MvcExtensionsTests
    {
        [Fact()]
        public void IsControllerTest()
        {
            Type type = typeof(PublicController);
            Assert.True(type.IsController());
            type = typeof(Internal1Controller);
            Assert.True(type.IsController());
            type = typeof(Internal2Controller);
            Assert.True(type.IsController());
            type = typeof(AbstractController);
            Assert.True(type.IsController(true));
            type = typeof(AttributeFunction);
            Assert.True(type.IsController());
            type = typeof(NamingController);
            Assert.True(type.IsController());
            type = typeof(NamingController2);
            Assert.False(type.IsController());
            type = typeof(AttributeNonController);
            Assert.False(type.IsController());
        }
        
    }
}