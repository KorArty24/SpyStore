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
   public class ShoppingCartRepo: RepoBase<ShoppingCartRecord>, IShoppingCartRepo

    {
        private readonly StoreContext context;
        private readonly IProductRepo productRepo;
        private readonly ICustomerRepo customerRepo;

        public ShoppingCartRepo(StoreContext context, IProductRepo productRepo, ICustomerRepo customerRepo): base(context)
        {
            
            this.productRepo = productRepo;
            this.customerRepo = customerRepo;
        }

        internal ShoppingCartRepo(DbContextOptions<StoreContext> options) : base(new StoreContext(options))
        {
            
            this.productRepo = new ProductRepo(Context);
            this.customerRepo = new CustomerRepo(Context);
            base.Dispose();
        }

        public int Add(ShoppingCartRecord entity, Product product, bool persist = true)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            this.productRepo.Dispose();
            this.customerRepo.Dispose();
            base.Dispose();
        }
        public override IEnumerable<ShoppingCartRecord> GetAll()
            => base.GetAll(x => x.DateCreated).ToList();

        public ShoppingCartRecord GetBy(int productId)
        {
            throw new NotImplementedException();
        }

        public CartRecordWithProductInfo GetShoppingCartRecord(int id) => Context.CartRecordWithProductInfo
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CartRecordWithProductInfo> GetShoppingCartRecords(int customerId)
        {
            throw new NotImplementedException();
        }

        public CartWithCustomerInfo GetShoppingCartRecordsWithCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public int Purchase(int customerId)
        {
            throw new NotImplementedException();
        }

        public int Update(ShoppingCartRecord entity, Product product, bool persist = true)
        {
            throw new NotImplementedException();
        }
    }
}
