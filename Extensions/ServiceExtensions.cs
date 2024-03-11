using _AbsoPickUp.Data;
using _AbsoPickUp.Helpers;
using _AbsoPickUp.IServices;
using _AbsoPickUp.LoggerService;
using _AbsoPickUp.Models;
using _AbsoPickUp.Repositories;
using _AbsoPickUp.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _AbsoPickUp.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "_AbsoPickUp", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, new[] { "Bearer" });
                c.AddSecurityRequirement(securityRequirement);
            });
        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureDatabaseSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connection = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
        }
        public static void ConfigureJWTAuthentication(this IServiceCollection services, IConfiguration config)
        {
            IdentityModelEventSource.ShowPII = true;
            var key = Encoding.UTF8.GetBytes(config["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                        var userId = context.Principal.Claims.FirstOrDefault(c => c.Type == "UserId").Value.ToString();
                        var deviceToken = context.Principal.Claims.FirstOrDefault(c => c.Type == "DeviceToken").Value.ToString();
                        //var user = userService.GetById(userId);
                        //if (user == null)
                        //{
                        //    context.Fail("Unauthorized");
                        //}

                        //if (user != null)
                        //{
                        //    if (user.DeviceToken != deviceToken)
                        //    {
                        //        context.Fail("Unauthorized");
                        //    }
                        //    else
                        //        return Task.CompletedTask;
                        //}
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
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration config)
        {
            // Get Identity Default Options
            IConfigurationSection identityDefaultOptionsConfigurationSection = config.GetSection("IdentityDefaultOptions");

            services.Configure<IdentityDefaultOptions>(identityDefaultOptionsConfigurationSection);

            var identityDefaultOptions = identityDefaultOptionsConfigurationSection.Get<IdentityDefaultOptions>();

            services.AddIdentityCore<UserDetails>(options =>
            {
                // Password settings
                options.Password.RequireDigit = identityDefaultOptions.PasswordRequireDigit;
                options.Password.RequiredLength = identityDefaultOptions.PasswordRequiredLength;
                options.Password.RequireNonAlphanumeric = identityDefaultOptions.PasswordRequireNonAlphanumeric;
                options.Password.RequireUppercase = identityDefaultOptions.PasswordRequireUppercase;
                options.Password.RequireLowercase = identityDefaultOptions.PasswordRequireLowercase;
                options.Password.RequiredUniqueChars = identityDefaultOptions.PasswordRequiredUniqueChars;
                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identityDefaultOptions.LockoutDefaultLockoutTimeSpanInMinutes);
                options.Lockout.MaxFailedAccessAttempts = identityDefaultOptions.LockoutMaxFailedAccessAttempts;
                options.Lockout.AllowedForNewUsers = identityDefaultOptions.LockoutAllowedForNewUsers;
                // User settings
                options.User.RequireUniqueEmail = identityDefaultOptions.UserRequireUniqueEmail;
                // email confirmation require
                options.SignIn.RequireConfirmedEmail = identityDefaultOptions.SignInRequireConfirmedEmail;
            }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
        public static void ConfigureGeneralConfigurations(this IServiceCollection services, IConfiguration config)
        {
            // Get SendGrid configuration options
            services.Configure<SendGridOptions>(config.GetSection("SendGridOptions"));
            //// Get SMTP configuration options
            services.Configure<SmtpOptions>(config.GetSection("SmtpOptions"));
            //// Get Super Admin Default options
            services.Configure<SuperAdminDefaultOptions>(config.GetSection("SuperAdminDefaultOptions"));

        }
        public static void ConfigureCustomServices(this IServiceCollection services)
        {
            // Add services here.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IFunctionalService, FunctionalService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITwilioManager, TwilioManager>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IDriverService, DriverService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<INotificationService, NotificationService>();
        }
    }
}
