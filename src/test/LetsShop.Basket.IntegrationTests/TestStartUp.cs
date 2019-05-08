using System.Reflection;
using AutoMapper;
using LetsShop.Basket.Domain.Entities;
using LetsShop.Basket.Domain.Repositories;
using LetsShop.Basket.Services;
using LetsShop.Basket.WebApi.Controllers;
using LetsShop.Basket.WebApi.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LetsShop.Basket.Domain.IntegrationTests
{
    public class TestStartUp
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // workaround for now:
            // https://stackoverflow.com/questions/43669633/why-is-testserver-not-able-to-find-controllers-when-controller-is-in-separate-as
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddApplicationPart(Assembly.Load(new AssemblyName("LetsShop.Basket.WebApi"))); ;
            
            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new MapperProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddTransient<ICartService, CartService>();
            services.AddSingleton<IRepository<Cart>>(new InMemoryCartRepository());
            services.AddSingleton(mapper);
            services.AddTransient<CartController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
            app.UseMvc();
        }
    }
}