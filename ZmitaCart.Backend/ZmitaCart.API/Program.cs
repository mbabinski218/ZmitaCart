using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using ZmitaCart.API.Common;
using ZmitaCart.API.Hubs;
using ZmitaCart.API.Services;
using ZmitaCart.Application;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;
using ZmitaCart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var settings = new GlobalSettings();
builder.Configuration.Bind(GlobalSettings.sectionName, settings);

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
builder.Services.AddHealthChecks()
	.AddSqlServerCheck(builder.Configuration.GetConnectionString(settings.ApplicationDbName), "Database")
	.AddSignalRHubCheck(settings.Url + settings.ChatHubUrl, "ChatHub");
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSignalR(options => options.EnableDetailedErrors = true);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, settings.ApplicationDbName);
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddCors(options => options.AddPolicy("corsapp", corsBuilder =>
	corsBuilder.WithOrigins(settings.Origin).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

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
app.MapHealthChecks(settings.HealthCheckUrl, new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.UseAuthorization();
app.MapControllers();

app.MapHub<ChatHub>(settings.ChatHubUrl);

app.Run();