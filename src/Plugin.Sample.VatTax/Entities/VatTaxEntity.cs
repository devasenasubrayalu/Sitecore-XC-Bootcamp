namespace Plugin.Bootcamp.Exercises.VatTax.Entities
{
    using System;
    using System.Collections.Generic;

    using Sitecore.Commerce.Core;
    
    public class VatTaxEntity : CommerceEntity
    {
        List<Component> Components = null;
        public VatTaxEntity()
        {
            /* Lab 3 - Creating custom entity for VAT TAX: Initializing properties */
            this.Components = new List<Component>();
            this.DateCreated = DateTime.UtcNow;
            this.DateUpdated = this.DateCreated;
            this.CountryCode = string.Empty;
            this.TaxTag = string.Empty;
            this.TaxPct = 1;
        }

        public VatTaxEntity(string id): this()
        {
            
            this.Id = id;
        }

        /* Adding Properties */

        /// <summary>
        /// CountryCode
        /// </summary>    
        public string CountryCode { get; set; }

        /// <summary>
        /// TaxTag
        /// </summary>
        public string TaxTag { get; set; }
        /// <summary>
        /// TaxPct
        /// </summary>
        public decimal TaxPct { get; set; }
    }
}