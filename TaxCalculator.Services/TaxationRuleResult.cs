using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services
{
    /// <summary>
    /// Present class with information about result from any TaxRule that is applied.
    /// </summary>
    public class TaxationRuleResult
    {
        public TaxationRuleResult(decimal taxValue, TaxIncome taxIncome, TaxRuleTypes taxType)
        {
            this.TaxIncome = taxIncome;
            this.TaxValue = taxValue;
            this.TaxType = taxType;
        }
        public decimal TaxValue { get; private set; }
        public TaxIncome TaxIncome { get; private set; }

        public TaxRuleTypes TaxType { get; set; }
    }

    public static class TaxationRuleResultExtentions
    {
        /// <summary>
        /// Finds the taxation value.
        /// If no such result is available in collection, 0 will be return.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="taxRuleType">Type of the tax rule.</param>
        /// <returns></returns>
        public static decimal FindTaxationValue(this IEnumerable<TaxationRuleResult> collection, TaxRuleTypes taxRuleType)
        {
            var r = collection.FirstOrDefault(x => x.TaxType == taxRuleType);
            if (r == null)
            {
                return 0;
            }
            return r.TaxValue;
        }
    }
}
