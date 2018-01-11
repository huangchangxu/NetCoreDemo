using PaymentCenter.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.ConsoleApp
{
    public class DbTest
    {
        public static void GetTest()
        {
            Repositorys.DBRepositorys.TransactionRecordRepository repository = new Repositorys.DBRepositorys.TransactionRecordRepository();
            Console.WriteLine(repository.GetTransactionRecord(5283).ToJson());
        }
    }
}
