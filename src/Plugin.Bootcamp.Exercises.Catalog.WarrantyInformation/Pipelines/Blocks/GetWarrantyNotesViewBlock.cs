using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Components;

namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Pipelines.Blocks
{
    [PipelineDisplayName("GetWarrantyNotesViewBlock")]
    public class GetWarrantyNotesViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            try
            {
                var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();

                /* Lab 2: Run method - Views for Edit and non edit mode */


                //Getting the views //
                var isVariationView = arg.Name.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase);
                var isDetailsView = arg.Name.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase);
                var isWarrantyNotesView = arg.Name.Equals("WarrantyNotes", StringComparison.OrdinalIgnoreCase);
                var isConnectView = arg.Name.Equals(catalogViewsPolicy.ConnectSellableItem, StringComparison.OrdinalIgnoreCase);
                var request = context.CommerceContext.GetObject<EntityViewArgument>();

                //Checking the required views//
                if (string.IsNullOrEmpty(arg.Name) || !isDetailsView &&
                   !isWarrantyNotesView && !isVariationView && !isConnectView)
                {
                    return Task.FromResult(arg);
                }

                //Checking for Sellable Item//
                if (!(request.Entity is SellableItem))
                {
                    return Task.FromResult(arg);
                }

                var sellableItem = (SellableItem)request.Entity;

                // Checking base sellable item or one of its variations.//
                var variationId = string.Empty;
                if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
                {
                    variationId = arg.ItemId;
                }

                //Checking Edit mode or not//
                var isEditView = !string.IsNullOrEmpty(arg.Action) &&
                arg.Action.Equals("WarrantyNotes-Edit", StringComparison.OrdinalIgnoreCase);


                var componentView = arg;
                if (!isEditView)
                {
                    // If not in edit mode, Create a new view and add it to the current entity view. //
                    componentView = new EntityView
                    {
                        Name = "WarrantyNotes",
                        DisplayName = "Warranty Information",
                        EntityId = arg.EntityId,
                        EntityVersion = request.EntityVersion == null ? 1 : (int)request.EntityVersion,
                        ItemId = variationId
                    };

                    arg.ChildViews.Add(componentView);
                }

                // in edit view or view add properties to the component view //
                if (sellableItem != null && (sellableItem.HasComponent<WarrantyNotesComponent>(variationId) || isEditView || isConnectView))
                {
                    var component = sellableItem.GetComponent<WarrantyNotesComponent>(variationId);

                    componentView.Properties.Add(
                    new ViewProperty
                    {

                        Name = nameof(WarrantyNotesComponent.WarrantyInformation),
                        DisplayName = "Description",
                        RawValue = component.WarrantyInformation,
                        IsReadOnly = !isEditView,
                        IsRequired = false
                    });

                    componentView.Properties.Add(
                    new ViewProperty
                    {

                        Name = nameof(WarrantyNotesComponent.NumberOfYears),
                        DisplayName = "Warranty Term (years)",
                        RawValue = component.NumberOfYears,
                        IsReadOnly = !isEditView,
                        IsRequired = false
                    });


                }
            }
            catch (Exception ex)
            {
                context.CommerceContext.LogException("Something went wrong in GetWarrantyNotesViewBlock", ex);
                throw;
            }
            return Task.FromResult(arg);
        }
    }
}
