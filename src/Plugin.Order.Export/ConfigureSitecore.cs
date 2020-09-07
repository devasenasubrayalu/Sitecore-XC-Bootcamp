using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Blocks;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Orders;

namespace Plugin.Bootcamp.Exercises.Order.Export
{
    public class ConfigureSitecore : IConfigureSitecore
    {
#pragma warning disable CA1822 // Mark members as static
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CA1822 // Mark members as static
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            
            services.Sitecore().Pipelines(config => config
            /*Block to display the export details in the Order*/
              .ConfigurePipeline<IGetEntityViewPipeline>(configure =>
              {
                  configure.Add<OrdersEntityViewBlock>().After<GetOrderLinesViewBlock>();
              })
              /*Blocks to get the orders list and to export in file path*/
              .AddPipeline<IExportOrderMinionPipeline, ExportOrderMinionPipeline>(configure =>

              {
                  configure.Add<RetrieveOrderBlock>().Add<ExportOrderToFileBlock>();
              })

             );


        }
    }
}
