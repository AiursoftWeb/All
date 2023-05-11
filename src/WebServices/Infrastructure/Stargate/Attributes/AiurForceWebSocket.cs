﻿using Aiursoft.Handler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aiursoft.Stargate.Attributes;

public class AiurForceWebSocket : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (context.HttpContext.WebSockets.IsWebSocketRequest) return;

        var arg = new AiurProtocol
        {
            Code = ErrorType.InvalidInput,
            Message = "Wrong protocal!"
        };
        context.Result = new JsonResult(arg);
    }
}