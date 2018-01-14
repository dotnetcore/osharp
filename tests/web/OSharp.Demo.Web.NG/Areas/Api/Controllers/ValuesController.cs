using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OSharp.Demo.Web.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/values")]
    public class ValuesController : Controller
    {
        public IEnumerable<string> Get()
        {
            string[] lines = typeof(ValuesController).Namespace.Split('.');
            return lines;
        }
    }
}