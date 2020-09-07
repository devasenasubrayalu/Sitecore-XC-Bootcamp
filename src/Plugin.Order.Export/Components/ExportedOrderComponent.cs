using System;
using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Order.Export.Components
{
    public class ExportedOrderComponent : Component
    {
        /* Lab 5 - Order Export Minion : Creating properties */
        /// <summary>
        /// DateExported
        /// </summary>
        public DateTime? DateExported { get; set; }

        /// <summary>
        /// ExportFilename
        /// </summary>
        public string ExportFilename { get; set; }
    }
}
