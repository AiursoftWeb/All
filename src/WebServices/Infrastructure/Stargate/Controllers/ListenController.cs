using System;
using System.Linq;
using System.Threading.Tasks;
using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Server;
using Aiursoft.Directory.SDK.Services;
using Aiursoft.Observer.SDK.Services.ToObserverServer;
using Aiursoft.Stargate.Attributes;
using Aiursoft.Stargate.Data;
using Aiursoft.Stargate.SDK.Models.ListenAddressModels;
using Aiursoft.Stargate.Services;
using Aiursoft.CSTools.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aiursoft.Stargate.Controllers;

[ApiExceptionHandler]
[ApiModelStateChecker]
public class ListenController : ControllerBase
{
    private readonly DirectoryAppTokenService _directoryAppTokenService;
    private readonly Counter _counter;
    private readonly ObserverService _eventService;
    private readonly ILogger<ListenController> _logger;
    private readonly StargateMemory _memoryContext;
    private readonly WebSocketPusher _pusher;

    public ListenController(
        StargateMemory memoryContext,
        WebSocketPusher pusher,
        ILogger<ListenController> logger,
        Counter counter,
        DirectoryAppTokenService directoryAppTokenService,
        ObserverService eventService)
    {
        _memoryContext = memoryContext;
        _pusher = pusher;
        _logger = logger;
        _counter = counter;
        _directoryAppTokenService = directoryAppTokenService;
        _eventService = eventService;
    }

    [AiurForceWebSocket]
    public async Task<IActionResult> Channel(ChannelAddressModel model)
    {
        var lastReadId = _counter.GetCurrent;
        var channel = _memoryContext[model.Id];
        if (channel == null)
        {
            return this.Protocol(Code.NotFound, "Can not find channel with id: " + model.Id);
        }

        if (channel.ConnectKey != model.Key)
        {
            return this.Protocol(new AiurResponse
            {
                Code = Code.Unauthorized,
                Message = "Wrong connection key!"
            });
        }

        await _pusher.Accept(HttpContext);
        var sleepTime = 0;
        try
        {
            await Task.Factory.StartNew(_pusher.PendingClose);
            channel.ConnectedUsers++;
            while (_pusher.Connected && channel.IsAlive())
            {
                channel.LastAccessTime = DateTime.UtcNow;
                var nextMessages = channel
                    .GetMessagesFrom(lastReadId)
                    .ToList();
                if (nextMessages.Any())
                {
                    var messageToPush = nextMessages.MinBy(t => t.Id);
                    if (messageToPush == null)
                    {
                        continue;
                    }

                    await _pusher.SendMessage(messageToPush.Content);
                    lastReadId = messageToPush.Id;
                    sleepTime = 0;
                }
                else
                {
                    if (sleepTime < 1000)
                    {
                        sleepTime += 5;
                    }

                    await Task.Delay(sleepTime);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Crashed while monitoring channel");
            var accessToken = await _directoryAppTokenService.GetAccessTokenAsync();
            await _eventService.LogExceptionAsync(accessToken, e, Request.Path);
        }
        finally
        {
            channel.ConnectedUsers--;
            await _pusher.Close();
        }

        return this.Protocol(new AiurResponse { Code = Code.UnknownError, Message = "You shouldn't be here." });
    }
}