using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using GrowingStrongAPI.DataAccess;
using GrowingStrongAPI.Services;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI
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
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFoodEntryService, FoodEntryService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFoodEntryRepository, FoodEntryRepository>();
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IAuthenticationHelper, AuthenticationHelper>();
            services.AddSingleton<IJwtHelper, JwtHelper>();

            var JWTSecret = Configuration["JWTSecret"];
            var key = Encoding.ASCII.GetBytes(JWTSecret);
            ConfigurationsHelper.JWTSecret = JWTSecret;

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
               x.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                       var userId = int.Parse(context.Principal.Identity.Name);
                       var response = userService.GetUserById(userId);

                       if (response.ResponseStatus.Status.Equals(ResponseStatusCode.NOT_FOUND))
                       {
                           //Unauthorized
                           context.Fail("Unauthorized");
                       }

                       return Task.CompletedTask;
                   }
               };

               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };

           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                ConfigurationsHelper.ConnectionString = Configuration["ConnectionStringDevelopment"];
            }

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            loggerFactory.AddFile("Logs/GrowingStrongAPI--{Date}.txt");
            app.UseHttpsRedirection();

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
