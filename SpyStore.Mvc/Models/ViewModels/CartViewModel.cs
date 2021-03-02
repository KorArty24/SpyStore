using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using
using SpyStore.Models.Entities;

namespace SpyStore.Mvc.Models.ViewModels
{
    public class CartViewModel
    {
        public Customer Customer { get; set; }
        public IList<CartRecordViewModel> CartRecords { get; set; }
    }

}
