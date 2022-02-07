using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services.TaxationRules
{
    public class SocialContributionRule : TaxRuleBase
    {
        public SocialContributionRule(int order, TaxRate taxRate) : base(order, taxRate, TaxRuleTypes.Social)
        {
            if(taxRate.MaxThreshold.GetValueOrDefault() <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(TaxRate.MaxThreshold), "Value is required to be defined and greater than 0.");
            }
        }

        public override TaxationRuleResult Apply(TaxIncome taxIncome)
        {
            if (!this.IsAppliable(taxIncome))
            {
                return new TaxationRuleResult(0, taxIncome, TaxType);
            }

            decimal taxableValue = taxIncome.GrossValue - this.TaxRate.MinThreshold;
            // TODO
            // 3.)	Social contributions of 15% are expected to be made as well. As for the previous case, the taxable income is whatever is above 1000 IDR but social contributions never apply to amounts higher than 3000.
            // In all example values for Social contribute is 15% over 2000, not 3000.
            // This is why we use this way to get max allowed tax value, instead of this.TaxRate.MaxThreshold value
            decimal maxAllowedTaxableValue = this.TaxRate.MaxThreshold.Value - this.TaxRate.MinThreshold;

            // the taxable income is whatever is above MinThreshold, but less than max one
            taxableValue = Math.Min(taxableValue, maxAllowedTaxableValue);

            decimal percent = 100;
            var taxValue = taxableValue * this.TaxRate.Value / percent;

            return new TaxationRuleResult(taxValue, taxIncome, this.TaxType);
        }

        public override bool IsAppliable(TaxIncome taxIncome)
        {
            return taxIncome.GrossValue > TaxRate.MinThreshold;
        }
    }
}
