using PaymentCenter.Infrastructure.DBContext;
using System.Data;

namespace PaymentCenter.Repositorys
{
    /// <summary>
    /// 基础仓储
    /// </summary>
    public class DbRepository
    {
        public IDbConnection readOnlyDbConnection;

        public IDbConnection writeReadDbConnection;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public virtual Infrastructure.DBContext.DbType DbType { get { return Infrastructure.DBContext.DbType.MySql; } }
        /// <summary>
        /// 只读连接
        /// </summary>
        public virtual string ReadOnlyDbConnectionString { get { return "server=192.168.2.8;user id=ms_test;persistsecurityinfo=true;database=ds_paymentcenter;Password=dev@ds365;charset=utf8;pooling=true;"; } }
        /// <summary>
        /// 读写连接
        /// </summary>
        public virtual string WriteReadDbConnectionString { get { return "server=192.168.2.8;user id=ms_test;persistsecurityinfo=true;database=ds_paymentcenter;Password=dev@ds365;charset=utf8;pooling=true;"; } }
        public DbRepository()
        {
            readOnlyDbConnection = DapperDbContext.GetDbConnection(DbType, ReadOnlyDbConnectionString);
            writeReadDbConnection = DapperDbContext.GetDbConnection(DbType, WriteReadDbConnectionString);
        }
    }
}
