using System;

using Sitecore.Commerce.Core;
using Sitecore.Framework.Rules;
using Sitecore.Commerce.Plugin.Carts;

namespace Plugin.Bootcamp.Exercises.Promotions
{
    [EntityIdentifier("MinTemperatureCondition")]
    public class MinTemperatureCondition : ICartsCondition
    {
        /// <summary>
        ///Minimum Temperature
        /// </summary>
        public IRuleValue<Decimal> MinimumTemperature { get; set; }
        /// <summary>
        /// City
        /// </summary>
        public IRuleValue<String> City { get; set; }
        /// <summary>
        /// Country
        /// </summary>
        public IRuleValue<String> Country { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            /*Lab 4 : Extending Promotion by creating new condition */
            
            var minimumTemperature = MinimumTemperature.Yield(context);
            var city = City.Yield(context);
            var country = Country.Yield(context);

            CommerceContext commerceContext = context.Fact<CommerceContext>((string)null);
            var weatherServicePolicy = commerceContext.GetPolicy<WeatherServiceClientPolicy>();

            var currentTemperature = GetCurrentTemperature(city, country, weatherServicePolicy.ApplicationId);

            return currentTemperature > minimumTemperature;
        }

        public decimal GetCurrentTemperature(string city, string country, string applicationId)
        {
            /*Getting temperature from WeatherService */

            WeatherService weatherService = new WeatherService(applicationId);
            var temperature = weatherService.GetCurrentTemperature(city, country).Result;

            return (decimal) temperature.Max;
        }
    }
}
