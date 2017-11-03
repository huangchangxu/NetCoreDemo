﻿using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace PaymentCenter.Infrastructure.Log
{
    public class LogHelper
    {

        static ILoggerRepository repository;
        static LogHelper()
        {
            repository = LogManager.CreateRepository("NETCoreRepository");
            

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(string.Format("<configuration ID=\"log4net\"><log4net><!--Error日志--><logger name=\"LogError\"><level value=\"DEBUG\" /><appender-ref ref=\"RollingLogFileAppender\" /></logger><!--Info日志--><logger name=\"LogInfo\">  <level value=\"DEBUG\" />  <appender-ref ref=\"InfoAppender\" /></logger><!--监控日志--><logger name=\"LogMonitor\">  <level value=\"DEBUG\" />  <appender-ref ref=\"MonitorAppender\" /></logger><!--Info日志--><appender name=\"InfoAppender\" type=\"log4net.Appender.RollingFileAppender,log4net\" >  <param name=\"File\" value=\"{1}Logs/LogInfo/\" />  <!--添加到文件-->  <param name=\"AppendToFile\" value=\"true\" />  <!--根据日期滚动-->  <param name=\"RollingStyle\" value=\"Date\" />  <!--文件名称规则-->  <param name=\"DatePattern\" value=\"yyyy-MM-dd&quot;.txt&quot;\" />  <!--日志文件名是否是固定不变的-->  <param name=\"StaticLogFileName\" value=\"false\" />  <!--log保留天数-->  <param name= \"MaxSizeRollBackups\" value= \"{0}\"/><layout type=\"log4net.Layout.PatternLayout,log4net\"><param name=\"ConversionPattern\" value=\"%d [%t] %-5p %c - %m%n\" /><param name=\"Header\" value=\"&#13;&#10;----------------------header--------------------------&#13;&#10;\" /><param name=\"Footer\" value=\"&#13;&#10;----------------------footer--------------------------&#13;&#10;\" />  </layout></appender><!--错误日志--><appender name=\"RollingLogFileAppender\" type=\"log4net.Appender.RollingFileAppender,log4net\" >  <param name=\"File\" value=\"Logs/LogError/\" />  <param name=\"AppendToFile\" value=\"true\" />  <param name=\"RollingStyle\" value=\"Date\" />  <param name=\"DatePattern\" value=\"yyyy-MM-dd&quot;.txt&quot;\" />  <param name=\"StaticLogFileName\" value=\"false\" />  <layout type=\"log4net.Layout.PatternLayout,log4net\"><param name=\"ConversionPattern\" value=\"%d [%t] %-5p %c - %m%n\" /><param name=\"Header\" value=\"&#13;&#10;----------------------header--------------------------&#13;&#10;\" /><param name=\"Footer\" value=\"&#13;&#10;----------------------footer--------------------------&#13;&#10;\" />  </layout></appender><!--监控日志--><appender name=\"MonitorAppender\" type=\"log4net.Appender.RollingFileAppender,log4net\" >  <param name=\"File\" value=\"Logs/LogMonitor/\" />  <param name=\"AppendToFile\" value=\"true\" />  <param name=\"RollingStyle\" value=\"Date\" />  <param name=\"DatePattern\" value=\"yyyy-MM-dd&quot;.txt&quot;\" />  <param name=\"StaticLogFileName\" value=\"false\" />  <layout type=\"log4net.Layout.PatternLayout,log4net\"><param name=\"ConversionPattern\" value=\"%d [%t] %-5p %c - %m%n\" /><param name=\"Header\" value=\"&#13;&#10;----------------------header--------------------------&#13;&#10;\" /><param name=\"Footer\" value=\"&#13;&#10;----------------------footer--------------------------&#13;&#10;\" />  </layout></appender>  </log4net></configuration>", 10, ""));
            XmlElement element = (XmlElement)doc.GetElementsByTagName("log4net")[0];
            XmlConfigurator.Configure(repository, element);
        }
    }
}
