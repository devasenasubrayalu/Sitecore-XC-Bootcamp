using Newtonsoft.Json;
using Plugin.Bootcamp.Exercises.Order.Export.Components;
using Plugin.Bootcamp.Exercises.Order.Export.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Blocks
{
    public class ExportOrderToFileBlock : PipelineBlock<Sitecore.Commerce.Plugin.Orders.Order, Sitecore.Commerce.Plugin.Orders.Order, CommercePipelineExecutionContext>
    {

        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public ExportOrderToFileBlock(IPersistEntityPipeline persistEntityPipeline)
        {
            this._persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<Sitecore.Commerce.Plugin.Orders.Order> Run(Sitecore.Commerce.Plugin.Orders.Order order, CommercePipelineExecutionContext context)
        {
            Contract.Requires(order != null);
            Contract.Requires(context != null);

            /*Checks the order and exports to the configured file path*/
            var exportComponent = order.GetComponent<ExportedOrderComponent>();

            exportComponent.DateExported = DateTime.Now;
            exportComponent.ExportFilename = context.GetPolicy<OrderExportPolicy>().ExportLocation + @"\" + order.OrderConfirmationId + ".json";
            var orderAsString = JsonConvert.SerializeObject(order);

            using (StreamWriter sw = new StreamWriter(exportComponent.ExportFilename))
            {
                await sw.WriteAsync(orderAsString).ConfigureAwait(false);
            }

            var persistEntityArgument = await _persistEntityPipeline.Run(new PersistEntityArgument(order), context).ConfigureAwait(false);
            return order;
        }
    }
}
