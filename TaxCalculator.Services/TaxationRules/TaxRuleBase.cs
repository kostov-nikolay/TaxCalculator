using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services.TaxationRules
{
    /// <summary>
    /// Present basic abstract class for ITaxRule interface.
    /// </summary>
    /// <seealso cref="TaxCalculator.Services.ITaxRule&lt;TaxCalculator.Services.TaxRate&gt;" />
    public abstract class TaxRuleBase : ITaxRule<TaxRate>
    {
        public int Order
        {
            get
            {
                return _order;
            }
        }

        private readonly TaxRate _taxRate;
        private readonly int _order;
        private readonly TaxRuleTypes _taxType;

        protected TaxRate TaxRate { get { return _taxRate; } }

        public TaxRuleTypes TaxType { get { return _taxType; } }

        protected TaxRuleBase(int order, TaxRate taxRate, TaxRuleTypes taxType)
        {
            this._order = order;
            this._taxRate = taxRate;
            this._taxType = taxType;
        }
        public abstract bool IsAppliable(TaxIncome taxIncome);

        public abstract TaxationRuleResult Apply(TaxIncome taxIncome);

    }
}
