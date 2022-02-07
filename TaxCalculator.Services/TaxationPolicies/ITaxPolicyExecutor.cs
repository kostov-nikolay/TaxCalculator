using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services.TaxationPolicies
{
    public interface ITaxPolicyExecutor
    {
        IEnumerable<TaxationRuleResult> CalculateTaxes(TaxIncome taxIncome);
    }
}
