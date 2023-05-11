﻿using Aiursoft.Archon.SDK.Services;
using Aiursoft.Archon.Services;
using Aiursoft.Developer.SDK;
using Aiursoft.Observer.SDK;
using Aiursoft.SDK;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aiursoft.Archon;

public class Startup
{
    public IConfiguration Configuration;
    public PrivateKeyStore KeyStore;

    public Startup(IConfiguration configuration)
    {
        AppsContainer.CurrentAppId = configuration["ArchonAppId"];
        AppsContainer.CurrentAppSecret = configuration["ArchonAppSecret"];
        Configuration = configuration;
        KeyStore = new PrivateKeyStore();
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddAiurAPIMvc();
        services.AddSingleton(KeyStore);
        services.AddSingleton(new ArchonLocator(Configuration["ArchonEndpoint"], KeyStore.GetPrivateKey()));
        services.AddObserverServer(Configuration.GetConnectionString("ObserverConnection"));
        services.AddDeveloperServer(Configuration.GetConnectionString("DeveloperConnection"));
        services.AddAiursoftSDK();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseAiurAPIHandler(env.IsDevelopment());
        app.UseAiursoftAPIDefault();
    }
}