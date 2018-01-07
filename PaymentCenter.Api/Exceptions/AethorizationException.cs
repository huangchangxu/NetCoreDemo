using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PaymentCenter.Api.Exceptions
{
    /// <summary>
    /// 授权验证异常
    /// </summary>
    [Serializable]
    public class AuthorizationException:SystemException, ISerializable
    {
        public AuthorizationException(Enums.ApiStatusCode statusCode, string message="")
            : base(message)
        {
            this.StatusCode = statusCode;
        }
        public Enums.ApiStatusCode StatusCode { get; set; }
    }
}
