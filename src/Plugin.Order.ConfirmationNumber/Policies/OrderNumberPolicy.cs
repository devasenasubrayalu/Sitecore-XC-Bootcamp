using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Policies
{
    public class OrderNumberPolicy : Policy
    {
        public OrderNumberPolicy()
        {
            /* Lab 1: initialising the properties */
            this.Prefix = string.Empty;
            this.Suffix = string.Empty;
            this.IncludeDate = false;
            
        }
        /* Lab 1:  properties  */

        /// <summary>
        /// Prefix
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// Suffix
        /// </summary>
        public string Suffix { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public bool IncludeDate { get; set; }
    }
}
