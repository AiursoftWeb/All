﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Aiursoft.Archon.SDK.Services;
using Aiursoft.Observer.SDK.Services.ToObserverServer;
using Aiursoft.Probe.Data;
using Aiursoft.Probe.SDK.Models;
using Aiursoft.Scanner.Interfaces;
using Aiursoft.XelNaga.Services;
using Microsoft.EntityFrameworkCore;

namespace Aiursoft.Probe.Services;

public class FileDeleter : ITransientDependency
{
    private readonly AppsContainer _appsContainer;
    private readonly EventService _eventService;
    private readonly ProbeDbContext _probeDbContext;
    private readonly RetryEngine _retryEngine;
    private readonly IStorageProvider _storageProvider;

    public FileDeleter(
        ProbeDbContext probeDbContext,
        RetryEngine retryEngine,
        IStorageProvider storageProvider,
        EventService eventService,
        AppsContainer appsContainer)
    {
        _probeDbContext = probeDbContext;
        _retryEngine = retryEngine;
        _storageProvider = storageProvider;
        _eventService = eventService;
        _appsContainer = appsContainer;
    }

    public async Task DeleteOnDisk(File file)
    {
        try
        {
            var haveDaemon = await _probeDbContext.Files.Where(f => f.Id != file.Id)
                .AnyAsync(f => f.HardwareId == file.HardwareId);
            if (!haveDaemon)
            {
                await _retryEngine.RunWithTry(_ =>
                {
                    _storageProvider.DeleteToTrash(file.HardwareId);
                    return Task.FromResult(0);
                }, 10);
            }
        }
        catch (Exception e)
        {
            var token = await _appsContainer.AccessTokenAsync();
            await _eventService.LogExceptionAsync(token, e, "Deleter");
        }
    }
}