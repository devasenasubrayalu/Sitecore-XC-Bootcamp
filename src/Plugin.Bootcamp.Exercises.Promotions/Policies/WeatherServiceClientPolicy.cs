using Sitecore.Commerce.Core;


namespace Plugin.Bootcamp.Exercises.Promotions
{
    class WeatherServiceClientPolicy : Policy
    {
        /* Property to set API key of Open Weather Map Service  */
        public WeatherServiceClientPolicy()
        {
            this.ApplicationId = string.Empty;
        }

        /// <summary>
        /// Application Id
        /// </summary>
        public string ApplicationId { get; set; }
    }
}
