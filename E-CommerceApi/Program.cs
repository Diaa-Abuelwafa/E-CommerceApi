
using E_CommerceApi.HandlingErrors;
using E_CommerceApi.Middlewares;
using E_CommerceDomain.Entities.Account_Module;
using E_CommerceDomain.Interfaces;
using E_CommerceDomain.Interfaces.Account_Module;
using E_CommerceDomain.Interfaces.Basket_Module;
using E_CommerceDomain.Interfaces.Caching_Service;
using E_CommerceDomain.Interfaces.Product_Module;
using E_CommerceRepository.Data.Contexts;
using E_CommerceRepository.Data.Helper;
using E_CommerceRepository.Repositories;
using E_CommerceRepository.Repositories.Basket_Module;
using E_CommerceRepository.Repositories.Caching_Service;
using E_CommerceService.Services.Account_Module;
using E_CommerceService.Services.Basket_Module;
using E_CommerceService.Services.Product_Module;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Net;
using System.Text;

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

            // Add Identity Services To The DI Container

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AccountDbContext>();

            // Add AppDbContext Service The DI Container
            builder.Services.AddDbContext<AppDbContext>(O =>
            {
                O.UseSqlServer(builder.Configuration.GetConnectionString("Cs"));
            });

            // Add AccountDbContext To The DI Container
            builder.Services.AddDbContext<AccountDbContext>(O =>
            {
                O.UseSqlServer(builder.Configuration.GetConnectionString("Identity"));
            });

            // Add IConnectionMultiplexer To The DI Container
            builder.Services.AddSingleton<IConnectionMultiplexer>(O =>
            {
                var Connection = builder.Configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(Connection);
            });

            // Configurations For Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Custom Services To The DI Container
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            builder.Services.AddScoped<IBasketServices, BasketService>();

            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddSingleton<ICachedService, CachedService>();

            // Configure Authentication Middleware To Work JWT Base
            builder.Services.AddAuthentication(O =>
            {
                // For Verify Authentication Jwt Base
                O.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // For When The Request Come From Un Authenticated Consumer Return UnAuthorize Response
                O.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(O =>
            {
                // Check In The Token On The Expire Time
                O.SaveToken = true;

                // The Request Https Protocol
                O.RequireHttpsMetadata = true;

                // Check On Claims Matched The App Values Or Not 
                O.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:ProviderUrl"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:ConsumerUrl"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };
            });

            var app = builder.Build();

            // To Handle Server Error
            app.UseMiddleware<ExceptionMiddleware>();

            // Update The Database(AppDbContext)
            using(var Scope = app.Services.CreateScope())
            {
                var Context = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var AccountContext = Scope.ServiceProvider.GetRequiredService<AccountDbContext>();

                try
                {
                    // Update AppDbContext Database
                    await Context.Database.MigrateAsync();

                    await AccountContext.Database.MigrateAsync();

                    // Add Seed Data To The Two Databases
                    SeedData.Seed(Context, AccountContext);
                }
                catch(Exception Ex)
                {
                    // Loging In Db
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();

            app.UseAuthorization();

            // MiddleWare To Handle The Not Found EndPoint
            app.UseStatusCodePagesWithReExecute("/api/Errors");

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
