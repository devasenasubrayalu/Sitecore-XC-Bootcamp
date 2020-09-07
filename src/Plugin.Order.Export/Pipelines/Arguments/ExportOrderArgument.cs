using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;

namespace Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Arguments
{
    public class ExportOrderArgument : PipelineArgument
    {   
        /// <summary>
        /// Order ID
        /// </summary>
        public string OrderId { get; set; }

        public ExportOrderArgument(string orderId)
        {
           /*Checks Order ID*/
            Condition.Requires<string>(orderId, "orderId").IsNotNullOrEmpty();

            this.OrderId = orderId;
        }
    }
}