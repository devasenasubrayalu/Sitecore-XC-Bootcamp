using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Bootcamp.Exercises.VatTax.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("EnsureActions")]
    public class PopulateVatTaxDashboardActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        
        public override Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(entityView != null);
            Contract.Requires(context != null);

            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");

            
            /* adds Add and Remove actions on the Vat Tax dashboard */
            var vatTaxViewActionsPolicy = entityView.GetPolicy<ActionsPolicy>();
            vatTaxViewActionsPolicy.Actions.Add(new EntityActionView
            {
                Name = "AddVatTax",
                DisplayName = "Adds Vat Tax",
                Description = "Adds a new Vat Tax Entry",
                IsEnabled = true,
                RequiresConfirmation = false,
                EntityView = "AddVatTaxView",
                Icon = "add"
            });

            vatTaxViewActionsPolicy.Actions.Add(new EntityActionView
            {
                Name = "RemoveVatTax",
                DisplayName = "Removes Vat Tax",
                Description = "Removes a Vat Tax Entry",
                IsEnabled = true,
                RequiresConfirmation = true,
                EntityView = string.Empty,
                Icon = "delete"
            });

            return Task.FromResult(entityView);
        }
    }
}
