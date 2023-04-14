using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Samvirk.Identities.Auth;

namespace Samvirk.Loyalty.Api.Extensions;

public static class SwaggerConfigurations
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc(RoutingConstants.Documentation._clientInterface,
                new OpenApiInfo
                { Title = RoutingConstants.Documentation._baseApiName, Version = "v1", Description = "" });
            swagger.SwaggerDoc(RoutingConstants.Documentation._technicalInterface,
                new OpenApiInfo
                { Title = RoutingConstants.Documentation._technicalApiName, Version = "v1", Description = "" });

            swagger.ConfigureWithSamvirkAuthentication();

            swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                "Samvirk.Loyalty.Api.xml"));
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(cfg =>
        {
            cfg.DefaultModelsExpandDepth(-1);
            cfg.DocumentTitle = "Samvirk Loyalty API";
            cfg.SwaggerEndpoint("client-interface/swagger.json",
                RoutingConstants.Documentation._baseApiName);
            cfg.SwaggerEndpoint("technical-interface/swagger.json",
                RoutingConstants.Documentation._technicalApiName);
        });

        return app;
    }
}