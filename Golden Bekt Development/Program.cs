
using FX.Services.Bunny;
using Golden_Bekt_Development.Models.Context;
using Golden_Bekt_Development.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

namespace Golden_Bekt_Development
{
    public class Program
    {
        // Pseudocode plan:
        // 1. The error indicates a version mismatch between AutoMapper and its extensions.
        // 2. Ensure the AutoMapper.Extensions.Microsoft.DependencyInjection NuGet package is installed and up-to-date.
        // 3. Remove any direct usage of AutoMapper.MapperConfiguration if present.
        // 4. Use AddAutoMapper with explicit assembly references if needed.
        // 5. Clean and rebuild the solution after updating packages.

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<GoldenDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("local")));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]!))
        };
    });


            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(GenericService<>));
            builder.Services.AddScoped<IBunyimagesServices, BunyimagesServices>();
            builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();
            // Program.cs / Startup.cs
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors("AllowAll");
            // Configure the HTTP request pipeline.
            app.UseSwagger(opt =>
            {
                opt.RouteTemplate = "openapi/{documentName}.json";
            });
            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Scalar Example";
                opt.Theme = ScalarTheme.Mars;
                opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
            });
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
