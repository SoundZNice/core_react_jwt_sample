using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core3.Application.Commands.Notes;
using Core3.Application.Infrastructure;
using Core3.Application.Infrastructure.AutoMapper;
using Core3.Application.Interfaces;
using Core3.Application.Interfaces.Services;
using Core3.Application.Models.ApiSettings;
using Core3.Application.Models.Token;
using Core3.Persistence;
using Core3.Services.Services;
using Core3.WebUI.Filters;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Core3.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(configuration["Logging:SerilogPath"], rollingInterval:RollingInterval.Hour)
                .CreateLogger();
        }
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).GetTypeInfo().Assembly);

            services.AddMediatR(typeof(CreateNoteCommand).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddDbContext<ICore3DbContext, Core3DbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Core3DB")));

            services.AddScoped<ITokenFactoryService, TokenFactoryService>();
            services.AddScoped<ITokenStoreService, TokenStoreService>();
            services.AddScoped<ITokenValidatorService, TokenValidatorService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<ISecurityService, SecurityService>();

            services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateNoteCommandValidator>());

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddOptions<BearerTokenOptions>()
                .Bind(Configuration.GetSection("BearerTokens"))
                .Validate(
                    bearerTokens =>
                        bearerTokens.AccessTokenExpirationMinutes < bearerTokens.RefreshTokenExpirationMinutes,
                    "RefrechTokenExpirationMinutes is less then AccessTokenExpirationMinutes. Obtaining new tokens using the refresh token should happen only if the access token has expired.");

            services.AddOptions<ApiSettings>()
                .Bind(Configuration.GetSection("ApiSettings"));

            services.AddAuthorization();
            services.AddAuthentication(options =>
                    {
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["BearerTokens:Issuer"],
                        ValidateIssuer = false, // TODO: change
                        ValidAudience = Configuration["BearerTokens:Audience"],
                        ValidateAudience = false, // TODO: change
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"])),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = ctx =>
                        {
                            ILogger logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                                .CreateLogger(nameof(JwtBearerEvents));
                            logger.LogError("Authentication failed.", ctx.Exception);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = ctx =>
                        {
                            ITokenValidatorService tokenValidatorService =
                                ctx.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                            return tokenValidatorService.ValidateAsync(ctx);
                        },
                        OnMessageReceived = ctx => Task.CompletedTask,
                        OnChallenge = ctx =>
                        {
                            ILogger logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                                .CreateLogger(nameof(JwtBearerEvents));
                            logger.LogError("OnChallenge error", ctx.Error, ctx.ErrorDescription);
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddAntiforgery(x => x.HeaderName = "X-XSRF-TOKEN");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer("start");
                }
            });
        }
    }
}
