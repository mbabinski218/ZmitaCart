using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using ZmitaCart.API.Common;
using ZmitaCart.API.Hubs;
using ZmitaCart.API.Services;
using ZmitaCart.Application;
using ZmitaCart.Application.Hubs;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;
using ZmitaCart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.DescribeAllParametersInCamelCase();
});
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSignalR();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IChatHub, ChatHub>();
builder.Services.AddCors(options => options.AddPolicy("corsapp", corsBuilder =>
    corsBuilder.AllowAnyMethod().AllowAnyHeader().WithOrigins("*")));

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
//app.UseHttpsRedirection();
app.UseCors("corsapp");
app.MapControllers();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/ChatHub");

app.Run();
