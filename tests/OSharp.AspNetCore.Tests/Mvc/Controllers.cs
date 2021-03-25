using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc;


namespace OSharp.AspNetCore.Tests.Mvc
{
    public class PublicController : BaseController
    {
        public IActionResult Read()
        {
            throw new NotImplementedException();
        }
    }
    

    internal class Internal1Controller : BaseController
    {
        public IActionResult Read1()
        {
            throw new NotImplementedException();
        }
    }


    internal class Internal2Controller : AbstractController
    {
        public IActionResult Read2()
        {
            throw new NotImplementedException();
        }
    }


    internal abstract class AbstractController : BaseController
    {
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
    
    [Controller]
    internal class AttributeFunction
    {
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
    
    internal class NamingController
    {
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }

    internal class NamingController2
    {
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }

    [NonController]
    internal class AttributeNonController
    {
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}
