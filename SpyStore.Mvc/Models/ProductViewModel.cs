using SpyStore.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyStore.Mvc.Models
{
    public class ProductViewModel : EntityBase
    {
        public ProductDetails Details { get; set; } = new ProductDetails;
        public bool IsFeatured { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }




    }
}
