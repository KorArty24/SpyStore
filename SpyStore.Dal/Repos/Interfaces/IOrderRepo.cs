﻿using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.Dal.Repos.Interfaces
{
    public interface IOrderRepo: IRepo<Order>
    {
        IList<Order> GetOrderHistory();
        OrderWithDetailsAndProductInfo GetOneWithDetails(int orderId);
    }
}
