using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EfStructures;
using SpyStore.Dal.Exceptions;
using SpyStore.Dal.Repos.Base;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SpyStore.Dal.Repos
{
    public class ShoppingCartRepo : RepoBase<ShoppingCartRecord>, IShoppingCartRepo

    {
        
        private readonly IProductRepo productRepo;
        private readonly ICustomerRepo customerRepo;

        public ShoppingCartRepo(StoreContext context, IProductRepo productRepo, ICustomerRepo customerRepo) : base(context)
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

        public override int Add(ShoppingCartRecord entity, bool persist = true)
        {
            var product = this.productRepo.FindAsNoTracking(entity.ProductId);
            if (product == null)
            {
                throw new SpyStoreInvalidProductException(
                "Unable to locate the product");
            }
            return Add(entity, product, persist);
        }
        public int Add(ShoppingCartRecord entity, Product product, bool persist = true)
        {
            var item = GetBy(entity.ProductId);
            if (item == null)
            {
                if (entity.Quantity > product.UnitsInStock)
                {
                    throw new SpyStoreInvalidQuantityException(
                    "Can't add more product than available in stock");
                }
                entity.LineItemTotal = entity.Quantity * product.CurrentPrice;
                return base.Add(entity, persist);
            }
            item.Quantity += entity.Quantity;
            return item.Quantity <= 0
            ? Delete(item, persist)
            : Update(item, product, persist);
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
            => Table.FirstOrDefault(x => x.ProductId == productId);


        public CartRecordWithProductInfo GetShoppingCartRecord(int id) => Context.CartRecordWithProductInfos.FirstOrDefault
            (x => x.Id == id);


        public IEnumerable<CartRecordWithProductInfo> GetShoppingCartRecords(int customerId)
            => Context.CartRecordWithProductInfos.Where(x => x.CustomerId == customerId).OrderBy(x => x.ModelName);


        public CartWithCustomerInfo GetShoppingCartRecordsWithCustomer(int customerId)
       => new CartWithCustomerInfo()
       {
           CartRecords = GetShoppingCartRecords(customerId).ToList(),
           Customer = this.customerRepo.Find(customerId)
       };

        public int Purchase(int customerId)
        {
            var customerIdParam = new SqlParameter("@customerId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = customerId
            };
            var orderIdParam = new SqlParameter("@orderId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            try
            {
                Context.Database.ExecuteSqlRaw(
                "EXEC [Store].[PurchaseItemsInCart] @customerId, @orderid out",
                customerIdParam, orderIdParam);
            }
            catch (Exception ex)
            {
                return -1;
            }
            return (int)orderIdParam.Value;
        }


        public override int Update(ShoppingCartRecord entity, bool persist = true)
        {
            var product = this.productRepo.FindAsNoTracking(entity.ProductId);
            if (product == null)
            {
                throw new SpyStoreInvalidProductException("Unable to locate product");
            }
            return Update(entity, product, persist);
        }
        public int Update(ShoppingCartRecord entity, Product product, bool persist = true)
        {
            if (entity.Quantity <= 0)
            {
                return Delete(entity, persist);
            }
            if (entity.Quantity > product.UnitsInStock) {
                throw new SpyStoreInvalidQuantityException("Can't add more products than availible in stock");

            }
            var dbRecord = Find(entity.Id);
            if (entity.TimeStamp != null && dbRecord.TimeStamp.SequenceEqual(entity.TimeStamp))
            {
                dbRecord.Quantity = entity.Quantity;
                dbRecord.LineItemTotal = entity.Quantity * product.CurrentPrice;
                return base.Update(dbRecord, persist);
            }
            throw new SpyStoreConcurrencyException("Record was changed since it was loaded");

        }
        public override int UpdateRange(IEnumerable<ShoppingCartRecord> entities, bool persist = true)
        {
            int counter = 0;
            foreach (var item in entities)
            {
                var product = this.productRepo.FindAsNoTracking(item.ProductId);
                counter += Update(item, product, false);
            }
            return persist ? SaveChanges() : counter;

        }
        public override int AddRange(IEnumerable<ShoppingCartRecord> entities, bool persist = true)
        {
            int counter = 0;
            foreach (var item in entities)
            { var product = this.productRepo.FindAsNoTracking(item.ProductId);
                counter += Add(item, product, false);
            }
            return persist ? SaveChanges() : counter;

        }


    } }
