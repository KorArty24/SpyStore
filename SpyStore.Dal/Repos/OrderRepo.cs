using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EfStructures;
using SpyStore.Dal.Repos.Base;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpyStore.Dal.Repos
{
    public class OrderRepo : RepoBase<Order>, IOrderRepo
    {
        private readonly IOrderDetailRepo orderDetailRepo;

        //public StoreContext Context { get; }

        public OrderRepo(StoreContext context, IOrderDetailRepo orderDetailRepo) : base(context)
        {
            
            this.orderDetailRepo = orderDetailRepo;
        }

        internal OrderRepo(DbContextOptions<StoreContext> options) : base(options)
        {
            this.orderDetailRepo = new OrderDetailRepo(Context);
        }
        public override void Dispose()
        {
            this.orderDetailRepo.Dispose();
            base.Dispose();
        }
        public IList<Order> GetOrderHistory()=> GetAll(x => x.OrderDate).ToList();

        public OrderWithDetailsAndProductInfo GetoOneWithDetails(int orderId)
        {
            throw new NotImplementedException();
        }
        public IList<Product> GetFeaturedWithCategoryName() => Table.Where(p => p.IsFeatured).
            Include.Where(p => p.CategoryNavigation).OrderBy(x => x.Details.ModelName).
            ToList();
       
    }
}
