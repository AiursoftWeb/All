﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Aiursoft.SDK.Middlewares
{
    public class EnforceHttpsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public EnforceHttpsMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            ILogger<EnforceHttpsMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // default to true.
            var disableHsts = _configuration["DisableHSTS"] == true.ToString();
            if (!disableHsts && !context.Response.Headers.ContainsKey("Strict-Transport-Security"))
            {
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=15552001; includeSubDomains; preload");
            }
            if (context.Request.Headers.ContainsKey("X-Forwarded-Proto") &&
                context.Request.Headers["X-Forwarded-Proto"] == "https")
            {
                _logger.LogInformation("Forwarded HTTP Request Handled.");
                await _next.Invoke(context);
            }
            else if (!context.Request.IsHttps)
            {
                _logger.LogWarning("Insecure HTTP request handled! Redirecting the user...");
                HandleNonHttpsRequest(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        protected virtual void HandleNonHttpsRequest(HttpContext context)
        {
            if (!string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            else
            {
                var optionsAccessor = context.RequestServices.GetRequiredService<IOptions<MvcOptions>>();
                var request = context.Request;
                var host = request.Host;
                if (optionsAccessor.Value.SslPort.HasValue && optionsAccessor.Value.SslPort > 0)
                {
                    host = new HostString(host.Host, optionsAccessor.Value.SslPort.Value);
                }
                else
                {
                    host = new HostString(host.Host);
                }
                var newUrl = string.Concat(
                    "https://",
                    host.ToUriComponent(),
                    request.PathBase.ToUriComponent(),
                    request.Path.ToUriComponent(),
                    request.QueryString.ToUriComponent());
                context.Response.Redirect(newUrl, permanent: true);
            }
        }
    }
}
