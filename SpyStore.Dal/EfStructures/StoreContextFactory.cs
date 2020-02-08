using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace SpyStore.Dal.EfStructures
{
    public class StoreContextFactory : IDesignTimeDbContextFactory<StoreContext>
    {
        public StoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
            var connectionString = @"Server=.,5433;Database=SpyStore21; User ID=sa;Password=P@ssw0rd;MultipleActiveResultSets=true;";
            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            optionsBuilder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            Console.WriteLine(connectionString);
            return new StoreContext(optionsBuilder.Options);
        }
    }
}

