using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Linq;

namespace PaymentCenter.Infrastructure.Tools
{
    /// <summary>
    /// 邮件工具类
    /// </summary>
    public class EmailTool
    {
        readonly EmailSmtpConfig _config;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        public EmailTool(EmailSmtpConfig config)
        {
            if (config == null || config == default(EmailSmtpConfig))
                throw new ArgumentNullException("邮件发送SMTP初始化参数异常");
            _config = config;
        }
        /// <summary>
        /// 使用MailKit发送邮件
        /// </summary>
        /// <param name="body">邮件内容</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="to">收件人集合</param>
        /// <param name="cc">抄送人集合</param>
        public void SendEmailByMailKit(string body, string subject, string[] to, string[] cc = null)
        {
            var message = new MimeKit.MimeMessage();
            //发件人
            message.From.Add(new MimeKit.MailboxAddress(_config.From, _config.From));
            //邮件主题
            message.Subject = (subject);
            //设置邮件内容
            if (_config.BodyHtml)
            {
                var bodybuilder = new MimeKit.BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodybuilder.ToMessageBody();
            }
            else
                message.Body = new MimeKit.TextPart("plain") { Text = body };
            //添加收件人
            foreach (var item in to)
            {
                message.To.Add(new MimeKit.MailboxAddress(item, item));
            }
            //添加抄送人
            if (cc != null)
            {
                foreach (var item in cc)
                {
                    message.Cc.Add(new MimeKit.MailboxAddress(item, item));
                }
            }
            //初始化SMTP服务器并发送邮件
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_config.SmtpHost, _config.SmtpPort, _config.EnableSsl);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_config.SmtpAccount, _config.SmtpPwd);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        /// <summary>
        /// 使用Net.mail发送邮件
        /// </summary>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        public void SendEmailByNetMail(string body, string subject, string[] to, string[] cc = null)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(_config.From,_config.From)
            };
            foreach (var item in to)
            {
                mail.To.Add(item);
            }
            
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = _config.BodyHtml;
            
            
            using (var client = new System.Net.Mail.SmtpClient())
            {
                client.Port = _config.SmtpPort;
                client.Host = _config.SmtpHost;
                client.EnableSsl = _config.EnableSsl;
                client.Credentials = new System.Net.NetworkCredential(_config.SmtpAccount, _config.SmtpPwd);
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.Send(mail);
            }
        }
        /// <summary>
        /// 使用FluentSmtpEmail发送邮件
        /// </summary>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        public void SendEmailByFluentSmtpEmail(string body, string subject, string[] to, string[] cc = null)
        {

            FluentEmail.Core.Email.DefaultSender = new FluentEmail.Smtp.SmtpSender(new System.Net.Mail.SmtpClient
            {
                Port = _config.SmtpPort,
                Host = _config.SmtpHost,
                EnableSsl = _config.EnableSsl,
                Credentials = new System.Net.NetworkCredential(_config.SmtpAccount, _config.SmtpPwd)
            });
            var email = FluentEmail.Core.Email
                .From(_config.From)
                .To(to.Select(t => new FluentEmail.Core.Models.Address { EmailAddress = t }).ToList())
                .Subject(subject)
                .Body(body, _config.BodyHtml);
            if (cc != null && cc.Length > 0)
                email.CC(cc.Select(c => new FluentEmail.Core.Models.Address { EmailAddress = c }).ToList());
            email.Send();
        }
        /// <summary>
        /// 使用FluentSmtpEmail发送邮件（消息模板）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <param name="data"></param>
        /// <param name="subject"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        public string SendTemplateEmailByFluent<T>(string template, T data,string subject, string[] to, string[] cc = null)
        {
            FluentEmail.Core.Email.DefaultRenderer = new FluentEmail.Razor.RazorRenderer();
            FluentEmail.Core.Email.DefaultSender = new FluentEmail.Smtp.SmtpSender(new System.Net.Mail.SmtpClient
            {
                Port = _config.SmtpPort,
                Host = _config.SmtpHost,
                EnableSsl = _config.EnableSsl,
                Credentials = new System.Net.NetworkCredential(_config.SmtpAccount, _config.SmtpPwd)
            });

            var email = FluentEmail.Core.Email
               .From(_config.From)
               .To(to.Select(t => new FluentEmail.Core.Models.Address { EmailAddress = t }).ToList())
               .Subject(subject)
               .UsingTemplate(template, data);

            if (cc != null && cc.Length > 0)
                email.CC(cc.Select(c => new FluentEmail.Core.Models.Address { EmailAddress = c }).ToList());
            email.Send();

            return email.Data.Body;
        }


    }
    /// <summary>
    /// 发送邮件SMTP服务器配置信息
    /// </summary>
    public class EmailSmtpConfig
    {
        /// <summary>
        /// 发件邮箱地址
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 服务器登录账号
        /// </summary>
        public string SmtpAccount { get; set; }
        /// <summary>
        /// 服务器登录密码
        /// </summary>
        public string SmtpPwd { get; set; }
        /// <summary>
        /// Smtp服务器地址
        /// </summary>
        public string SmtpHost { get; set; }
        /// <summary>
        /// Smtp服务器端口
        /// </summary>
        public int SmtpPort { get; set; }
        /// <summary>
        /// 是否开启SSL通道
        /// </summary>
        public bool EnableSsl { get; set; } = false;
        /// <summary>
        /// 是否发送HTML内容
        /// </summary>
        public bool BodyHtml { get; set; } = false;
    }
}
