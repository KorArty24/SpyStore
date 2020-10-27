using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using SpyStore.Models.Entities.Base;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System.Text.Json.Serialization;

namespace SpyStore.Models.Entities

{
    [Table("ShoppingCartRecords", Schema = "Store")]
    public class ShoppingCartRecord :ShoppingCartRecordBase
    {
        [JsonIgnore]
        [ForeignKey(nameof(CustomerId))]
        public Customer CustomerNavigation { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(ProductId))]
        public Product ProductNavigation { get; set; }
    }
}
