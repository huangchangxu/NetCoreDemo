using System;
using System.Xml.Serialization;
using PaymentCenter.PaymentCore.Alipay.Domain;

namespace PaymentCenter.PaymentCore.Alipay.Response
{
    /// <summary>
    /// AlipayCommerceEducateStudentinfoShareResponse.
    /// </summary>
    public class AlipayCommerceEducateStudentinfoShareResponse : AopResponse
    {
        /// <summary>
        /// 学生信息
        /// </summary>
        [XmlElement("student_info_share_result")]
        public EduStudentInfoShareResult StudentInfoShareResult { get; set; }
    }
}
