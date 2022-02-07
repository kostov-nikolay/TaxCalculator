using System;
using TaxCalculator.Services.TaxationRules;
using Xunit;

namespace TaxCalculator.Services.Tests
{
    public class CharityRuleTests
    {

        public CharityRuleTests()
        {
        }

        [Theory()]
        [InlineData(10, 1000, 10, 15)]
        [InlineData(2, 1100, 2, 15)]
        [InlineData(14, 1000, 14, 15)]
        [InlineData(20, 1000, 20, 15)]
        [InlineData(15, 1000, 15, 15)]
        [InlineData(150, 1000, 150, 15)]
        [InlineData(150, 1000, 200, 15)]
        public void Calculate_Charity_Reduce(decimal expectedResult,
            decimal grossIncome,
            decimal charitySpent,
            decimal percentRate
            )
        {
            TaxRate taxRate = new TaxRate(decimal.MaxValue, decimal.MaxValue, percentRate);
            var rule = new CharityRule(-1, taxRate);

            var result = rule.Apply(new TaxIncome(grossIncome, charitySpent));
            Assert.Equal(expectedResult, result.TaxValue);

            // Make sure that we do !!!modify!! gross income of input argument.
            Assert.NotEqual(grossIncome, result.TaxIncome.GrossValue);
            // Make sure we modify as we expected with correct value
            var expectedGrossAfterCharity = grossIncome - result.TaxValue;
            Assert.Equal(expectedGrossAfterCharity, result.TaxIncome.GrossValue);
        }

        [Theory()]
        [InlineData(0, 1000, 0)] //Border case range
        [InlineData(0, 500, 0)] //Border case range
        public void Do_Not_Apply_Charity_Reduce(decimal expectedResult,
            decimal grossIncome,
            decimal charitySpent 
            )
        {
            decimal percentRate = 1;
            TaxRate taxRate = new TaxRate(decimal.MaxValue, decimal.MaxValue, percentRate);
            var rule = new CharityRule(-1, taxRate);

            var result = rule.Apply(new TaxIncome(grossIncome, charitySpent));
            Assert.Equal(expectedResult, result.TaxValue);

            // Make sure that we do !!!not modify!! gross income of input argument.
            Assert.Equal(grossIncome, result.TaxIncome.GrossValue);
        }
    }
}
