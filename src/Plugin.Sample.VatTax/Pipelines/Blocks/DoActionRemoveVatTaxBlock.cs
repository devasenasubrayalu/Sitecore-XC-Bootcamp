using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("DoActionRemoveDashboardEntity")]
    public class DoActionRemoveVatTaxBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionRemoveVatTaxBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(context != null);

            /* Removes the selected Vat Tax item */
            if (entityView == null
                || !entityView.Action.Equals("RemoveVatTax", StringComparison.OrdinalIgnoreCase))
            {
                return entityView;
            }

            try
            {
                await _commerceCommander.Command<DeleteEntityCommand>()
                    .Process(context.CommerceContext, entityView.ItemId).ConfigureAwait(false);
            }

            catch (Exception ex)
            {
                context.CommerceContext.LogException("Something went wrong in DoActionRemoveVatTaxBlock", ex);
                throw;
            }
            return entityView;
        }
    }
}
