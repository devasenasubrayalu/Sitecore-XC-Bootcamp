using Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading.Tasks;
using XC = Sitecore.Commerce.Plugin.Orders;

namespace Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Blocks
{
    [PipelineDisplayName("Orders.block.RetrieveOrderBlock")]
    public class RetrieveOrderBlock : PipelineBlock<ExportOrderArgument, XC.Order, CommercePipelineExecutionContext>
    {
        private readonly IFindEntityPipeline _findEntityPipeline;

        public RetrieveOrderBlock(IFindEntityPipeline findEntityPipeline)
        {
            this._findEntityPipeline = findEntityPipeline;
        }

        public override async Task<XC.Order> Run(ExportOrderArgument arg, CommercePipelineExecutionContext context)
        {
            Contract.Requires(arg != null);
            Contract.Requires(context != null);

            XC.Order order = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(XC.Order), arg.OrderId, false), context).ConfigureAwait(false) as XC.Order;

            /* When the order is null,export execution will be aborted */
            if (order == null)
            {
                CommercePipelineExecutionContext executionContext = context;
                string reason = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "OrderNotFound", new object[1] {
          (object) arg.OrderId
                }, string.Format(CultureInfo.InvariantCulture,"Order {0} was not found.", arg.OrderId)).ConfigureAwait(false);
                executionContext.Abort(reason, (object)context);
                executionContext = null;
                return (XC.Order)null;
            }
            return order;
        }
    }
}
