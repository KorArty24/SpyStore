using System;
using System.IO;
using System.Reflection;
using SpyStore.Dal.Initialization;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.Repos;
using SpyStore.Dal.Repos.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpyStore.Dal.EfStructures;
using Microsoft.OpenApi.Models;
using SpyStore.Service.Filters;

namespace SpyStore.Service
{

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
            var connection = @"Server=db;Database=SpyStore21;User=sa;Password=P@ssw0rd;";

            services.AddControllersWithViews(config => config.Filters.Add(new SpyStoreExceptionFilter(_env)))
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = new MyTransparentJsonNamingPolicy();
                });


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
                            serviceScope.ServiceProvider.GetRequiredService<StoreContext>();
                        SampleDataInitializer.InitializeData(context);

                    }
                }
            app.UseSwagger(c => {
                c.RouteTemplate = "SpyStore Service v1/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/SpyStore Service v1/swagger/v1/swagger.json", "SpyStore Service v1");
                });
                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseStaticFiles();
                app.UseCors("AllowAll");      
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        
    } 
} 
