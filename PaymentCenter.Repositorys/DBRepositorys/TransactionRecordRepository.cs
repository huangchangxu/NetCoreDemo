using Dapper;

namespace PaymentCenter.Repositorys.DBRepositorys
{
    public sealed class TransactionRecordRepository:DbRepository
    {
        public Models.TransactionRecord GetTransactionRecord(int id)
        {
            return readOnlyDbConnection.QueryFirst<Models.TransactionRecord>("select * from TransactionRecord where id=@id", new { id });
        }

    }
}
