using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsCRUD.Application.MapperProfiles;
using ProductsCRUD.Application.S_AuthenticationService;
using ProductsCRUD.Application.S_CipherService;
using ProductsCRUD.Application.S_ProductService.Read;
using ProductsCRUD.Application.S_ProductService.Write;
using ProductsCRUD.Application.S_PromotionService.Read;
using ProductsCRUD.Application.S_PromotionService.Write;
using ProductsCRUD.Application.S_PromotionTypeService.Read;
using ProductsCRUD.Data.EntityFrameworkCore.Context;
using ProductsCRUD.Data.EntityFrameworkCore.Repositories._core;
using ProductsCRUD.Domain._core;
using ProductsCRUD.WebApi.MapperProfiles;
using ProductsCRUD.WebApi.Settings;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtToken:Issuer"].ToString(),
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtToken:Audience"].ToString(),
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtToken:SigningKey"].ToString())),
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetSection("JwtToken"));


// =========== Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// =========== Add mapper
builder.Services.AddAutoMapper(typeof(PresentationAuthenticationProfile), typeof(ProductProfile));


// =========== Add UnitOfWork and services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICipherService, CipherService>();
builder.Services.AddScoped<IProductWriteService, ProductWriteService>();
builder.Services.AddScoped<IProductReadService, ProductReadService>();
builder.Services.AddScoped<IPromotionWriteService, PromotionWriteService>();
builder.Services.AddScoped<IPromotionReadService, PromotionReadService>();
builder.Services.AddScoped<IPromotionTypeReadService, PromotionTypeReadService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await dbContext.Database.MigrateAsync();

app.Run();

