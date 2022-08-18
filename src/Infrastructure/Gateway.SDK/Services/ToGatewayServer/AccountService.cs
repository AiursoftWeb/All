﻿using Aiursoft.Gateway.SDK.Models.API.AccountAddressModels;
using Aiursoft.Gateway.SDK.Models.API.AccountViewModels;
using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Aiursoft.Scanner.Interfaces;
using Aiursoft.XelNaga.Models;
using Aiursoft.XelNaga.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Aiursoft.Gateway.SDK.Services.ToGatewayServer
{
    public class AccountService : IScopedDependency
    {
        private readonly GatewayLocator _serviceLocation;
        private readonly APIProxyService _http;

        public AccountService(
            GatewayLocator serviceLocation,
            APIProxyService http)
        {
            _serviceLocation = serviceLocation;
            _http = http;
        }

        public async Task<CodeToOpenIdViewModel> CodeToOpenIdAsync(string accessToken, int code)
        {
            var url = new AiurUrl(_serviceLocation.Endpoint, "Account", "CodeToOpenId", new CodeToOpenIdAddressModel
            {
                AccessToken = accessToken,
                Code = code
            });
            var result = await _http.Get(url, true);
            var jResult = JsonConvert.DeserializeObject<CodeToOpenIdViewModel>(result);

            if (jResult.Code != ErrorType.Success)
            {
                throw new AiurUnexpectedResponse(jResult);
            }

            return jResult;
        }

        public async Task<UserInfoViewModel> OpenIdToUserInfo(string accessToken, string openid)
        {
            var url = new AiurUrl(_serviceLocation.Endpoint, "Account", "UserInfo", new UserInfoAddressModel
            {
                AccessToken = accessToken,
                OpenId = openid
            });
            var result = await _http.Get(url, true);
            var jResult = JsonConvert.DeserializeObject<UserInfoViewModel>(result);
            if (jResult.Code != ErrorType.Success)
            {
                throw new AiurUnexpectedResponse(jResult);
            }

            return jResult;
        }
    }
}
