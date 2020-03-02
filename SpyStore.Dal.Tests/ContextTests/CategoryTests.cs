using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EfStructures;
using SpyStore.Dal.Initialization;
using SpyStore.Models.Entities;
using Xunit;

namespace SpyStore.Dal.Tests.ContextTests
{
    [Collection("SpyStore.Dal")]
    public class CategoryTests
    {
        private readonly StoreContext _db;

        
        
        private void CleanDatabase()
        {
            SampleDataInitializer.ClearData(_db);
        }
        public CategoryTests()
        {
            _db = new StoreContextFactory().CreateDbContext(new string[0]);
            CleanDatabase();
        }
        public void Dispose()
        {
            CleanDatabase();
            _db.Dispose();
        }

        [Fact]
        public void FirstTest()
        {
            Assert.True(true);
        }
        [Fact]
        public void ShouldAddCategoryWithDbSet() {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);
            Assert.Equal(EntityState.Added, _db.Entry(category).State);
            Assert.True(category.Id == 0);
            Assert.Null(category.TimeStamp);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(category).State);
            Assert.NotNull(category.TimeStamp);
            Assert.Equal(1, _db.Categories.Count());
        }
    }
}
