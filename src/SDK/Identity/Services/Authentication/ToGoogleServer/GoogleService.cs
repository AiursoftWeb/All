﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Aiursoft.Directory.SDK.Configuration;
using Aiursoft.Directory.SDK.Services;
using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Aiursoft.XelNaga.Models;
using Aiursoft.XelNaga.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Aiursoft.Identity.Services.Authentication.ToGoogleServer;

public class GoogleService : IAuthProvider
{
    private readonly HttpClient _client;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly APIProxyService _http;
    private readonly DirectoryConfiguration _serviceLocation;

    public GoogleService(
        APIProxyService http,
        IHttpClientFactory clientFactory,
        IConfiguration configuration,
        IOptions<DirectoryConfiguration> serviceLocation,
        ILogger<GoogleService> logger)
    {
        _http = http;
        _serviceLocation = serviceLocation.Value;
        _client = clientFactory.CreateClient();
        _clientId = configuration["Google:ClientId"];
        _clientSecret = configuration["Google:ClientSecret"];
        if (string.IsNullOrWhiteSpace(_clientId) || string.IsNullOrWhiteSpace(_clientSecret))
        {
            logger.LogWarning(
                "Did not set correct Google credential! You can only access the service property but can execute OAuth process!");
        }
    }

    public bool IsEnabled()
    {
        return !string.IsNullOrWhiteSpace(_clientId) && !string.IsNullOrWhiteSpace(_clientSecret);
    }

    public string GetName()
    {
        return "Google";
    }

    public string GetSettingsPage()
    {
        return "https://myaccount.google.com/permissions";
    }

    public string GetButtonColor()
    {
        return "primary";
    }

    public string GetButtonIcon()
    {
        return "fab fa-google";
    }

    public string GetBindRedirectLink()
    {
        return new AiurUrl("https://accounts.google.com", "/o/oauth2/v2/auth", new GoogleAuthAddressModel
        {
            ClientId = _clientId,
            RedirectUri = new AiurUrl(_serviceLocation.Instance, $"/third-party/bind-account/{GetName()}", new { })
                .ToString(),
            State = "a",
            Scope = "profile",
            ResponseType = "code"
        }).ToString();
    }

    public string GetSignInRedirectLink(AiurUrl state)
    {
        return new AiurUrl("https://accounts.google.com", "/o/oauth2/v2/auth", new GoogleAuthAddressModel
        {
            ClientId = _clientId,
            RedirectUri =
                new AiurUrl(_serviceLocation.Instance, $"/third-party/sign-in/{GetName()}", new { }).ToString(),
            State = state.ToString(),
            Scope = "profile",
            ResponseType = "code"
        }).ToString();
    }

    public async Task<IUserDetail> GetUserDetail(string code, bool isBinding = false)
    {
        var token = await GetAccessToken(_clientId, _clientSecret, code, isBinding);
        return await GetUserInfo(token);
    }

    private async Task<string> GetAccessToken(string clientId, string clientSecret, string code, bool isBinding)
    {
        var apiAddress = "https://oauth2.googleapis.com/token";
        var url = new AiurUrl(apiAddress, new { });
        var action = isBinding ? "bind-account" : "sign-in";
        var form = new AiurUrl(string.Empty, new GoogleAccessTokenAddressModel
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Code = code,
            RedirectUri =
                new AiurUrl(_serviceLocation.Instance, $"/third-party/{action}/{GetName()}", new { }).ToString(),
            GrantType = "authorization_code"
        });
        try
        {
            var json = await _http.Post(url, form);
            var response = JsonConvert.DeserializeObject<AccessTokenResponse>(json);
            if (string.IsNullOrWhiteSpace(response.AccessToken))
            {
                throw new AiurAPIModelException(ErrorType.Unauthorized, "Invalid google crenditial");
            }

            return response.AccessToken;
        }
        catch (WebException)
        {
            throw new AiurAPIModelException(ErrorType.Unauthorized, "Invalid google crenditial");
        }
    }

    private async Task<GoogleUserDetail> GetUserInfo(string accessToken)
    {
        var apiAddress = "https://www.googleapis.com/oauth2/v3/userinfo";
        var request = new HttpRequestMessage(HttpMethod.Get, apiAddress);

        request.Headers.Add("Authorization", $"Bearer {accessToken}");
        request.Headers.Add("User-Agent", "curl/7.65.3");

        var response = await _client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<GoogleUserDetail>(json);
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                user.Name = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                user.Email = user.Name.Replace(' ', '_') + $"@from.{GetName().ToLower()}.com";
            }

            return user;
        }

        throw new WebException(response.ReasonPhrase);
    }
}