using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using PaymentCenter.Infrastructure.ModelValidationAttributes;

namespace PaymentCenter.ConsoleApp
{
    public class ModelValidTest
    {
        public static void Test()
        {
            model data = new model();
            data.bbb = "aaa";
            var validationContext = new ValidationContext(data);
            var resultValidation = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(data, validationContext, resultValidation, true);
           var errormsg = string.Join("|", resultValidation.Select(x => x.ErrorMessage));
        }

        public class model
        {
            [Optional("bbb")]
            public string aaa { get; set; }
            [Optional("aaa")]
            public string bbb { get; set; }
        }
    }
}
