using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudOfficeOnlineServer.Middlewares;
using StudOfficeOnlineServer.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.WithOrigins(builder.Configuration["Cors:Origin"]!).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});
string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<DBContext>(options => options.UseNpgsql(connection));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Configuration"];
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["AuthOptions:ISSUER"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["AuthOptions:AUDIENCE"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthOptions:KEY"]!))
    };
});

var app = builder.Build();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseMiddleware<JWTCheckMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
