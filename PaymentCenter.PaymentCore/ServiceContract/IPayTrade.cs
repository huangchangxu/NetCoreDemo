using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.PaymentCore.ServiceContract
{
    /// <summary>
    /// 付款交易约束接口
    /// </summary>
    public interface IPayTrade
    {
        PaymentUnifiedResponse<TOut> TradeExecute<TOut, TInput>(TInput input);
    }
}
