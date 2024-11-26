
using E_CommerceApi.HandlingErrors;
using E_CommerceApi.Middlewares;
using E_CommerceDomain.Interfaces;
using E_CommerceDomain.Interfaces.Basket_Module;
using E_CommerceDomain.Interfaces.Product_Module;
using E_CommerceRepository.Data.Contexts;
using E_CommerceRepository.Data.Helper;
using E_CommerceRepository.Repositories;
using E_CommerceRepository.Repositories.Basket_Module;
using E_CommerceService.Services.Basket_Module;
using E_CommerceService.Services.Product_Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Net;

namespace E_CommerceApi
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add CORS Configurations
            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", O =>
                {
                    O.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
                });
            });

            // Add services to the container.

            // Add Controller Service To The DI Container And Configure Behavior Of Validation Error

            builder.Services.AddControllers().ConfigureApiBehaviorOptions((O) =>
            {
                // Configurations Of ModelState Validation
                O.InvalidModelStateResponseFactory = (Context) =>
                {

                    // Select The Errors From ModelState
                    var Errors = Context.ModelState.Where(x => x.Value.Errors.Count() > 0)
                                                   .SelectMany(x => x.Value.Errors)
                                                   .Select(x => x.ErrorMessage)
                                                   .ToList();

                    // Make The Custom Response 
                    var Response = new ApiValidationErrorResponse((int)HttpStatusCode.BadRequest, Errors);

                    return new BadRequestObjectResult(Response);
                };
            });

            

            // Add AppDbContext Service Ti The DI Container
            builder.Services.AddDbContext<AppDbContext>(O =>
            {
                O.UseSqlServer(builder.Configuration.GetConnectionString("Cs"));
            });

            // Add IConnectionMultiplexer To The DI Container
            builder.Services.AddSingleton<IConnectionMultiplexer>(O =>
            {
                var Connection = builder.Configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(Connection);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Custom Services To The DI Container
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            builder.Services.AddScoped<IBasketServices, BasketService>();

            var app = builder.Build();

            // To Handle Server Error
            app.UseMiddleware<ExceptionMiddleware>();

            // Update The Database(AppDbContext)
            using(var Scope = app.Services.CreateScope())
            {
                var Context = Scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    // Update Database
                    await Context.Database.MigrateAsync();

                    // Add Seed Data To The Database
                    SeedData.Seed(Context);
                }
                catch(Exception Ex)
                {
                    // Loging In Db
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // CORS Policy Middleware
            app.UseCors("MyPolicy");

            // To Handle NotFound EndPoint
            app.UseStatusCodePagesWithReExecute("/api/Error");

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
