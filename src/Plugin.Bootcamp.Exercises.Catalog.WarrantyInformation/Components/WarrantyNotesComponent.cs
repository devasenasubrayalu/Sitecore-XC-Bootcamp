using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Components
{
    public class WarrantyNotesComponent : Component
    {
        /* Lab 2: properties */

        /// <summary>
        /// WarrantyInformation
        /// </summary>
        public string WarrantyInformation { get; set; } = string.Empty;

        /// <summary>
        /// NumberOfYears
        /// </summary>
        public int NumberOfYears { get; set; } = 1;
    }
}
