using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VirocGanpati.Data;
using VirocGanpati.Mapping;
using VirocGanpati.Models;
using VirocGanpati.Repositories;
using VirocGanpati.Services;

namespace VirocGanpati.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AWSOptions>(configuration.GetSection("AWS"));
        services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<AWSOptions>>().Value);
        return services;
    }

    public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null)
            )
        );
        return services;
    }

    public static IServiceCollection RegisterJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        services.AddAuthentication(options =>
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
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
            options.AddPolicy("ManagerViewPolicy", policy => policy.RequireRole("Manager", "ManagerView"));
        });

        return services;
    }

    public static IServiceCollection RegisterSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "VIROC Ganpati API",
                Version = "v1",
                Description = "API for Managing VIROC Ganpati Competition",
                Contact = new OpenApiContact
                {
                    Name = "Prit Thakkar",
                    Email = "pritthakkar111101@gmail.com"
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
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

        return services;
    }

    public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        return services;
    }

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMandalRepository, MandalRepository>();
        services.AddScoped<IMandalService, MandalService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRecordRepository, RecordRepository>();
        services.AddScoped<IRecordService, RecordService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IS3Service, S3Service>();
        services.AddScoped<IMandalAreaRepository, MandalAreaRepository>();
        services.AddScoped<IMandalAreaService, MandalAreaService>();
        services.AddScoped<IArtiMorningTimeRepository, ArtiMorningTimeRepository>();
        services.AddScoped<IArtiMorningTimeService, ArtiMorningTimeService>();
        services.AddScoped<IArtiEveningTimeRepository, ArtiEveningTimeRepository>();
        services.AddScoped<IArtiEveningTimeService, ArtiEveningTimeService>();
        services.AddScoped<IDateOfVisarjanRepository, DateOfVisarjanRepository>();
        services.AddScoped<IDateOfVisarjanService, DateOfVisarjanService>();
        services.AddScoped<IOtpRepository, OtpRepository>();
        services.AddScoped<IOtpService, OtpService>();
        return services;
    }
}
