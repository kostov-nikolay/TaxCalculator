using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Web
{
    public class Taxes
    {
        public decimal GrossIncome { get; set; }

        public decimal CharitySpent { get; set; }

        public decimal IncomeTax { get; set; }

        public decimal SocialTax { get; set; }

        public decimal TotalTax
        {
            get
            {
                return IncomeTax + SocialTax;
            }
        }

        public decimal NetIncome
        {
            get
            {
                return GrossIncome - TotalTax;

            }
        }
    }
}
