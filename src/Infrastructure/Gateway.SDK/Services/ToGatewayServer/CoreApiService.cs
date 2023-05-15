﻿using System.Threading.Tasks;
using Aiursoft.Gateway.SDK.Models.API;
using Aiursoft.Gateway.SDK.Models.API.APIAddressModels;
using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Aiursoft.Scanner.Abstract;
using Aiursoft.XelNaga.Models;
using Aiursoft.XelNaga.Services;
using Newtonsoft.Json;

namespace Aiursoft.Gateway.SDK.Services.ToGatewayServer;

public class CoreApiService : IScopedDependency
{
    private readonly APIProxyService _http;
    private readonly GatewayLocator _serviceLocation;

    public CoreApiService(
        GatewayLocator serviceLocation,
        APIProxyService http)
    {
        _serviceLocation = serviceLocation;
        _http = http;
    }

    /// <summary>
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="pageNumber">Starts from 1</param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<AiurPagedCollection<Grant>> AllUserGrantedAsync(string accessToken, int pageNumber, int pageSize)
    {
        var url = new AiurUrl(_serviceLocation.Endpoint, "API", "AllUserGranted", new AllUserGrantedAddressModel
        {
            AccessToken = accessToken,
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        var result = await _http.Get(url, true);
        var jResult = JsonConvert.DeserializeObject<AiurPagedCollection<Grant>>(result);

        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> DropGrantsAsync(string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Endpoint, "API", "DropGrants", new { });
        var form = new AiurUrl(string.Empty, new
        {
            AccessToken = accessToken
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }
}