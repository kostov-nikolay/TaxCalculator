Projects:
1. TaxCalculator.Web
2. TaxCalculator.Services
3. TaxCalculator.Services.Tests

TaxCalculator.Web
Contains REST API controller.
Definitions of the rules is here using DI. See method RegisterTaxes.
Validation of input contracts is accomplish via Fluent Validation.
Memory cache is used here to check whether we have calculated already taxes for this Payer with same GrossIncome and ChrritySpent.

TaxCalculator.Services
Contains definitions of interfaces and classes for all available Taxes and rules how to apply them.
 - TaxRate - present some definition about the Tax such as its Rate Value, Min and Max Threshold
 - ITaxRule<TaxRate> - interface that has two methods IsAppliable and Apply. This interface has to be implemented and used 
for each tax rule definition. See CharityRule, IncomeRule, SocialContributionRule
 - TaxPolicyExecutor - present service that calculate the taxes. It go trough all available rules in provided order. Check 
 whether current rule has to be applied, and apply it. Get result from the rule and pass it to the next one. This give us a way
 to modify input arguments(for example gross income) for next rule (for example CharityRule has to reduce GrossIncome, before we execute TaxIncome).

TaxCalculator.Services.Tests
Project with unit tests.
In addition we use Moq to mock some of the rules. Validate and guarantee that TaxPolicyExecutor respect order of the rule.

