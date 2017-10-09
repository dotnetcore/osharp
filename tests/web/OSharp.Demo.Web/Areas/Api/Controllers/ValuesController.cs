using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OSharp.Demo.Web.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    public class ValuesController : Controller
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "OSharpNS", "Angular4", "AspNetCore" };
        }
    }
}