using AutoMapper;
using LetsShop.Basket.Domain.Entities;
using LetsShop.Basket.Domain.Repositories;
using LetsShop.Basket.Services;
using LetsShop.Basket.WebApi.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace LetsShop.Basket.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var info = new Info
            {
                Title = "Shop API",
                Version = "v1",
                Contact = new Contact
                {
                    Name = "Michael Brakspear",
                    Url = "https://github.com/michaelBrakspear/lets-shop",
                    Email = "mjbrakspear@gmail.com"
                }
            };

            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new MapperProfile());
            });

            var mapper = mappingConfig.CreateMapper();

            services.AddSwaggerGen(options => { options.SwaggerDoc("v1", info); });
            services.AddTransient<ICartService, CartService>();
            services.AddSingleton<IRepository<Cart>>(new InMemoryCartRepository());
            services.AddSingleton(mapper);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "LetsShop");
                options.DocExpansion(DocExpansion.List);
            });

            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
            app.UseMvc();
        }
    }
}