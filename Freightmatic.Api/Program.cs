using Freightmatic.Application.Delivery;
using Freightmatic.Application.Files;
using Freightmatic.Application.News;
using Freightmatic.Application.Notifications;
using Freightmatic.Application.Shared;
using Freightmatic.Application.Users;
using Freightmatic.Domain.Deliveries;
using Freightmatic.Domain.Files;
using Freightmatic.Domain.News;
using Freightmatic.Domain.Notifications;
using Freightmatic.Domain.Users;
using Freightmatic.Infrastructure.Deliveries;
using Freightmatic.Infrastructure.Files;
using Freightmatic.Infrastructure.News;
using Freightmatic.Infrastructure.Notifications;
using Freightmatic.Infrastructure.Shared;
using Freightmatic.Infrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT Auth support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Freightmatic API", Version = "v1" });
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

// JWT Authentication setup
var key = Encoding.ASCII.GetBytes(builder.Configuration["AccessTokenKey"] ?? "zctf!-eip%6!&uf1gswj8oohy3)t7f-04w-yx7&w7n(29k6t");
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
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

// Domain / Identity Services
builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repositories & Application Services
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<INewsRepository, NewsRepository>();
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddTransient<IDeliveryRepository, DeliveryRepository>();

// Register File Repository (Use LocalFileRepository for self-hosted VPS, or FileRepository for Azure Blob Storage)
if (builder.Configuration["FileStorage:UseLocal"]?.ToLower() == "true" || string.IsNullOrEmpty(builder.Configuration.GetConnectionString("BlobStorage")))
{
    builder.Services.AddTransient<IFileRepository, LocalFileRepository>();
}
else
{
    builder.Services.AddTransient<IFileRepository, FileRepository>();
}

builder.Services.AddTransient<UserAppService>();
builder.Services.AddTransient<NewsAppService>();
builder.Services.AddTransient<NotificationAppService>();
builder.Services.AddTransient<DeliveryAppService>();
builder.Services.AddTransient<FileAppService>();

// Register CosmosDbClient if connection string is configured
if (!string.IsNullOrEmpty(builder.Configuration.GetConnectionString("CosmosDB")))
{
    builder.Services.AddSingleton<CosmosDbClient>();
}

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Freightmatic API v1"));
}

app.UseCors("AllowAll");

// Serve uploads folder as static files
string uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
