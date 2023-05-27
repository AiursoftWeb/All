﻿using System;
using System.Threading.Tasks;
using Aiursoft.Directory.SDK.Models;
using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Aiursoft.Scanner.Abstract;
using Aiursoft.XelNaga.Tools;
using Newtonsoft.Json;

namespace Aiursoft.Directory.SDK.Services;

public class AiursoftAppTokenValidator : IScopedDependency
{
    private readonly AppTokenRSAService _appTokenRSAService;

    public AiursoftAppTokenValidator(AppTokenRSAService appTokenRSAService)
    {
        _appTokenRSAService = appTokenRSAService;
    }

    public virtual async Task<string> ValidateAccessTokenAsync(string value)
    {
        ACToken token;
        try
        {
            var tokenParts = value.Split('.');
            string tokenBase64 = tokenParts[0], tokenSign = tokenParts[1];
            token = JsonConvert.DeserializeObject<ACToken>(tokenBase64.Base64ToString());
            if (DateTime.UtcNow > token.Expires)
            {
                throw new AiurAPIModelException(ErrorType.Unauthorized, "Token was timed out!");
            }

            if (!await _appTokenRSAService.VerifyDataAsync(tokenBase64.Base64ToString(), tokenSign))
            {
                throw new AiurAPIModelException(ErrorType.Unauthorized,
                    "Invalid signature! Token could not be authorized!");
            }
        }
        catch (Exception e)
        {
            throw new AiurAPIModelException(ErrorType.Unauthorized,
                $"Token was not in a valid format and can not be verified! Details: {e.Message}");
        }

        return token.AppId;
    }
}