using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.ConsoleApp
{
    public class EmailTest
    {
        public static void TempEmailTest()
        {
            //Infrastructure.Tools.EmailTool email = new Infrastructure.Tools.EmailTool(new Infrastructure.Tools.EmailSmtpConfig
            //{
            //    From = "huangchangxu@126.com",
            //    BodyHtml = true,
            //    EnableSsl = false,
            //    SmtpAccount = "huangchangxu@126.com",
            //    SmtpPwd = "CHANGxu910527",
            //    SmtpHost = "smtp.126.com",
            //    SmtpPort=25
            //});

            //var template = "sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
            //var result = email.SendTemplateEmailByFluent(template, new { Name = DateTime.Now.ToString(), Numbers = new string[] { "1", "2", "3" } }, "店商测试邮件", new string[] { "changxu.huang@ds365.com" });
            FluentEmail.Core.Email.DefaultRenderer = new FluentEmail.Razor.RazorRenderer();
            string template = "sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
            var email = FluentEmail.Core.Email
                .From("huangchangxu@126.com")
                .To("changxu.huang@ds365.com")
                .Subject("店商测试邮件")
                .UsingTemplate(template, new { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } });
            Console.WriteLine(email.Data.Body);

        }
    }
}
