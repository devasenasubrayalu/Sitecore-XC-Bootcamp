using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Policies;
using Sitecore.Commerce.Plugin.Orders;
using System.Diagnostics.Contracts;


namespace Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Blocks
{
    [PipelineDisplayName("OrderConfirmation.OrderConfirmationIdBlock")]
    public class OrderPlacedAssignCustomConfirmationIdBlock : PipelineBlock<Sitecore.Commerce.Plugin.Orders.Order, Sitecore.Commerce.Plugin.Orders.Order, CommercePipelineExecutionContext>
    {
        public override Task<Sitecore.Commerce.Plugin.Orders.Order> Run(Sitecore.Commerce.Plugin.Orders.Order arg, CommercePipelineExecutionContext context)
        {

            Contract.Requires(arg != null);
            Contract.Requires(context != null);
            /* Lab 1 : method to modify the order number */
            Condition.Requires(arg).IsNotNull($"{this.Name}:The Order can not be null");
            OrderPlacedAssignCustomConfirmationIdBlock confirmationIdBlock = this;
            string uniqueCode;
            try
            {

                uniqueCode = GetCustomOrder(context);

            }
            catch(Exception ex)
            {
                context.CommerceContext.LogException((confirmationIdBlock.Name) + "-UniqueCodeEception", ex);
                throw;
            }
            arg.OrderConfirmationId = uniqueCode;
            return Task.FromResult<Sitecore.Commerce.Plugin.Orders.Order>(arg);
            
        }
        //Method to update the order ID.
        private string GetCustomOrder(CommercePipelineExecutionContext context)
        {
            var policy = context.GetPolicy<OrderNumberPolicy>();
            return policy.IncludeDate == true ? $"{policy.Prefix },{DateTime.Today.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture)},{policy.Suffix},{Guid.NewGuid().ToString()}" :
                $"{policy.Prefix},{string.Empty},{policy.Suffix},{Guid.NewGuid().ToString()}";
        }
    }

}
