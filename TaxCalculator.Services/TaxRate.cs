using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services
{
    /// <summary>
    /// Present Tax Rate information/parameters.
    /// </summary>
    public class TaxRate
    {

        public TaxRate(decimal minThreshold, decimal? maxThreshold, decimal value)
        {
            if (minThreshold < decimal.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(minThreshold));
            }
            if (maxThreshold.HasValue && maxThreshold.Value < decimal.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(maxThreshold));
            }
            if (maxThreshold.HasValue && maxThreshold.Value < minThreshold)
            {
                throw new ArgumentOutOfRangeException(nameof(maxThreshold), $"Value:'{maxThreshold.Value}' has to be greater than {nameof(minThreshold)}:'{minThreshold}'.");
            }
            if (value < decimal.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(maxThreshold));
            }
            this.MinThreshold = minThreshold;
            this.MaxThreshold = maxThreshold;
            this.Value = value;
        }

        public decimal MinThreshold { get; private set; }

        public decimal? MaxThreshold { get; private set; }

        public decimal Value { get; private set; }
    }
}
