using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentCenter.Infrastructure.Elasticsearch
{
    /// <summary>
    /// ElasticSearch执行结果
    /// </summary>
    public sealed class ElasticsearchExceResult
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 错误状态码
        /// </summary>
        public int ErrorStatus { get; set; }
        /// <summary>
        /// 错误原因
        /// </summary>
        public string ErrorReason { get; set; }
    }
}
