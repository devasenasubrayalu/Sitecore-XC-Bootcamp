using Microsoft.Extensions.DependencyInjection;
using Plugin.Bootcamp.Exercises.VatTax.EntityViews;
using Plugin.Bootcamp.Exercises.VatTax.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.BusinessUsers;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using System.Reflection;

namespace Plugin.Bootcamp.Exercises.VatTax
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
               /* Block to display new Vat Tax entity in Business Tools */
               .ConfigurePipeline<IBizFxNavigationPipeline>(d =>
                {
                    d.Add<GetVatTaxNavigationViewBlock>();
                })
                /*Block to calculate tax for the cart items*/
               .ConfigurePipeline<ICalculateCartLinesPipeline>(d =>
                {
                    d.Replace<Sitecore.Commerce.Plugin.Tax.CalculateCartLinesTaxBlock, CalculateCartLinesTaxBlockCustom>();
                })
                /*Block to create vat tax component view*/
                .ConfigurePipeline<IGetEntityViewPipeline>(d =>
                {
                    d.Add<GetVatTaxDashboardViewBlock>().Before<IFormatEntityViewPipeline>();
                })
                /*Block to populate add and remove actions on vat tax*/
                .ConfigurePipeline<IFormatEntityViewPipeline>(d =>
                 {
                     d.Add<PopulateVatTaxDashboardActionsBlock>().After<PopulateEntityViewActionsBlock>();
                 })
                 /*Block to add and remove tax item configuration*/
                .ConfigurePipeline<IDoActionPipeline>(d =>
                 {
                     d.Add<DoActionAddVatTaxBlock>().After<ValidateEntityVersionBlock>()
                         .Add<DoActionRemoveVatTaxBlock>().After<ValidateEntityVersionBlock>();
                 }));

            services.RegisterAllCommands(assembly);
        }
    }
}