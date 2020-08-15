using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortalRandkowy.API.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using PortalRandkowy.API.Helpers;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PortalRandkowy.API
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
            services.AddDbContext<DataContext>(x => x.UseMySql("Server=localhost; Database=PortalRandkowy; Uid=appuser;Pwd=kcmw96312").EnableDetailedErrors());
            services.AddMvc().AddMvcOptions(opt =>
                {
                    opt.EnableEndpointRouting = false;
                })
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            services.AddCors();
            services.AddAutoMapper(typeof(Startup)); // dodanie automappera
            services.AddTransient<Seed>(); // dodanie testowych wartosci do db
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IGenericRepository, GenericRepository>(); // rejestrowanie wstrzykiwania jezeli uzyjemy interfejsu to z automatu wstrzykanie nam klase jakas tam ktora jest podana
            services.AddScoped<IUserRepository, UserRepository>();
            services.Configure<ClaudinarySettings>(Configuration.GetSection("ClaudinarySettings"));  // dodajemy konfig dla claoudinary settings dla sekscji w appsetings ClaudinarySett...
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                        .AddJwtBearer(options =>
                        {
                            options.RequireHttpsMetadata = false;
                            options.SaveToken = true;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                ValidateLifetime = true
                            };
                        })
                        .AddNegotiate();
            services.AddScoped<LogUserActivity>(); // dodajemy to aby on zapisywal nam ostatnia aktywnosc
        }

        // public void ConfigureDevelopmentServices(IServiceCollection services)       // configuracja dla developmentu
        // {
        //     services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
        //     services.AddMvc().AddMvcOptions(opt =>
        //         {
        //             opt.EnableEndpointRouting = false;
        //         })
        //         .AddNewtonsoftJson(opt =>
        //         {
        //             opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        //         });
        //     services.AddCors();
        //     services.AddAutoMapper(typeof(Startup)); // dodanie automappera
        //     services.AddTransient<Seed>(); // dodanie testowych wartosci do db
        //     services.AddScoped<IAuthRepository, AuthRepository>();
        //     services.AddScoped<IGenericRepository, GenericRepository>(); // rejestrowanie wstrzykiwania jezeli uzyjemy interfejsu to z automatu wstrzykanie nam klase jakas tam ktora jest podana
        //     services.AddScoped<IUserRepository, UserRepository>();
        //     services.Configure<ClaudinarySettings>(Configuration.GetSection("ClaudinarySettings"));  // dodajemy konfig dla claoudinary settings dla sekscji w appsetings ClaudinarySett...
        //     services.AddAuthentication(x =>
        //     {
        //         x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //         x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //     })
        //                 .AddJwtBearer(options =>
        //                 {
        //                     options.RequireHttpsMetadata = false;
        //                     options.SaveToken = true;
        //                     options.TokenValidationParameters = new TokenValidationParameters
        //                     {
        //                         ValidateIssuerSigningKey = true,
        //                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
        //                         ValidateIssuer = false,
        //                         ValidateAudience = false,
        //                         ValidateLifetime = true
        //                     };
        //                 })
        //                 .AddNegotiate();
        //     services.AddScoped<LogUserActivity>(); // dodajemy to aby on zapisywal nam ostatnia aktywnosc
        // }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())        // tryb developerski
            {
                app.UseDeveloperExceptionPage();
            }
            else        // trub produkcyjny
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;      // wygenerowanie statusu z kodu na cyfre

                        var error = context.Features.Get<IExceptionHandlerFeature>();       // pobieramy z kontextu jakie sa bledy

                        if (error != null)       // jesli sa bledy to podajemy je
                        {
                            context.Response.AddAplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                }); // globalna obsluga bledow dla produkcji
            }

            seeder.SeedUsers();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseAuthorization();
            
            
           
            
            app.UseStatusCodePages();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(name:"spa", pattern: "{controller=Fallback}/{action=Index}");
                endpoints.MapFallbackToController("Index", "Fallback");
            });


            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

           
            app.UseMvc();


        }
    }
}
