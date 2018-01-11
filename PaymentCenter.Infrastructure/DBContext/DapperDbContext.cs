using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using PaymentCenter.Infrastructure.Extension;

namespace PaymentCenter.Infrastructure.DBContext
{
    /// <summary>
    /// Dapper操作单例
    /// </summary>
    public class DapperDbContext
    {
        private static ConcurrentDictionary<string, IDbConnection> concurrentDictionary = new ConcurrentDictionary<string, IDbConnection>();
        private static readonly object locker = new object();

        private DapperDbContext()
        {
        }
        /// <summary>
        /// 获取DB连接实例
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IDbConnection GetDbConnection(DbType dbType, string connectionString)
        {
            var key = $"{dbType}_{connectionString}".ToMd5();
            IDbConnection dbConnection = null;

            if (!concurrentDictionary.ContainsKey(key) || concurrentDictionary[key].IsNull())
            {
                lock (locker)
                {
                    if (!concurrentDictionary.ContainsKey(key) || concurrentDictionary[key].IsNull())
                    {
                        
                        if (dbType == DbType.MySql)
                        {
                            dbConnection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                        }
                        else if (dbType == DbType.SqlServer)
                        {
                            dbConnection = new System.Data.SqlClient.SqlConnection(connectionString);
                        }
                        else
                        {
                            throw new ArgumentException("数据类型错误");
                        }

                        concurrentDictionary.TryAdd(key, dbConnection);
                    }
                    else
                        dbConnection = concurrentDictionary[key];
                }
            }
            else
                dbConnection= concurrentDictionary[key];
            
            return dbConnection;
        }
    }
}
