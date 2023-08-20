﻿using Aiursoft.Directory.SDK;
using Aiursoft.Observer.SDK;
using Aiursoft.Probe.Data;
using Aiursoft.Probe.Models.Configuration;
using Aiursoft.Probe.SDK;
using Aiursoft.Probe.SDK.Models.HomeViewModels;
using Aiursoft.Probe.Services;
using Aiursoft.SDK;
using Microsoft.AspNetCore.Http.Features;

namespace Aiursoft.Probe;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.Configure<FormOptions>(x => x.MultipartBodyLengthLimit = long.MaxValue);
        services.Configure<ProbeDownloadPatternConfig>(Configuration.GetSection("DownloadPatternConfig"));
        services.Configure<DiskAccessConfig>(Configuration.GetSection("DiskAccessConfig"));

        services.AddDbContextForInfraApps<ProbeDbContext>(Configuration.GetConnectionString("DatabaseConnection"));

        services.AddAiurMvc();
        services.AddAiursoftAppAuthentication(Configuration.GetSection("AiursoftAuthentication"));
        services.AddAiursoftObserver(Configuration.GetSection("AiursoftObserver"));
        services.AddAiursoftProbe(Configuration.GetSection("AiursoftProbe"));

        services.AddAiursoftSdk();
        services.AddScoped<IStorageProvider, DiskAccess>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseAiursoftHandler(env.IsDevelopment(), addUserFriendlyPages: true);
        app.UseCors(builder => builder.AllowAnyOrigin());
        app.UseAiursoftAPIAppRouters();
    }
}