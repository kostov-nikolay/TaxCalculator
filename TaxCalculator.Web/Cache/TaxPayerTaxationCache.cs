using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Web.Cache
{
    public class TaxPayerTaxationCache
    {
        public TaxPayer TaxPayer { get; }

        public Taxes Taxes { get; }

        public TaxPayerTaxationCache(TaxPayer taxPayer, Taxes taxes)
        {
            this.Taxes = taxes;
            this.TaxPayer = taxPayer;
        }
    }
}
