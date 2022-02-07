using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services.TaxationRules
{
    public class IncomeRule : TaxRuleBase
    {
        public IncomeRule(int order, TaxRate taxRate) : base(order, taxRate, TaxRuleTypes.Income)
        {

        }

        public override TaxationRuleResult Apply(TaxIncome taxIncome)
        {
            if (!this.IsAppliable(taxIncome))
            {
                return new TaxationRuleResult(0, taxIncome, TaxType);
            }
            var taxableValue = taxIncome.GrossValue - this.TaxRate.MinThreshold;
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
