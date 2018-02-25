using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayMobilePublicMockListsmlistApiResponse.
    /// </summary>
    public class AlipayMobilePublicMockListsmlistApiResponse : AopResponse
    {
        /// <summary>
        /// 简单模型嵌套list
        /// </summary>
        [XmlArray("list_sm_model_list")]
        [XmlArrayItem("list_list_sm_mock_model")]
        public List<ListListSmMockModel> ListSmModelList { get; set; }
    }
}
