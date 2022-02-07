using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Web
{
    public class TaxPayer
    {
        public string FullName { get; set; }

        public string SSN { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public decimal GrossIncome { get; set; }

        public decimal? CharitySpent { get; set; }
    }
}
