using Microsoft.Extensions.Logging;
using Plugin.Bootcamp.Exercises.VatTax.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.ManagedLists;
using Sitecore.Framework.Pipelines;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("DoActionAddDashboardEntity")]
    public class DoActionAddVatTaxBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionAddVatTaxBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(context != null);

            /*  Saves  new Vat Tax  item */

            if (entityView == null
                || !entityView.Action.Equals("AddVatTax", StringComparison.OrdinalIgnoreCase))
            {
                return entityView;
            }

            var taxTag = entityView.Properties.First(p => p.Name == "TaxTag").Value ?? "";
            var countryCode = entityView.Properties.First(p => p.Name == "CountryCode").Value ?? "";
            var taxPct = Convert.ToDecimal(entityView.Properties.First(p => p.Name == "TaxPct").Value ?? "0", CultureInfo.InvariantCulture);

            using (var vatTaxEntity = new VatTaxEntity
            {
                Id = CommerceEntity.IdPrefix<VatTaxEntity>() + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
                Name = string.Empty,
                TaxTag = taxTag,
                CountryCode = countryCode,
                TaxPct = taxPct
            })
            {
                vatTaxEntity.GetComponent<ListMembershipsComponent>().Memberships.Add(CommerceEntity.ListName<VatTaxEntity>());

                /*Saves the item*/
                await _commerceCommander.PersistEntity(context.CommerceContext, vatTaxEntity).ConfigureAwait(false);
            }



            return entityView;
        }
    }
}
