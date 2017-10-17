using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OSharp.Entity;
using OSharp.Infrastructure;


namespace OSharp.Demo.Web.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    public class ValuesController : Controller
    {
        private readonly IRepository<Function, Guid> _functionRepository;

        /// <summary>
        /// 初始化一个<see cref="ValuesController"/>类型的新实例
        /// </summary>
        public ValuesController(IRepository<Function,Guid>functionRepository)
        {
            _functionRepository = functionRepository;
        }

        public IEnumerable<string> Get()
        {
            var lines = _functionRepository.Query().Select(m => $"{m.Id.ToString("N")}: {m.Name}");
            return lines;
        }
    }
}