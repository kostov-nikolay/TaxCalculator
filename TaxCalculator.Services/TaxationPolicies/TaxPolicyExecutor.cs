using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services.TaxationPolicies
{
    public class TaxPolicyExecutor : ITaxPolicyExecutor
    {
        private readonly IEnumerable<ITaxRule<TaxRate>> _taxRules;

        public TaxPolicyExecutor(IEnumerable<ITaxRule<TaxRate>> taxRules)
        {
            this._taxRules = taxRules;
        }
        public IEnumerable<TaxationRuleResult> CalculateTaxes(TaxIncome taxIncome)
        {
            IEnumerable<ITaxRule<TaxRate>> allAvaiableRulesForThisTaxIncome = this.GetDefinedTaxRules(taxIncome);
            var result = new List<TaxationRuleResult>();
            foreach (var rule in allAvaiableRulesForThisTaxIncome)
            {
                // We check each rule whether is applicable here, because each rule could change input arguments for next one.
                // For example charity rule is changing gross income. This could cause some of the followed rules to be skipped,
                // because of the changes or calculation made by previous one.
                var hasToApply = rule.IsAppliable(taxIncome);
                if (!hasToApply)
                {
                    continue;
                }
                var applyResult = rule.Apply(taxIncome);

                taxIncome = applyResult.TaxIncome;
                result.Add(applyResult);
            }

            return result;
        }

        private IEnumerable<ITaxRule<TaxRate>> GetDefinedTaxRules(TaxIncome taxIncome)
        {
            // We return all available tax rules ordered by property.
            return this._taxRules.OrderBy(x => x.Order);
        }
    }
}
