using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using System.IO;
using System.Reflection;
using SpyStore.Dal;
using SpyStore.Dal.Initialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.Repos;
using SpyStore.Dal.Repos.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using SpyStore.Dal.EfStructures;
using Swashbuckle.Swagger;
using Microsoft.OpenApi.Models;
using SpyStore.Service.Filters;

namespace SpyStore.Service { 

    public class Startup
    {
        public readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(config => config.Filters.Add(new SpyStoreExceptionFilter(_env)))
                .AddNewtonsoftJson(options => 
                options.SerializerSettings.ContractResolver =
                  new CamelCasePropertyNamesContractResolver()).AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);
            
            // http://docs.asp.net/en/latest/security/cors.html
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                 //   .AllowCredentials();
                });
            });
            services.AddDbContextPool<StoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SpyStore")));
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
            //services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "SpyStore Service",
                    Version = "v1",
                    Description = "Service to support the SpyStore sample eCommercesite",
                    TermsOfService = new Uri("https://example.com/terms"),
                    License = new OpenApiLicense
                    {
                        Name = "Freeware",
                        //Url= "https://en.wikipedia.org/wiki/Freeware"
                        Url = new Uri ("http://localhost:23741/LICENSE.txt")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }




            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    using (var serviceScope =
                        app.ApplicationServices
                        .GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        var context =
                            serviceScope.ServiceProvider.GetRequiredService<Dal.EfStructures.StoreContext>();
                        SampleDataInitializer.InitializeData(context);

                    }
                }
            app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpyStore Service v1");
                });
                app.UseStaticFiles();
                app.UseCors("AllowAll");
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        
    } 
} 
