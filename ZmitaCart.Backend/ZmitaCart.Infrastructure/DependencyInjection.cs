﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Common;
using ZmitaCart.Infrastructure.Common.Settings;
using ZmitaCart.Infrastructure.Persistence;
using ZmitaCart.Infrastructure.Persistence.Interceptors;
using ZmitaCart.Infrastructure.Repositories;

namespace ZmitaCart.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string applicationDbName)
	{
		AddDatabase(services, configuration, applicationDbName);
		AddAuthentication(services, configuration);
		AddRepositories(services);
		services.AddAuthorization();

		return services;
	}

	private static IServiceCollection AddRepositories(IServiceCollection services)
	{
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();
		services.AddScoped<IOfferRepository, OfferRepository>();
		services.AddScoped<IPictureRepository, PictureRepository>();
		services.AddScoped<IConversationRepository, ConversationRepository>();
		services.AddScoped<GoogleAuthentication>();

		return services;
	}

	private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, string applicationDbName)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString(applicationDbName)));

		services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

		services.AddIdentity<User, IdentityRole<int>>(options =>
			{
				//TODO Remove password requirements
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 1;
				options.Password.RequiredUniqueChars = 0;
			})
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		services.ConfigureApplicationCookie(options =>
		{
			options.Cookie.Name = "auth_cookie";
			options.Events.OnRedirectToLogin = context =>
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				context.RedirectUri = "";
				return Task.CompletedTask;
			};
		});

		services.AddScoped<PublishDomainEventsInterceptor>();

		return services;
	}

	private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var jwtSettings = new JwtSettings();
		configuration.Bind(JwtSettings.sectionName, jwtSettings);
		services.AddSingleton(Options.Create(jwtSettings));

		var googleSettings = new GoogleSettings();
		configuration.Bind(GoogleSettings.sectionName, googleSettings);
		services.AddSingleton(Options.Create(googleSettings));

		services.AddSingleton<JwtHelper>();

		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
				};
			})
			.AddGoogle(options =>
			{
				options.ClientId = googleSettings.ClientId;
				options.ClientSecret = googleSettings.ClientSecret;
			});

		return services;
	}
}