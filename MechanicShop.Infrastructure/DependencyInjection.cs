

namespace MechanicShop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration , AppSettings appSettings)
        {
            services.AddSingleton(TimeProvider.System);

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            ArgumentNullException.ThrowIfNull(connectionString);

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            services.AddScoped<ApplicationDbContextInitialiser>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("JwtSettings");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                           Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
                };
            });

            services
            .AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IAuthorizationHandler, LaborAssignedHandler>();

            services.AddAuthorizationBuilder()
                  .AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"))
                  .AddPolicy("SelfScopedWorkOrderAccess", policy =>
                    policy.Requirements.Add(new LaborAssignedRequirement()));

            services.AddTransient<IIdentityService, IdentityService>();
            
            services.AddScoped<IWorkOrderPolicy, WorkOrderPolicy>();

            services.AddScoped<ITokenProvider, TokenProvider>();

            services.AddScoped<IInvoicePdfGenerator, InvoicePdfGenerator>();

            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IWorkOrderNotifier, SignalRWorkOrderNotifier>();

            services.AddScoped<IAppDbContext, AppDbContext>();
            
            services.AddScoped<ApplicationDbContextInitialiser>();

            services.AddHostedService<OverdueBookingCleanupService>();

            return services;
        }

    }
}
