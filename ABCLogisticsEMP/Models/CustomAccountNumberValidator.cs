using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ABCLogisticsEMP.Models
{
    public class CustomAccountNumberValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string AccountNumber = value.ToString();

                AccountNumber = AccountNumber.Replace("-", "");

                if (AccountNumber.Length != 9)
                {
                    return new ValidationResult("Please Enter a Valid Account Number.");
                }

                try
                {
                    int res = Convert.ToInt32(AccountNumber);
                }
                catch (Exception)
                {

                    return new ValidationResult("Please Enter a Valid Account Number.");
                }

                // Run through each digit and calculate the total.

                int n = 0;
                int y = Convert.ToInt32("1");
        
                for (int i = 0; i < AccountNumber.Length; i += 3)
                {
                    n += Convert.ToInt32(char.GetNumericValue(AccountNumber[i])) * 3
                      + Convert.ToInt32(char.GetNumericValue(AccountNumber[i + 1])) * 7
                      + Convert.ToInt32(char.GetNumericValue(AccountNumber[i + 2]));
                }

                // If the resulting sum is an even multiple of ten (but not zero),
                // the aba routing number is good.

                if (n != 0 && n % 10 == 0)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Please Enter a Valid Account Number.");
            }
            else
            {
                return new ValidationResult("" + validationContext.DisplayName + " is required");
            }


        }
    }
}

