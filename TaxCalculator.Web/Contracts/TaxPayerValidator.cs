using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Web
{
    public class TaxPayerValidator : AbstractValidator<TaxPayer>
    {
        public TaxPayerValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
            // TODO apply FullName regex validation.
            // We take the basic regex from here and extend it to contains upper letters
            // https://stackoverflow.com/questions/27426057/regular-expression-validate-textbox-at-least-two-words-is-required
            RuleFor(x => x.FullName).Matches("^[a-zA-Z]+(?: [a-zA-Z]+)+$").WithMessage("'{PropertyName}' is invalid. It has to contains at least two words separated by space – allowed symbols letters and spaces only.");

            RuleFor(x => x.SSN).NotNull();
            // TODO apply SSN validation - digits from 5 to 10
            RuleFor(x => x.SSN).Matches("^[0-9]{5,10}$");
            RuleFor(x => x.GrossIncome).GreaterThan(decimal.Zero);
            RuleFor(x => x.CharitySpent).GreaterThanOrEqualTo(decimal.Zero).When(x => x.CharitySpent.HasValue);
        }
    }
}
