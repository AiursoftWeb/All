﻿using System.Threading.Tasks;
using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Aiursoft.Probe.SDK.Configuration;
using Aiursoft.Probe.SDK.Models.SitesAddressModels;
using Aiursoft.Probe.SDK.Models.SitesViewModels;
using Aiursoft.Scanner.Abstract;
using Aiursoft.XelNaga.Models;
using Aiursoft.XelNaga.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Aiursoft.Probe.SDK.Services.ToProbeServer;

public class SitesService : IScopedDependency
{
    private readonly APIProxyService _http;
    private readonly ProbeConfiguration _probeLocator;

    public SitesService(
        APIProxyService http,
        IOptions<ProbeConfiguration> serviceLocation)
    {
        _http = http;
        _probeLocator = serviceLocation.Value;
    }

    public async Task<AiurProtocol> CreateNewSiteAsync(string accessToken, string newSiteName, bool openToUpload,
        bool openToDownload)
    {
        var url = new AiurUrl(_probeLocator.Endpoint, "Sites", "CreateNewSite", new { });
        var form = new AiurUrl(string.Empty, new CreateNewSiteAddressModel
        {
            AccessToken = accessToken,
            NewSiteName = newSiteName,
            OpenToUpload = openToUpload,
            OpenToDownload = openToDownload
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<ViewMySitesViewModel> ViewMySitesAsync(string accessToken)
    {
        var url = new AiurUrl(_probeLocator.Endpoint, "Sites", "ViewMySites", new ViewMySitesAddressModel
        {
            AccessToken = accessToken
        });
        var result = await _http.Get(url, true);
        var jResult = JsonConvert.DeserializeObject<ViewMySitesViewModel>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<ViewSiteDetailViewModel> ViewSiteDetailAsync(string accessToken, string siteName)
    {
        var url = new AiurUrl(_probeLocator.Endpoint, "Sites", "ViewSiteDetail", new ViewSiteDetailAddressModel
        {
            AccessToken = accessToken,
            SiteName = siteName
        });
        var result = await _http.Get(url, true);
        var jResult = JsonConvert.DeserializeObject<ViewSiteDetailViewModel>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> UpdateSiteInfoAsync(string accessToken, string oldSiteName, string newSiteName,
        bool openToUpload, bool openToDownload)
    {
        var url = new AiurUrl(_probeLocator.Endpoint, "Sites", "UpdateSiteInfo", new { });
        var form = new AiurUrl(string.Empty, new UpdateSiteInfoAddressModel
        {
            AccessToken = accessToken,
            OldSiteName = oldSiteName,
            NewSiteName = newSiteName,
            OpenToDownload = openToDownload,
            OpenToUpload = openToUpload
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> DeleteSiteAsync(string accessToken, string siteName)
    {
        var url = new AiurUrl(_probeLocator.Endpoint, "Sites", "DeleteSite", new { });
        var form = new AiurUrl(string.Empty, new DeleteSiteAddressModel
        {
            AccessToken = accessToken,
            SiteName = siteName
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> DeleteAppAsync(string accessToken, string appId)
    {
        var url = new AiurUrl(_probeLocator.Endpoint, "Sites", "DeleteApp", new { });
        var form = new AiurUrl(string.Empty, new DeleteAppAddressModel
        {
            AccessToken = accessToken,
            AppId = appId
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