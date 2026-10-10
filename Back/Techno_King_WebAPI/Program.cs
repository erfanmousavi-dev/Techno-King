using App.Domain.Core.Techno_King.App.Domain.Core;
using App.Domain.Core.Techno_King.Config;
using App.Domain.Core.Techno_King.Data.Repositories;
using App.Domain.Core.Techno_King.Entities.Users;
using App.Domain.Core.Techno_King.Enum;
using App.Domain.Core.Techno_King.Service;
using App.Infra.Data.Repos.Ef.Techno_King;
using Connection.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Reflection;
using System.Text;
using Techno_KingAppService.Techno_King.Categories;
using Techno_KingAppService.Techno_King.Products;
using Techno_KingAppService.Techno_King.Users;
using Techno_KingService.Techno_King.Categories;
using Techno_KingService.Techno_King.Products;
using Techno_KingService.Techno_King.Techno_GeneralService;
using Techno_KingService.Techno_King.Users;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// --- Load SiteSettings from the built-in configuration ---
var siteSettings = builder.Configuration.GetSection("SiteSettings").Get<Sitesettings>();
builder.Services.AddSingleton(siteSettings);

// --- Configure Serilog using the loaded SiteSettings ---
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console();
    config.WriteTo.Seq(siteSettings.SeqConfigurations.UrlAddress, apiKey: siteSettings.SeqConfigurations.ApiToken);
});

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(siteSettings.SqlConfigurations.ConnectionString,
        x => x.MigrationsAssembly("Connection")));

builder.Services.AddIdentity<UserBase, IdentityRole<int>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole<int>>()
.AddEntityFrameworkStores<AppDbContext>();

// --- JWT Authentication Configuration ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductAppService, ProductAppService>();
builder.Services.AddScoped<IGeneralService, GeneralService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<ICategoriesRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryAppService, CategoryAppService>();
builder.Services.AddScoped<IProductQueryOrchestrationService, ProductQueryOrchestrationService>();
builder.Services.AddScoped<IProductQueryOrchestrationAppService, ProductQueryOrchestrationAppService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- Swagger Configuration with JWT Security Definition ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Techno_King API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// --- CORS Configuration ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Techno_King API v1");
        c.RoutePrefix = string.Empty;
    });
}

// app.UseHttpsRedirection();

app.UseAuthentication(); // Registered before UseAuthorization
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        Log.Information("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while migrating the database.");
    }
}

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred while starting the application");
    if (ex is ReflectionTypeLoadException reflEx)
    {
        foreach (var loaderEx in reflEx.LoaderExceptions)
        {
            Log.Error(loaderEx, "Loader Exception: {Message}", loaderEx.Message);
        }
    }
}
finally
{
    Log.CloseAndFlush();
}