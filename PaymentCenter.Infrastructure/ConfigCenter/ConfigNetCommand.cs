using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.ConfigCenter
{
    /// <summary>
    /// 网络命令
    /// </summary>
    public class ConfigNetCommand
    {
        public EnumCommandType CommandType { get; set; }
        public int CategoryID { get; set; }
        public string ProjectName { get; set; }

        /// <summary>
        /// 分布式系统网络命令 指定要操作的ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 或许同一台机器上 启动了同一个服务的多个应用  根据ip和实例名称 指定相应操作
        /// </summary>
        public string ProgramName { get; set; }
    }
    /// <summary>
    /// 网络命令类型
    /// </summary>
    public enum EnumCommandType
    {
        /// <summary>
        /// 配置更新
        /// </summary>
        ConfigUpdate = 0,
        /// <summary>
        /// 配置重新加载
        /// </summary>
        ConfigReload = 1,
    }
}
