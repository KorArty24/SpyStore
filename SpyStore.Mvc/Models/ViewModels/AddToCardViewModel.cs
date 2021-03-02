using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SpyStore.Models.ViewModels;
using SpyStore.Mvc.Validation;

namespace SpyStore.Mvc.Models.ViewModels
{
    public class AddToCardViewModel :CartRecordWithProductInfo
    {
        [Required]
        [MustNotBeGreaterThan(nameof(UnitsInStock))]
        [MustBeGreaterThanZero]
        public new int Quantity { get;set  }
    }
}
