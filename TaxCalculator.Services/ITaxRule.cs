using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services
{
    /// <summary>
    /// Interface that present any taxation rule.
    /// </summary>
    /// <typeparam name="T">Definition of the Rate data</typeparam>
    public interface ITaxRule<T> where T : TaxRate
    {
        /// <summary>
        /// Determines whether the specified tax income is appliable.
        /// </summary>
        /// <param name="taxIncome">The tax income.</param>
        /// <returns>
        ///   <c>true</c> if the specified tax income is appliable; otherwise, <c>false</c>.
        /// </returns>
        bool IsAppliable(TaxIncome taxIncome);

        /// <summary>
        /// Applies the specified tax income rule.
        /// </summary>
        /// <param name="taxIncome">The tax income.</param>
        /// <returns></returns>
        TaxationRuleResult Apply(TaxIncome taxIncome);

        /// <summary>
        /// Gets the order to execute this rule.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        int Order { get; }

        /// <summary>
        /// Gets the type of the tax.
        /// </summary>
        /// <value>
        /// The type of the tax.
        /// </value>
        TaxRuleTypes TaxType { get; }
    }
}
