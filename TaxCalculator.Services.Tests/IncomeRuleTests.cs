using System;
using TaxCalculator.Services.TaxationRules;
using Xunit;

namespace TaxCalculator.Services.Tests
{
    public class IncomeRuleTests
    {

        public IncomeRuleTests()
        {
        }

        [Theory()]
        [InlineData(600, 30, 1000, 3000)]
        [InlineData(150, 15, 1000, 2000)]
        [InlineData(0.15, 15, 1000, 1001)]

        public void Calculate_Tax(decimal expectedResult, decimal percentRate, decimal minThreshold, decimal grossIncome)
        {
            TaxRate taxRate = new TaxRate(minThreshold, null, percentRate);
            var rule = new IncomeRule(-1, taxRate);

            var result = rule.Apply(new TaxIncome(grossIncome, 0));
            Assert.Equal(expectedResult, result.TaxValue);
            // Make sure that we do not modify gross income of input argument.
            Assert.Equal(grossIncome, result.TaxIncome.GrossValue);
        }

        [Theory()]
        [InlineData(0, 1000, 800)]
        [InlineData(0, 1000, 999)]
        [InlineData(0, 1000, 1000)]
        public void Do_Not_Apply_Any_Tax_Because_Of_Min_Gross(decimal expectedResult, decimal minThreshold, decimal grossIncome)
        {
            TaxRate taxRate = new TaxRate(minThreshold, null, 1);
            var rule = new IncomeRule(-1, taxRate);

            var result = rule.Apply(new TaxIncome(grossIncome, 0));
            Assert.Equal(expectedResult, result.TaxValue);
            // Make sure that we do not modify gross income of input argument.
            Assert.Equal(grossIncome, result.TaxIncome.GrossValue);
        }
    }
}
