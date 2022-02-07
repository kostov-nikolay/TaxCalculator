using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.Services.TaxationPolicies;

namespace TaxCalculator.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.RegisterTaxes(services); // TODO call DI method that will register all TaxRates 


            services.AddControllers();
            services.AddFluentValidation(x =>
            {
                x.DisableDataAnnotationsValidation = true;
                x.RegisterValidatorsFromAssemblyContaining<TaxPayerValidator>();
            }); // TODO Add fluent validation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaxCalculator.Web", Version = "v1" });
            });
            services.AddMemoryCache();// TODO add memory cache
        }

        private void RegisterTaxes(IServiceCollection services)
        {
            services.AddScoped<TaxCalculator.Services.ITaxRule<Services.TaxRate>>(x =>
            {
                return new Services.TaxationRules.SocialContributionRule(2, new Services.TaxRate(1000, 3000, 15));
            });
            services.AddScoped<TaxCalculator.Services.ITaxRule<Services.TaxRate>>(x =>
            {
                return new Services.TaxationRules.CharityRule(0, new Services.TaxRate(decimal.MaxValue, decimal.MaxValue, 10));
            });
            services.AddScoped<TaxCalculator.Services.ITaxRule<Services.TaxRate>>(x =>
            {
                return new Services.TaxationRules.IncomeRule(2, new Services.TaxRate(1000, null, 10));
            });

            services.AddScoped<ITaxPolicyExecutor, TaxPolicyExecutor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaxCalculator.Web v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
