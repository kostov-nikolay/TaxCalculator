using System;
using System.Collections.Generic;
using System.Linq;
using TaxCalculator.Services.TaxationPolicies;
using TaxCalculator.Services.TaxationRules;
using Xunit;

namespace TaxCalculator.Services.Tests
{
    public class TaxPolicyExecutorTests
    {
        public TaxPolicyExecutorTests()
        {
        }

        private static IEnumerable<ITaxRule<TaxRate>> GetRules()
        {
            List<ITaxRule<TaxRate>> rules = new List<ITaxRule<TaxRate>>()
            {
                new CharityRule(-1, new TaxRate(Decimal.MaxValue, Decimal.MaxValue, 10)),
                new IncomeRule(10, new TaxRate(1000, null, 10)),
                new SocialContributionRule(20, new TaxRate(1000, 3000, 15))
            };

            return rules;
        }

        [Theory()]
        [InlineData("Example1: George", 980, 0, 0, 0, 0)]
        [InlineData("Example2: Irina", 3400, 0, 240, 300, 0)]
        [InlineData("Example3: Mick", 2500, 150, 135, 202.5, 150)]
        [InlineData("Example3: Bill", 3600, 520, 224, 300, 360)]
        public void Validate_Example_Value(string exampleDescription, 
            decimal grossIncome, 
            decimal charitySpent,
            decimal expectedIncomeTax,
            decimal expectedSocialTax,
            decimal expectedCharityReduce)
        {
            var rules = GetRules();
            TaxPolicyExecutor s = new TaxPolicyExecutor(rules);
            var taxes = s.CalculateTaxes(new TaxIncome(grossIncome, charitySpent)).ToList();

            var actualIncomeTax = taxes.FindTaxationValue(TaxRuleTypes.Income);
            var actualSocialTax = taxes.FindTaxationValue(TaxRuleTypes.Social);
            var actualCharityReduce = taxes.FindTaxationValue(TaxRuleTypes.Charity);

            Assert.Equal(expectedIncomeTax, actualIncomeTax);
            Assert.Equal(expectedSocialTax, actualSocialTax);
            Assert.Equal(expectedCharityReduce, actualCharityReduce);
        }

        [Fact]
        public void Respect_Order_Of_The_Policies()
        {
            var policy1 = new Moq.Mock<ITaxRule<TaxRate>>();
            policy1.Setup(x => x.Order).Returns(1);
            policy1.Setup(x => x.TaxType).Returns(TaxRuleTypes.Social);
            policy1.Setup(x => x.IsAppliable(Moq.It.IsAny<TaxIncome>())).Returns(true);
            policy1.Setup(x => x.Apply(Moq.It.IsAny<TaxIncome>())).Returns(new TaxationRuleResult(2, null, TaxRuleTypes.Social));

            var policy2 = new Moq.Mock<ITaxRule<TaxRate>>();
            policy2.Setup(x => x.Order).Returns(-1);
            policy2.Setup(x => x.TaxType).Returns(TaxRuleTypes.Income);
            policy2.Setup(x => x.IsAppliable(Moq.It.IsAny<TaxIncome>())).Returns(true);
            policy2.Setup(x => x.Apply(Moq.It.IsAny<TaxIncome>())).Returns(new TaxationRuleResult(1, null, TaxRuleTypes.Income));

            var rules = new List<ITaxRule<TaxRate>>();
            rules.Add(policy1.Object);
            rules.Add(policy2.Object);
            TaxPolicyExecutor s = new TaxPolicyExecutor(rules);
            var result = s.CalculateTaxes(new TaxIncome(1, 1)).ToList();
            Assert.Equal(2, result.Count);

            //First it has to execute policy2, because it has order: -1
            Assert.Equal(TaxRuleTypes.Income, result[0].TaxType);
            Assert.Equal(1, result[0].TaxValue);

            Assert.Equal(TaxRuleTypes.Social, result[1].TaxType);
            Assert.Equal(2, result[1].TaxValue);

        }

        [Fact]
        public void Respect_IsAppliable_Of_The_Policies()
        {
            var policy1 = new Moq.Mock<ITaxRule<TaxRate>>(Moq.MockBehavior.Strict);
            policy1.Setup(x => x.Order).Returns(1);
            policy1.Setup(x => x.TaxType).Returns(TaxRuleTypes.Social);
            policy1.Setup(x => x.IsAppliable(Moq.It.IsAny<TaxIncome>())).Returns(true);
            policy1.Setup(x => x.Apply(Moq.It.IsAny<TaxIncome>())).Returns(new TaxationRuleResult(2, null, TaxRuleTypes.Social));

            var policy2 = new Moq.Mock<ITaxRule<TaxRate>>(Moq.MockBehavior.Strict);
            policy2.Setup(x => x.Order).Returns(-1);
            policy2.Setup(x => x.TaxType).Returns(TaxRuleTypes.Income);
            policy2.Setup(x => x.IsAppliable(Moq.It.IsAny<TaxIncome>())).Returns(false);
            policy2.Setup(x => x.Apply(Moq.It.IsAny<TaxIncome>())).Returns(new TaxationRuleResult(1, null, TaxRuleTypes.Income));

            var rules = new List<ITaxRule<TaxRate>>();
            rules.Add(policy1.Object);
            rules.Add(policy2.Object);
            TaxPolicyExecutor s = new TaxPolicyExecutor(rules);
            var result = s.CalculateTaxes(new TaxIncome(1, 1)).ToList();
            Assert.Single(result);

            // Make sure that we do not call Apply method on policy2, because it should return false for IsAppliable method
            policy2.Verify(x => x.Apply(Moq.It.IsAny<TaxIncome>()), Moq.Times.Never());

            //We should have execute only policy1
            Assert.Equal(TaxRuleTypes.Social, result[0].TaxType);
            Assert.Equal(2, result[0].TaxValue);
        }
    }
}
