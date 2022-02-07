using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services
{
    /// <summary>
    /// Present Tax income data to on which we want to apply/calculate taxes.
    /// </summary>
    public class TaxIncome
    {
        public TaxIncome(decimal grossValue, decimal charityValue)
        {
            this.CharityValue = charityValue;
            this.GrossValue = grossValue;
        }
        public decimal GrossValue { get; set; }

        public decimal CharityValue { get; set; }
    }
}
