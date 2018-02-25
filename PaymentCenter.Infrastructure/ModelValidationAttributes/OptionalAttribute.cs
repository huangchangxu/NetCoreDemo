using PaymentCenter.Infrastructure.Extension;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PaymentCenter.Infrastructure.ModelValidationAttributes
{
    /// <summary>
    /// 二选一必填验证
    /// </summary>
    public class OptionalAttribute : ValidationAttribute
    {
        /// <summary>
        /// 成员名称
        /// </summary>
        private string memberName;

        /// <summary>
        /// 二选一必填验证
        /// </summary>
        /// <param name="memberName">关联成员名称</param>
        public OptionalAttribute(string memberName)
        {
            this.memberName = memberName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.IsNotNull())
                return ValidationResult.Success;

            var member = validationContext.ObjectInstance.GetType().GetProperties().Single(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
            if (member.GetCustomAttributes(true).Any(m => m is OptionalAttribute))
            {
                var attribute = member.GetCustomAttributes(true).Single(m => m is OptionalAttribute) as OptionalAttribute;
                if (attribute.memberName.Equals(validationContext.MemberName, StringComparison.OrdinalIgnoreCase))
                {
                    var newValue = member.GetValue(validationContext.ObjectInstance);

                    if (newValue.IsNull())
                    {
                        var errorMessage = $"{validationContext.MemberName}和{member.Name}两个字段不能同时为空";
                        return new ValidationResult(errorMessage);
                    }
                }

                return ValidationResult.Success;
            }
            else
                return ValidationResult.Success;

        }
    }
}
