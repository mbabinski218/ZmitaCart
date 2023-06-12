using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using ZmitaCart.API.Common;
using ZmitaCart.API.Hubs;
using ZmitaCart.API.Services;
using ZmitaCart.Application;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;
using ZmitaCart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("auth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        Name = "Authorization",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, 
                    Id = "auth"
                }
            },
            Array.Empty<string>()
        }
    });
    options.DescribeAllParametersInCamelCase();
});
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSignalR(options => options.EnableDetailedErrors = true);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddCors(options => options.AddPolicy("corsapp", corsBuilder =>
    corsBuilder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

if (builder.Environment.IsDevelopment())
{
   //builder.Services.AddScoped<ICurrentUserService, FakeCurrentUserService>();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
await seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors("corsapp");
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapHub<ChatHub>("/ChatHub");

app.Run();
