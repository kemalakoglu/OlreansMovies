using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Movies.Server.Extensions;

public static class SwaggerExtension
{
	public static void ConfigureSwagger(this IServiceCollection services) =>
		// Register the Swagger generator, defining one or more Swagger documents
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("OrleansMovie",
				new OpenApiInfo
				{
					Title = "Orleans Movie API",
					Version = "1.0",
					Description = "Orleans Movie Web API Documentation"
				});
			c.AddSecurityDefinition("Bearer",
				new OpenApiSecurityScheme
				{
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					Description =
						"Input your Bearer token in this format - Bearer {your token here} to access this API"
				});

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"},
						Scheme = "Bearer",
						Name = "Bearer",
						In = ParameterLocation.Header
					},
					new List<string>()
				}
			});
		});
}