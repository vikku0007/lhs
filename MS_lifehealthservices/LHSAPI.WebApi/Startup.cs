
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using LHSAPI.Application.Account.Commands.Create.SignUp;
using LHSAPI.Domain.Entities;
using LHSAPI.Infrastucture.Extensions;
using LHSAPI.Persistence.DbContext;
using LHSAPI.WebApi.Controllers.Account;
using System;
using System.Reflection;
using static LHSAPI.ApiKeyValidation.ApiKeyHandler;
using System.Security.Claims;
using LHSAPI.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using LHSAPI.Application.Services;
using LHSAPI.Application.SchedulerService;

namespace LHSAPI
{
    public class Startup
    {

        //readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }
        private readonly SymmetricSecurityKey _signingKey;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtIssuerOptions:JwtKey"]));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.Configure<JwtIssuerOptions>(Configuration.GetSection("JwtIssuerOptions"));
            NativeInjectorServices.RegisterServices(services);
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(1);
            });
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            //services.Configure<JwtIssuerOptions>(options =>
            //{
            //  options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            //  options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
            //  options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);

            //});
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(cfg =>
           {
               cfg.RequireHttpsMetadata = false;
               cfg.SaveToken = true;
               cfg.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Configuration["JwtIssuerOptions:Issuer"],
                   ValidAudience = Configuration["JwtIssuerOptions:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtIssuerOptions:JwtKey"])),
                   RequireExpirationTime = true,
                   ClockSkew = TimeSpan.FromMinutes(0)
               };
               cfg.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
               {
                   if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                   {
                       context.Response.Headers.Add("Token-Expired", "true");
                   }
                   return Task.CompletedTask;
               }
               };
           });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("RequireAdmin", policy => { policy.RequireRole("SuperAdmin"); policy.AddAuthenticationSchemes("Bearer"); policy.RequireAuthenticatedUser(); });
                auth.AddPolicy("RequireAdminOrAccountOfficer", policy => { policy.RequireRole("SuperAdmin", "AccountsOfficer"); policy.AddAuthenticationSchemes("Bearer"); policy.RequireAuthenticatedUser(); });
                // auth.AddPolicy("AllRoles", policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin", "EndUser"));
                auth.AddPolicy("RequireEmployee", policy =>
                      {
                          policy.RequireRole("Employee", "ComplianceOfficer",
      "CareCoordinator",
      //"AccountsOfficer",
      "IncidentReportingOfficer",
      "HouseTeamLeader",
      "HumanResourcesOfficer",
      "DisabilitySupportWorker",
      "MentalHealthWorkerOrDisabilitySupportWorker",
      "GeneralManager",
      "OperationsManager",
      "ChiefExecutiveOfficer"); policy.AddAuthenticationSchemes("Bearer"); policy.RequireAuthenticatedUser();
                      });
                auth.AddPolicy("RequireClient", policy => { policy.RequireRole("Client"); policy.AddAuthenticationSchemes("Bearer"); policy.RequireAuthenticatedUser(); });
                auth.AddPolicy("RequireAdminOrEmployee", policy =>
                {
                    policy.RequireRole("Employee", "SuperAdmin",
"ComplianceOfficer",
"CareCoordinator",
"AccountsOfficer",
"IncidentReportingOfficer",
"HouseTeamLeader",
"HumanResourcesOfficer",
"DisabilitySupportWorker",
"MentalHealthWorkerOrDisabilitySupportWorker",
"GeneralManager", "OperationsManager", "ChiefExecutiveOfficer"
); policy.AddAuthenticationSchemes("Bearer"); policy.RequireAuthenticatedUser();
                });
                auth.AddPolicy("RequireClientOrEmployee", policy => { policy.RequireRole("Employee", "Client"); policy.AddAuthenticationSchemes("Bearer"); policy.RequireAuthenticatedUser(); });
            });
            //services.AddAuthorization(authConfig =>
            //      {
            //          authConfig.AddPolicy("ApiKeyPolicy",
            //              policyBuilder => policyBuilder
            //                  .AddRequirements(new ApiKeyRequirement()));
            //        //authConfig.AddPolicy("RequireAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin"));
            //        //authConfig.AddPolicy("AllRoles", policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin", "EndUser"));
            //        //authConfig.AddPolicy("RequireEndUser", policy => policy.RequireClaim(ClaimTypes.Role, "EndUser"));
            //      });

            services.AddCors();



            // swagger configuration
            //services.AddSwaggerDocumentation();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LHS API", Version = "v1", Description = "LHS API" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
            });


            // Add DbContext using SQL Server Provider
            services.AddDbContext<LHSDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
            // Add MediatR
            services.AddMediatR(typeof(SignupCommandHandler).GetTypeInfo().Assembly);
            // ===== Add Identity ========
            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<LHSDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            //services.AddMvc(option => option.EnableEndpointRouting = false);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());


            // swagger configuration
            app.UseSwaggerDocumentation();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestService");
            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                  {
                      endpoints.MapControllers();
                  });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ProfileImages")),
                RequestPath = new PathString("/ProfileImages")
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ProfileImages")),
                RequestPath = new PathString("/ProfileImages")
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/ClientDocuments/")),
                RequestPath = new PathString("/ClientDocuments")
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ClientDocuments")),
                RequestPath = new PathString("/ClientDocuments")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/EmployeeDocuments/")),
                RequestPath = new PathString("/EmployeeDocuments")
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\EmployeeDocuments")),
                RequestPath = new PathString("/EmployeeDocuments")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
          Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/ClientProfileImages/")),
                RequestPath = new PathString("/ClientProfileImages")
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
              Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ClientProfileImages")),
                RequestPath = new PathString("/ClientProfileImages")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/EmailTemplate/")),
                RequestPath = new PathString("/EmailTemplate")
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
              Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\EmailTemplate")),
                RequestPath = new PathString("/EmailTemplate")
            });
        }
    }
}
