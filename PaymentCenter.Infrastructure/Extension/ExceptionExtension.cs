using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Extension
{
    /// <summary>
    /// 异常扩展
    /// </summary>
    public static class ExceptionExtension
    {
        public static Exception GetInnestException(this Exception ex)
        {
            Exception innerException = ex.InnerException;
            Exception exception2 = ex;
            while (innerException != null)
            {
                exception2 = innerException;
                innerException = innerException.InnerException;
            }
            return exception2;
        }

        public static void WriteToLog(this Exception ex)
        {

        }
    }
}
