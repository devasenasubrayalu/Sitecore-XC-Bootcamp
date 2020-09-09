using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Plugin.Bootcamp.Exercises.VatTax.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("GetVatTaxDashboardViewBlock")]
    public class GetVatTaxDashboardViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public GetVatTaxDashboardViewBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(entityView != null);
            Contract.Requires(context != null);
            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");

            /*Vat Tax component view*/
            try
            {
                if (!string.IsNullOrEmpty(entityView?.Name) && !entityView.Name.Equals("AddVatTaxView", StringComparison.OrdinalIgnoreCase)
                    && entityView.Name.Equals("VatTaxDashboard", StringComparison.OrdinalIgnoreCase))
                {
                    var vatTaxEntityView = entityView;
                    entityView.UiHint = "Table";
                    entityView.Icon = "cubes";
                    entityView.ItemId = string.Empty;

                    /*Gets the items and adds in view*/
                    var vatTaxEntities = await _commerceCommander.Command<ListCommander>()
                        .GetListItems<VatTaxEntity>(context.CommerceContext,
                            CommerceEntity.ListName<VatTaxEntity>(), 0, 99).ConfigureAwait(false);
                    foreach (var vatTaxEntity in vatTaxEntities)
                    {
                        vatTaxEntityView.ChildViews.Add(
                            new EntityView
                            {
                                ItemId = vatTaxEntity.Id,
                                Icon = "cubes",
                                Properties = new List<ViewProperty>
                                {
                            new ViewProperty {Name = "TaxTag", RawValue = vatTaxEntity.TaxTag },
                            new ViewProperty {Name = "CountryCode", RawValue = vatTaxEntity.CountryCode },
                            new ViewProperty {Name = "TaxPct", RawValue = vatTaxEntity.TaxPct }
                                }
                            });
                    }


                }
                /*View for add vat tax*/
                else if (entityView.Name.Equals("AddVatTaxView", StringComparison.OrdinalIgnoreCase))
                {
                    entityView.Properties.Add(
                    new ViewProperty
                    {
                        Name = "TaxTag",
                        IsHidden = false,
                        IsRequired = false,
                        RawValue = string.Empty
                    });

                    entityView.Properties.Add(
                        new ViewProperty
                        {
                            Name = "CountryCode",
                            IsHidden = false,
                            IsRequired = false,
                            RawValue = string.Empty
                        });

                    entityView.Properties.Add(
                        new ViewProperty
                        {
                            Name = "TaxPct",
                            IsHidden = false,
                            IsRequired = false,
                            RawValue = 0
                        });
                }
                else
                {
                    return entityView;
                }
            }
            catch (Exception ex)
            {
                context.CommerceContext.LogException("Something went wrong in GetVatTaxDashboardViewBlock", ex);
                throw;
            }
            return entityView;
        }
    }
}
