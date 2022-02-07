using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.Services;
using TaxCalculator.Services.TaxationPolicies;

namespace TaxCalculator.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;
        private readonly ITaxPolicyExecutor _taxPolicyExecutor;
        private readonly IMemoryCache _memoryCache;

        public CalculatorController(ILogger<CalculatorController> logger,
            ITaxPolicyExecutor taxPolicyExecutor,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _taxPolicyExecutor = taxPolicyExecutor;
            _memoryCache = memoryCache;
        }

        [HttpPost("Calculate")] // TODO add name of the action to be Calculate
        public Taxes Calculate(TaxPayer taxPayer)
        {
            // TODO add read cache and if they match - return cached data without executing calculation.
            Cache.TaxPayerTaxationCache cachedData;
            if (_memoryCache.TryGetValue<Cache.TaxPayerTaxationCache>(taxPayer.SSN, out cachedData))
            {
                if (cachedData.TaxPayer.GrossIncome.Equals(taxPayer.GrossIncome)
                    && cachedData.TaxPayer.CharitySpent.GetValueOrDefault().Equals(taxPayer.CharitySpent.GetValueOrDefault()))
                {
                    _logger.LogDebug($"Read data for taxes from cache and return cached result.");
                    return cachedData.Taxes;
                }
                _logger.LogDebug($"Cached data do not match request GrossIncome: '{cachedData.TaxPayer.GrossIncome}' or CharitySpent: '{cachedData.TaxPayer.CharitySpent}'. We are going to calculate again the taxation.");
            }
            // We do not have correct calculated taxes for this user and this input arguments. So lets calculate it.
            var allTaxesCalculationRaw = _taxPolicyExecutor.CalculateTaxes(new TaxIncome(taxPayer.GrossIncome, taxPayer.CharitySpent.GetValueOrDefault()));
            
            var incomeTax = allTaxesCalculationRaw.FindTaxationValue(TaxRuleTypes.Income);
            var socialTax = allTaxesCalculationRaw.FindTaxationValue(TaxRuleTypes.Social);

            var taxes = new Taxes
            {
                CharitySpent = taxPayer.CharitySpent.GetValueOrDefault(),
                GrossIncome = taxPayer.GrossIncome,
                IncomeTax = incomeTax,
                SocialTax = socialTax,
            };

            // TODO Save cache for later reuse
            _memoryCache.Set<Cache.TaxPayerTaxationCache>(taxPayer.SSN, new Cache.TaxPayerTaxationCache(taxPayer, taxes));
            _logger.LogDebug($"Save calculated taxation into cache for later reuse.");
            return taxes;
        }
    }
}
