using System;
using Azure.Identity;
using Fabillio.Inventory.API.Controllers;
using Fabillio.Inventory.API.Extensions;
using Fabillio.Inventory.Cqrs.Queries;
using Fabillio.Inventory.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Newtonsoft.Json.Converters;
using Fabillio.Common.Configurations.Extensions;
using Fabillio.Common.Events;
using Fabillio.Common.Exceptions;
using Fabillio.Inventory.Cqrs.Actors;
using Fabillio.Inventory.Cqrs.EventHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>(
    typeof(Program).FullName, LogLevel.Warning);

builder.Host.UseDefaultServiceProvider(opt => opt.ValidateScopes = false);

if (!builder.Environment.IsDevelopment())
{
    var keyVaultName = builder.Configuration.GetValue<string>("KeyVaultName");
    var keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
}

builder.Host.ConfigureServices(services =>
{
    services.AddApplicationInsightsTelemetry();

    services
        .AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

    services.ConfigureRaven(sectionName: "InventoryRavenSettings");

    services.AddDaprClient();
    services.AddEvents(new[]{ typeof(UpdateProductsRemainingCount).Assembly });

    services.AddMediatR(
        typeof(ProductsController).Assembly,
        typeof(GetAllProductsQuery).Assembly
    );

    services.AddExceptionHandling(new[] { typeof(Program).Assembly });

    services.AddHttpContextAccessor();

    services.AddSwaggerDocumentation();

    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            b => b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    services.AddActors(options =>
    {
        options.Actors.RegisterActor<ProductActor>();
    });
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("CorsPolicy");
}

app.UseSwaggerDocumentation();

app.UseStaticFiles();

var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
app.UseExceptionHandling(serviceScopeFactory);

app.UseRouting();
app.UseCloudEvents();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", context => context.Response.WriteAsync("Inventory Service"))
        .WithMetadata(new AllowAnonymousAttribute());
    endpoints.MapControllers();
    endpoints.MapSamvirkEventsSubscriptions(serviceScopeFactory);
    endpoints.MapActorsHandlers();
});

app.InitRavenDatabase<Product>(
    builder.Configuration,
    null,
    builder.Configuration.GetSection("InventoryRavenSettings:Urls").Get<string[]>(),
    null);

await app.RunAsync();