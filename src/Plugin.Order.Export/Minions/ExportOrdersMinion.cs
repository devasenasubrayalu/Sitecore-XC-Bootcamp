using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using System;
using System.Threading.Tasks;
using System.Linq;
using XC = Sitecore.Commerce.Plugin.Orders;
using System.Globalization;

namespace Plugin.Bootcamp.Exercises.Order.Export.Minions
{
    public class ExportOrdersMinion : Minion
    {
        protected IExportOrderMinionPipeline ExportOrderPipeline { get; set; }

        public override void Initialize(IServiceProvider serviceProvider,
            MinionPolicy policy,
            CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, policy, globalContext);
            ExportOrderPipeline = serviceProvider.GetService<IExportOrderMinionPipeline>();
        }

        protected override async Task<MinionRunResultsModel> Execute()
        {
            MinionRunResultsModel runResults = new MinionRunResultsModel();

            /*Gets list of Items from ReleasedOrders and executes export order to file block*/

            long listCount = await GetListCount(Policy.ListToWatch).ConfigureAwait(false);
            this.Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "{0}-Review List {1}: Count:{2}", Name, Policy.ListToWatch, listCount));
            foreach (var listItems in (await GetListItems<XC.Order>(Policy.ListToWatch, Convert.ToInt32(listCount)).ConfigureAwait(false)))
            {
                this.Logger.LogDebug(string.Format(CultureInfo.InvariantCulture, "{0}-Reviewing Pending Order: {1}", listItems.Name, listItems.Id), Array.Empty<object>());

                var minionPipeline = ExportOrderPipeline;
                var ordersMinionArgument = new ExportOrderArgument(listItems.Id);
                using (var commerceContext = new CommerceContext(Logger, MinionContext.TelemetryClient, null))
                {
                    commerceContext.Environment = this.Environment;

                    CommercePipelineExecutionContextOptions executionContextOptions = new CommercePipelineExecutionContextOptions(commerceContext, null, null, null, null, null);
                    
                    var order = await minionPipeline.Run(ordersMinionArgument, executionContextOptions).ConfigureAwait(false);
                }
            }

            return runResults;
        }


    }
}
