﻿using System;
using System.Threading.Tasks;
using Aiursoft.Scanner.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Aiursoft.Directory.Services;

public class APISMSSender : ITransientDependency
{
    private readonly ILogger _logger;
    private readonly string _smsAccountFrom;
    private readonly string _smsAccountIdentification;
    private readonly string _smsAccountPassword;

    public APISMSSender(
        IConfiguration configuration,
        ILogger<APISMSSender> logger)
    {
        _smsAccountFrom = configuration["SMSAccountFrom"];
        _smsAccountIdentification = configuration["SMSAccountIdentification"];
        _smsAccountPassword = configuration["SMSAccountPassword"];
        _logger = logger;
    }

    public Task SendAsync(string number, string message)
    {
        if (string.IsNullOrWhiteSpace(_smsAccountFrom))
        {
            throw new ArgumentNullException();
        }

        if (string.IsNullOrWhiteSpace(_smsAccountIdentification))
        {
            throw new ArgumentNullException();
        }

        if (string.IsNullOrWhiteSpace(_smsAccountPassword))
        {
            throw new ArgumentNullException();
        }

        try
        {
            TwilioClient.Init(_smsAccountIdentification, _smsAccountPassword);
            return MessageResource.CreateAsync(
                new PhoneNumber(number),
                from: new PhoneNumber(_smsAccountFrom),
                body: message);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Failed to send an SMS to user {Phone}", number);
            return Task.CompletedTask;
        }
    }
}