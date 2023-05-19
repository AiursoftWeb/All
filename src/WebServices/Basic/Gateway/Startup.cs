﻿using System;
using Aiursoft.Gateway.SDK.Services;
using Aiursoft.Developer.SDK;
using Aiursoft.Gateway.Data;
using Aiursoft.Gateway.Models;
using Aiursoft.Identity;
using Aiursoft.Identity.Services;
using Aiursoft.Identity.Services.Authentication;
using Aiursoft.Observer.SDK;
using Aiursoft.Probe.SDK;
using Aiursoft.SDK;
using Edi.Captcha;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aiursoft.Gateway.Services;

namespace Aiursoft.Gateway;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContextWithCache<GatewayDbContext>(Configuration.GetConnectionString("DatabaseConnection"));

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
        });

        services.AddIdentity<GatewayUser, IdentityRole>(options => options.Password = AuthValues.PasswordOptions)
            .AddEntityFrameworkStores<GatewayDbContext>()
            .AddDefaultTokenProviders();

        services.AddAiurMvc();
        var keyStore = new PrivateKeyStore();
        services.AddSingleton(keyStore);
        services.AddSingleton(new GatewayLocator(Configuration["GatewayEndpoint"], keyStore.GetPrivateKey()));
        services.AddDeveloperServer(Configuration.GetConnectionString("DeveloperConnection"));
        services.AddObserverServer(Configuration.GetConnectionString("ObserverConnection"));
        services.AddProbeServer(Configuration.GetConnectionString("ProbeConnection"));
        services.AddAiursoftSDK(abstracts: typeof(IAuthProvider));
        services.AddScoped<UserImageGenerator<GatewayUser>>();
        services.AddSessionBasedCaptcha();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseAiurUserHandler(env.IsDevelopment());
        app.UseSession();
        app.UseAiursoftDefault();
    }
}