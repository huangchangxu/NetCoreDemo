using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Http;
using PaymentCenter.Infrastructure.Extension;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace PaymentCenter.Infrastructure.Log
{
    public class LogHelper
    {

        //static ILoggerRepository repository;
        static readonly ILog loggerInfo;
        static readonly ILog loggerError;
        static readonly ILog loggerMonitor;
        static readonly string loggerSource;
        static LogHelper()
        {
           var repository = LogManager.CreateRepository("NETCoreRepository");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(string.Format("<configuration ID=\"log4net\"><log4net><!--Error日志--><logger name=\"LogError\"><level value=\"DEBUG\" /><appender-ref ref=\"RollingLogFileAppender\" /></logger><!--Info日志--><logger name=\"LogInfo\">  <level value=\"DEBUG\" />  <appender-ref ref=\"InfoAppender\" /></logger><!--监控日志--><logger name=\"LogMonitor\">  <level value=\"DEBUG\" />  <appender-ref ref=\"MonitorAppender\" /></logger><!--Info日志--><appender name=\"InfoAppender\" type=\"log4net.Appender.RollingFileAppender,log4net\" >  <param name=\"File\" value=\"{1}Logs/LogInfo/\" />  <!--添加到文件-->  <param name=\"AppendToFile\" value=\"true\" />  <!--根据日期滚动-->  <param name=\"RollingStyle\" value=\"Date\" />  <!--文件名称规则-->  <param name=\"DatePattern\" value=\"yyyy-MM-dd&quot;.txt&quot;\" />  <!--日志文件名是否是固定不变的-->  <param name=\"StaticLogFileName\" value=\"false\" />  <!--log保留天数-->  <param name= \"MaxSizeRollBackups\" value= \"{0}\"/><layout type=\"log4net.Layout.PatternLayout,log4net\"><param name=\"ConversionPattern\" value=\"%d [%t] %-5p %c - %m%n\" /><param name=\"Header\" value=\"&#13;&#10;----------------------header--------------------------&#13;&#10;\" /><param name=\"Footer\" value=\"&#13;&#10;----------------------footer--------------------------&#13;&#10;\" />  </layout></appender><!--错误日志--><appender name=\"RollingLogFileAppender\" type=\"log4net.Appender.RollingFileAppender,log4net\" >  <param name=\"File\" value=\"Logs/LogError/\" />  <param name=\"AppendToFile\" value=\"true\" />  <param name=\"RollingStyle\" value=\"Date\" />  <param name=\"DatePattern\" value=\"yyyy-MM-dd&quot;.txt&quot;\" />  <param name=\"StaticLogFileName\" value=\"false\" />  <layout type=\"log4net.Layout.PatternLayout,log4net\"><param name=\"ConversionPattern\" value=\"%d [%t] %-5p %c - %m%n\" /><param name=\"Header\" value=\"&#13;&#10;----------------------header--------------------------&#13;&#10;\" /><param name=\"Footer\" value=\"&#13;&#10;----------------------footer--------------------------&#13;&#10;\" />  </layout></appender><!--监控日志--><appender name=\"MonitorAppender\" type=\"log4net.Appender.RollingFileAppender,log4net\" >  <param name=\"File\" value=\"Logs/LogMonitor/\" />  <param name=\"AppendToFile\" value=\"true\" />  <param name=\"RollingStyle\" value=\"Date\" />  <param name=\"DatePattern\" value=\"yyyy-MM-dd&quot;.txt&quot;\" />  <param name=\"StaticLogFileName\" value=\"false\" />  <layout type=\"log4net.Layout.PatternLayout,log4net\"><param name=\"ConversionPattern\" value=\"%d [%t] %-5p %c - %m%n\" /><param name=\"Header\" value=\"&#13;&#10;----------------------header--------------------------&#13;&#10;\" /><param name=\"Footer\" value=\"&#13;&#10;----------------------footer--------------------------&#13;&#10;\" />  </layout></appender>  </log4net></configuration>", 10, ""));
            XmlElement element = (XmlElement)doc.GetElementsByTagName("log4net")[0];
            XmlConfigurator.Configure(repository, element);

            loggerInfo = LogManager.GetLogger(repository.Name, "LogInfo");
            loggerError = LogManager.GetLogger(repository.Name, "LogError");
            loggerMonitor = LogManager.GetLogger(repository.Name, "LogMonitor");
            loggerSource = ConfigCenter.ConfigCenterHelper.GetInstance().Get("LogConfig.LogSource");
        }
        
        public static void TryLog<T>(T logInfo, LogType logType, string className = null, LogLevel logLevel = LogLevel.Info, bool sendMail = false)
        {
            string message = string.Empty;
            if (logInfo is string)
            {
                LogInfoDto dto = new LogInfoDto
                {
                    Message = logInfo as string,
                    Level = logLevel,
                    Class = className,
                    Source = loggerSource
                };
                message = dto.ToJson();
                
            }
            else if (logInfo is LogInfoDto)
            {
                var data = logInfo as LogInfoDto;
                data.Source = loggerSource;
                message = data.ToJson();
            }
            else if(logInfo is LogMonitorDto)
            {
                var data = logInfo as LogMonitorDto;
                data.Source = loggerSource;
                message = data.ToJson();
            }

            if (logType == LogType.File || logType == LogType.FileAndDb)
            {
                if (logLevel <= LogLevel.Info)
                    loggerInfo.Info(message);
                else if (logLevel <= LogLevel.Warn)
                    loggerMonitor.Warn(message);
                else
                    loggerError.Error(message);
            }


            if (logType == LogType.Db || logType == LogType.FileAndDb)
            {
                Task.Factory.StartNew(() =>
                {
                    new RabbitMq.RabbitMqService(ConfigCenter.ConfigCenterHelper.GetInstance().Get("RabbitMqConfig.Url")).Publish("log.BaseFrameLog"
                        , (logInfo is LogMonitorDto) ? "log.BaseFrameLogMonitor" : "log.BaseFrameLogInfo"
                        , (logInfo is LogMonitorDto) ? "log.BaseFrameLogMonitor" : "log.BaseFrameLogInfo"
                        , message
                        , true);
                });
            }
        }

        public static void TryLog(string logInfo, LogType logType, string className = null, LogLevel logLevel = LogLevel.Info, bool sendMail = false)
        {
            string message = string.Empty;
            if (logInfo is string)
            {
                LogInfoDto dto = new LogInfoDto
                {
                    Message = logInfo as string,
                    Level = logLevel,
                    Class = className,
                    Source = loggerSource
                };
                message = dto.ToJson();

            }
            else if (logInfo is LogInfoDto)
            {
                var data = logInfo as LogInfoDto;
                data.Source = loggerSource;
                message = data.ToJson();
            }
            else if (logInfo is LogMonitorDto)
            {
                var data = logInfo as LogMonitorDto;
                data.Source = loggerSource;
                message = data.ToJson();
            }

            if (logType == LogType.File || logType == LogType.FileAndDb)
            {
                if (logLevel <= LogLevel.Info)
                    loggerInfo.Info(message);
                else if (logLevel <= LogLevel.Warn)
                    loggerMonitor.Warn(message);
                else
                    loggerError.Error(message);
            }


            if (logType == LogType.Db || logType == LogType.FileAndDb)
            {
                Task.Factory.StartNew(() =>
                {
                    new RabbitMq.RabbitMqService(ConfigCenter.ConfigCenterHelper.GetInstance().Get("RabbitMqConfig.Url")).Publish("log.BaseFrameLog"
                        , (logInfo is LogMonitorDto) ? "log.BaseFrameLogMonitor" : "log.BaseFrameLogInfo"
                        , (logInfo is LogMonitorDto) ? "log.BaseFrameLogMonitor" : "log.BaseFrameLogInfo"
                        , message
                        , true);
                });
            }
        }

        public static void TryLogMonitor(LogMonitorDto dto)
        {
            TryLog(dto, LogType.Db, "", dto.Level, sendMail: dto.Level == LogLevel.Error);
        }
        /// <summary>
        /// 写入API监控日志
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <param name="timeTotal">请求耗时</param>
        /// <param name="exception">异常</param>
        /// <param name="exceptionResponse">发生异常时响应</param>
        public static void WriteApiMonitorLog(HttpContext context, string responseMsg, long timeTotal, string exception = null, string exceptionResponse = null)
        {

            LogMonitorDto monitorDto = new LogMonitorDto(context)
            {
                BeginTime = DateTime.Now.AddMilliseconds(timeTotal),
                TimeTotal = timeTotal,
                ModuleType = ModuleType.api_dotnet,
                ApiUrl = context.Request.Path,
                Arguments = context.GetClientPostData(),
                Level = LogLevel.Info
            };

            if (!exception.IsNullOrEmpty())
            {
                monitorDto.Exception = exception;
                monitorDto.Level = LogLevel.Error;
                if (exceptionResponse.IsNullOrEmpty())
                    monitorDto.Message = exception;
                else
                    monitorDto.Message = exceptionResponse;
            }
            else
                monitorDto.Message = responseMsg;

            Task.Factory.StartNew(() =>
            {
                //TryLogMonitor(monitorDto);
            });
        }
    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// The TRACE Level designates finer-grained informational events than the DEBUG.
        /// </summary>
        Trace,
        /// <summary>
        /// The DEBUG Level designates fine-grained informational events that are most useful to debug an application. 
        /// </summary>
        Debug,
        /// <summary>
        /// The INFO level designates informational messages that highlight the progress of the application at coarse-grained level. 
        /// </summary>
        Info,
        /// <summary>
        /// The WARN level designates potentially harmful situations. 
        /// </summary>
        Warn,
        /// <summary>
        /// The ERROR level designates error events that might still allow the application to continue running.
        /// </summary>
        Error,
        /// <summary>
        /// The FATAL level designates very severe error events that will presumably lead the application to abort.
        /// </summary>
        Fatal
    }

    /// <summary>
    /// 日志记录类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 记录在文件
        /// </summary>
        File = 1,
        /// <summary>
        ///记录在数据库
        /// </summary>
        Db = 2,
        /// <summary>
        /// 同时记录文件和数据库
        /// </summary>
        FileAndDb = 3
    }

    public enum ModuleType
    {
        unknown,
        api_dotnet,
        api_client_dotnet,
        web_dotnet,
    }
}
