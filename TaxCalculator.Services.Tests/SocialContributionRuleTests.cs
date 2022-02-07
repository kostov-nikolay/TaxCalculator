using System;
using TaxCalculator.Services.TaxationRules;
using Xunit;

namespace TaxCalculator.Services.Tests
{
    public class SocialContributionRuleTests
    {

        public SocialContributionRuleTests()
        {
        }


        [Theory()]
        [InlineData(300, 4000, 15)]
        [InlineData(300, 4450, 15)]
        [InlineData(380, 4000, 19)]
        [InlineData(380, 4450, 19)]
        public void Calculate_Tax_Above_MaxThreshold_Range(decimal expectedResult,
            decimal grossIncome,
            decimal percentRate)
        {
            var minThreshold = 1000;
            var maxThreshold = 3000;
            TaxRate taxRate = new TaxRate(minThreshold, maxThreshold, percentRate);
            var rule = new SocialContributionRule(-1, taxRate);

            var result = rule.Apply(new TaxIncome(grossIncome, 0));
            Assert.Equal(expectedResult, result.TaxValue);

            // Make sure that we do not modify gross income of input argument.
            Assert.Equal(grossIncome, result.TaxIncome.GrossValue);
        }

        [Theory()]
        [InlineData(367.5, 3450, 15, 1000, 4500)]
        [InlineData(655.5, 4450, 19, 1000, 4500)]
        [InlineData(0.02, 1001, 2, 1000, 4500)] //Border case range
        [InlineData(104.97, 4499, 3, 1000, 4500)] //Border case range
        public void Calculate_Tax_In_MaxThreshold_Range(decimal expectedResult,
            decimal grossIncome,
            decimal percentRate,
            decimal minThreshold,
            decimal maxThreshold
            )
        {
            TaxRate taxRate = new TaxRate(minThreshold, maxThreshold, percentRate);
            var rule = new SocialContributionRule(-1, taxRate);

            var result = rule.Apply(new TaxIncome(grossIncome, 0));
            Assert.Equal(expectedResult, result.TaxValue);

            // Make sure that we do not modify gross income of input argument.
            Assert.Equal(grossIncome, result.TaxIncome.GrossValue);
        }

        [Theory()]
        [InlineData(0, 999.99, 33, 1000, 4500)] //Border case range
        [InlineData(0, 1000, 31, 1000, 4500)] //Border case range
        [InlineData(0, 100, 34, 1000, 4500)]
        public void Do_Not_Apply_Any_Tax_Because_Of_Min_Gross(decimal expectedResult,
            decimal grossIncome,
            decimal percentRate,
            decimal minThreshold,
            decimal maxThreshold
            )
        {
            TaxRate taxRate = new TaxRate(minThreshold, maxThreshold, percentRate);
            var rule = new SocialContributionRule(-1, taxRate);

            var result = rule.Apply(new TaxIncome(grossIncome, 0));
            Assert.Equal(expectedResult, result.TaxValue);

            // Make sure that we do not modify gross income of input argument.
            Assert.Equal(grossIncome, result.TaxIncome.GrossValue);
        }
    }
}
