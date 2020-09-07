using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Order.Export.Policies
{
    public class OrderExportPolicy : Policy
    {
        /// <summary>
        /// File path to export the released orders 
        /// </summary>
        public string ExportLocation { get; set; } = "C:\\temp\\";
    }
}
