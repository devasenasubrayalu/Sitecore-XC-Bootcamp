using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Components;


namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Pipelines.Blocks
{
    [PipelineDisplayName("DoActionEditWarrantyNotesBlock")]
    class DoActionEditWarrantyNotesBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionEditWarrantyNotesBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            /* Lab 2: Run method for saving values from Edit view */

            //Checking edit action//
            if (string.IsNullOrEmpty(arg.Action) ||
            !arg.Action.Equals("WarrantyNotes-Edit", StringComparison.OrdinalIgnoreCase))
            {
                return arg;
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
            {
                return arg;
            }
            try
            {
                var component = entity.GetComponent<WarrantyNotesComponent>(arg.ItemId);

                // assigning entity view properties to component//

                component.WarrantyInformation = arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(WarrantyNotesComponent.WarrantyInformation), StringComparison.OrdinalIgnoreCase))?.Value;
                component.NumberOfYears = Int32.Parse(arg.Properties.FirstOrDefault(x =>
                  x.Name.Equals(nameof(WarrantyNotesComponent.NumberOfYears), StringComparison.OrdinalIgnoreCase))?.Value);

                // Persist/Save changes//
                await this._commerceCommander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);
            }
            catch(Exception ex)
            {
                context.CommerceContext.LogException("Something went wrong in DoActionEditWarrantyNotesBlock", ex);
                throw;
            }

            return arg;
        }
    }
}
