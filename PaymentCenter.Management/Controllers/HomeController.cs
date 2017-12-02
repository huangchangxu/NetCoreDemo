using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PaymentCenter.Management.Models;
using PaymentCenter.Infrastructure.Tools;
using Microsoft.AspNetCore.Hosting;
using Autofac;

namespace PaymentCenter.Management.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _host;
        public HomeController(IHostingEnvironment host)
        {
            _host = host;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            throw new Exception("dadfasf");
            return View();
        }
        /// <summary>
        /// 图形验证码
        /// </summary>
        /// <returns></returns>
        public FileResult ValidateCode()
        {
            var fontPath = _host.ContentRootPath + "/wwwroot/fonts/arial.ttf";
            var codeArray = ValidateCodeTool.GetValidCodeByte(fontPath,out string code);
            return File(codeArray, @"image/png");
        }
    }
}
