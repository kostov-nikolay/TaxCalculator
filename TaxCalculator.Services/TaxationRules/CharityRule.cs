using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services.TaxationRules
{
    public class CharityRule : TaxRuleBase
    {
        public CharityRule(int order, TaxRate taxRate) : base(order, taxRate, TaxRuleTypes.Charity)
        {

        }

        public override TaxationRuleResult Apply(TaxIncome taxIncome)
        {
            if (!this.IsAppliable(taxIncome))
            {
                return new TaxationRuleResult(0, taxIncome, this.TaxType);
            }

            decimal percent = 100;
            var maxAllowedCharityAmount = taxIncome.GrossValue * this.TaxRate.Value / percent;

            // We are getting min value.
            // If we spent more than allowed, we have to take max allowed
            // If we spent less than max allowed, we have to take what we have spent
            // max=100, spent=50 => take 50
            // max=100, spent=200 => take 100
            // (take min value of two)
            var respectedCharityAmount = Math.Min(maxAllowedCharityAmount, taxIncome.CharityValue);

            // We modify gross value here
            // Other rule will get the new value
            taxIncome.GrossValue = taxIncome.GrossValue - respectedCharityAmount;
            return new TaxationRuleResult(respectedCharityAmount, taxIncome, TaxType);
        }

        public override bool IsAppliable(TaxIncome taxIncome)
        {
            return taxIncome.CharityValue > 0;
        }
    }
}
