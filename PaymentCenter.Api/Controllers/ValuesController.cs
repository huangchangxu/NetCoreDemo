using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentCenter.Infrastructure.Extension;

namespace PaymentCenter.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Content(new Infrastructure.Responses.ApiCommonResponseDto<object>(123).ToJson());
        }
        [Route("/getapi")]
        [HttpGet]
        public Infrastructure.Responses.ApiCommonResponseDto<object> GetApi()
        {
            return new Infrastructure.Responses.ApiCommonResponseDto<object>(123);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
