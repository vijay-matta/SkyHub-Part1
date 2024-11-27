using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using System.Text;
using SkyHub.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using SkyHub.Models.Roles;
using Microsoft.AspNetCore.Identity;
using SkyHub.Services;
using System.Text.Json.Serialization;
using System.Text.Json;
using SkyHub.Models.Flight_Details;
using SkyHub.Controllers;
using System.Security.Claims;





namespace SkyHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SkyHubDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure JWT Authentication
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        RoleClaimType = ClaimTypes.Role
                    };
                });
           



            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Add Flight Servies
            builder.Services.AddScoped<IFlightService, FlightService>();

            //builder.Services.AddIdentity<Users, IdentityRole>()
            //         .AddEntityFrameworkStores<SkyHubDbContext>()
            //       .AddDefaultTokenProviders();



            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            var flights = new Flights();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32 // Optional: adjust the maximum depth as needed
            };

            var json = JsonSerializer.Serialize(flights, options);


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register ASP.NET Core Identity
           // builder.Services.AddIdentity<Users, IdentityRole>()
              //  .AddEntityFrameworkStores<SkyHubDbContext>()
              //  .AddDefaultTokenProviders();

            // Other services (e.g., DbContext)
            builder.Services.AddDbContext<SkyHubDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services for DI
            builder.Services.AddScoped<UserProfileController>();

            // Add controllers or other necessary services
            builder.Services.AddControllers();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }


        
    }
}
