using PaymentCenter.Infrastructure.Extension;
using PaymentCenter.Infrastructure.Redis;
using PaymentCenter.Infrastructure.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentCenter.Infrastructure.ConfigCenter
{
    /// <summary>
    /// 配置中心
    /// </summary>
    public class ConfigCenterHelper
    {
        private static string ConfigUrl => "";
        private static string _oldResult = string.Empty;

        private Dictionary<string, string> ConfigDictionary { get; set; }

        /// <summary>
        /// 记录上一次的字段信息
        /// </summary>
        private Dictionary<string, string> OldConfigDictionary { get; set; }

        private static ConfigCenterHelper _configManagerHelper;

        private ConfigCenterHelper()
        {
            ConfigDictionary = Init();
            //初始化
            OldConfigDictionary = ConfigDictionary;

            if (!TryGet("Debug").ToBoolean())
            {
                //订阅统一配置中心redis 修改和重启通知
                Task.Factory.StartNew(RedisSubscriptionHeartbeat);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static ConfigCenterHelper GetInstance()
        {
            return _configManagerHelper ?? (_configManagerHelper = new ConfigCenterHelper());
        }

        #region 站点心跳检测  WebHeartbeat()  和订阅心跳

        private void WebHeartbeat()
        {
            try
            {
                //获取ip
                var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(e => e.AddressFamily.ToString().Equals("InterNetwork"));
                while (true)
                {
                    RedisHelper.HashSet("DBF.WebHeartbeat", TryGet("LogConfig.LogSource", ""), DateTime.Now + "[" + ip + "]");
                    Thread.Sleep(1000 * 60);
                }
            }
            catch
            {
            }

        }
        /// <summary>
        /// 和配置中心的长连接可能会断掉
        /// </summary>
        private void RedisSubscriptionHeartbeat()
        {
            try
            {
                //while (true)
                //{
                //    Thread.Sleep(1000 * 60 * 5);
                RedisSubscription();
                ConfigDictionary = Init();
                //初始化
                OldConfigDictionary = ConfigDictionary;
                //}
            }
            catch
            {
            }

        }

        #endregion

        #region    订阅配置中心redis  RedisSubscription
        /// <summary>
        /// 订阅配置中心redis  实现配置的重新加载和项目的重启
        /// </summary>
        private void RedisSubscription()
        {
            string projectName = new Regex("projectName=([^&]+)").Match(ConfigUrl).Groups[1].Value.Trim();

            //订阅重启事件
            ConfigCenterRedisHelper.RedisSub(ConfigDictionary["ConfigCenter.redisHost"], ConfigDictionary["ConfigCenter.redisChannel"],
                 (channel, msg) =>
                 {
                     //获取到订阅消息
                     ConfigNetCommand result = msg.FromJson<ConfigNetCommand>();
                     if (ConfigDictionary["project.categoryids"].Contains("," + result.CategoryID + ",") ||
                         string.Equals(projectName, result.ProjectName, StringComparison.CurrentCultureIgnoreCase))
                     {
                         string info = "频道【" + channel + "】订阅客户端接收消息：" + ":" + msg;
                         if (result.CommandType == EnumCommandType.ConfigReload)
                         {
                             ConfigDictionary = Init();
                         }
                         else if (result.CommandType == EnumCommandType.ConfigUpdate)
                         {
                             OldConfigDictionary = ConfigDictionary;
                             ConfigDictionary = Init();
                         }
                     }
                 });
        }
        #endregion

        #region 初始化项目配置参数 Init()
        /// <summary>
        /// 初始化项目配置参数
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> Init()
        {
            Dictionary<string, string> list = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            //本地配置文件
            string localcachejsonpath = AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + "\\" + "temp\\" + "config.localcache.json";
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var result1 = "";
                    using (HttpClient client = new HttpClient())
                    {
                        result1 = client.PostAsync(ConfigUrl, new StringContent("")).Result.Content.ReadAsStringAsync().Result;
                    }
                    if (!string.IsNullOrWhiteSpace(result1))
                    {
                        result1 = new Regex("\"response\":(.*),\"total\"").Match(result1).Groups[1].Value.Trim();
                        if (string.IsNullOrWhiteSpace(result1))
                        {
                            throw new NotFiniteNumberException("获取系统配置信息错误..");
                        }
                        list = result1.FromJson<Dictionary<string, string>>();
                        //写入本地文件
                        if (!_oldResult.Equals(result1))
                        {
                            _oldResult = result1;
                            Task.Factory.StartNew(() =>
                            {
                                IOTool.CreateDirectory(localcachejsonpath);
                                IOTool.WriteTextFile(localcachejsonpath, result1);
                                // ReSharper disable once FunctionNeverReturns
                            });
                        }
                        break;
                        //return list;
                    }
                }
                catch (Exception ex)
                {
                    if (i != 2) continue;
                    //加载本地文件
                    if (File.Exists(localcachejsonpath))
                    {
                        try
                        {
                            list = IOTool.ReadTextFile(localcachejsonpath).FromJson<Dictionary<string, string>>();
                        }
                        catch (Exception ex1)
                        {
                            throw new Exception("试图读取统一配置中心出错，读取本地配置文件再次错误，请检查configUrl项配置", ex1);
                        }

                    }
                    else
                        throw new Exception("加载系统配置错误！", ex);
                }
            }
            if (list.Count < 1 || !list.ContainsKey("LogConfig.LogSource") || string.IsNullOrWhiteSpace(list["LogConfig.LogSource"]))
            {
                throw new Exception("获取网络配置错误,请检查LogConfig.LogSource,该项为项目标识不能为空");
            }
            return list;
        }
        #endregion
        
        #region 公共方法
        /// <summary>
        /// 获取配置到指定类型 仅仅限制于configkey对应的value值为json格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configkey"></param>
        /// <returns></returns>
        public T Get<T>(string configkey) where T : class
        {
            try
            {
                return ConfigDictionary[configkey].FromJson<T>();
            }
            catch (Exception ex)
            {
                throw new Exception("读取配置中心配置" + configkey + ",转化为" + typeof(T) + "出错，请检查配置项", ex);
            }

        }

        /// <summary>
        /// 获取某一项配置
        /// </summary>
        /// <param name="configkey"></param>
        /// <returns></returns>
        public string Get(string configkey)
        {

            try
            {
                return ConfigDictionary[configkey];
            }
            catch (Exception ex)
            {
                throw new Exception("读取配置中心配置" + configkey + ",出错，请确认配置中心是否有该配置项", ex);
            }
        }

        /// <summary>
        /// 尝试获取字典的值  没有不报错 返回默认值
        /// </summary>
        /// <param name="configkey"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public string TryGet(string configkey, string defaultvalue = "")
        {
            if (ConfigDictionary.ContainsKey(configkey))
                return ConfigDictionary[configkey];
            return defaultvalue;
        }

        /// <summary>
        /// 验证指定的键值对是否在配置中心中做了更改  仅仅针对于重新加载配置  不针对重启
        /// 也可前缀方式 比较两个字典Dictionary是否相同
        /// </summary>
        /// <param name="configKey"></param>
        /// <param name="prefix">该key是否为前缀的key</param>
        /// <returns>如果指定的配置键修改了 则返回true</returns>
        public bool CheckUpdate(string configKey, bool prefix = false)
        {
            if (prefix)
            {
                if (!configKey.EndsWith("."))
                    configKey = configKey + ".";
                var x = GetByprefix(configKey);
                var y = OldConfigDictionary.Where(dic => dic.Key.StartsWith(configKey)).ToDictionary(dic => dic.Key.Remove(0, configKey.Length), dic => dic.Value);
                if (x.Count != y.Count) return true;
                if (x.Keys.Except(y.Keys).Any()) return true;
                if (y.Keys.Except(x.Keys).Any()) return true;
                return !x.All(pair => string.Equals(pair.Value, y.TryGet(pair.Key), StringComparison.CurrentCultureIgnoreCase));
            }
            try
            {
                return !string.Equals(OldConfigDictionary.TryGet(configKey), ConfigDictionary.TryGet(configKey), StringComparison.CurrentCultureIgnoreCase);
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// 获取指定前缀的所有配置 
        /// </summary>
        /// <param name="prefix">前缀 符号.可有可无</param>
        /// <returns>去掉前缀的配置项</returns>
        public Dictionary<string, string> GetByprefix(string prefix)
        {
            if (!prefix.EndsWith("."))
                prefix = prefix + ".";
            return ConfigDictionary.Where(dic => dic.Key.StartsWith(prefix)).ToDictionary(dic => dic.Key.Remove(0, prefix.Length), dic => dic.Value);
        }

        /// <summary>
        /// 获取项目所有配置  还是不提供了
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAll()
        {
            return ConfigDictionary;
        }
        #endregion
    }
}
