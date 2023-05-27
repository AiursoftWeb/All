﻿using System.Threading.Tasks;
using Aiursoft.Directory.SDK.Configuration;
using Aiursoft.Directory.SDK.Models.API;
using Aiursoft.Directory.SDK.Models.API.UserAddressModels;
using Aiursoft.Directory.SDK.Models.API.UserViewModels;
using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Aiursoft.Scanner.Abstract;
using Aiursoft.XelNaga.Models;
using Aiursoft.XelNaga.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Aiursoft.Directory.SDK.Services.ToDirectoryServer;

public class UserService : IScopedDependency
{
    private readonly APIProxyService _http;
    private readonly DirectoryConfiguration _serviceLocation;

    public UserService(
        IOptions<DirectoryConfiguration> serviceLocation,
        APIProxyService http)
    {
        _serviceLocation = serviceLocation.Value;
        _http = http;
    }

    public async Task<AiurProtocol> ChangeProfileAsync(string openId, string accessToken, string newNickName,
        string newIconFilePathName, string newBio)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ChangeProfile", new { });
        var form = new AiurUrl(string.Empty, new ChangeProfileAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId,
            NewNickName = newNickName,
            NewIconFilePathName = newIconFilePathName,
            NewBio = newBio
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);

        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> ChangePasswordAsync(string openId, string accessToken, string oldPassword,
        string newPassword)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ChangePassword", new { });
        var form = new AiurUrl(string.Empty, new ChangePasswordAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId,
            OldPassword = oldPassword,
            NewPassword = newPassword
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);

        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurValue<string>> ViewPhoneNumberAsync(string openId, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ViewPhoneNumber", new ViewPhoneNumberAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId
        });
        var result = await _http.Get(url, true);
        var jResult = JsonConvert.DeserializeObject<AiurValue<string>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> SetPhoneNumberAsync(string openId, string accessToken, string phoneNumber)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "SetPhoneNumber", new { });
        var form = new AiurUrl(string.Empty, new SetPhoneNumberAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId,
            Phone = phoneNumber
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurCollection<AiurUserEmail>> ViewAllEmailsAsync(string accessToken, string openId)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ViewAllEmails", new ViewAllEmailsAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId
        });
        var result = await _http.Get(url, true);
        var jResult = JsonConvert.DeserializeObject<AiurCollection<AiurUserEmail>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> BindNewEmailAsync(string openId, string newEmail, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "BindNewEmail", new { });
        var form = new AiurUrl(string.Empty, new BindNewEmailAddressModel
        {
            OpenId = openId,
            NewEmail = newEmail,
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

    public async Task<AiurProtocol> DeleteEmailAsync(string openId, string thatEmail, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "DeleteEmail", new { });
        var form = new AiurUrl(string.Empty, new DeleteEmailAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId,
            ThatEmail = thatEmail
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> SendConfirmationEmailAsync(string accessToken, string userId, string email)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "SendConfirmationEmail", new { });
        var form = new AiurUrl(string.Empty, new SendConfirmationEmailAddressModel
        {
            AccessToken = accessToken,
            OpenId = userId,
            Email = email
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> SetPrimaryEmailAsync(string accessToken, string userId, string email)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "SetPrimaryEmail", new { });
        var form = new AiurUrl(string.Empty, new SetPrimaryEmailAddressModel
        {
            AccessToken = accessToken,
            OpenId = userId,
            Email = email
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurCollection<Grant>> ViewGrantedAppsAsync(string accessToken, string userId)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ViewGrantedApps", new UserOperationAddressModel
        {
            AccessToken = accessToken,
            OpenId = userId
        });
        var result = await _http.Get(url, true);
        var jResult = JsonConvert.DeserializeObject<AiurCollection<Grant>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> DropGrantedAppsAsync(string accessToken, string userId, string appId)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "DropGrantedApps", new { });
        var form = new AiurUrl(string.Empty, new DropGrantedAppsAddressModel
        {
            AccessToken = accessToken,
            OpenId = userId,
            AppIdToDrop = appId
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    /// <summary>
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="userId"></param>
    /// <param name="pageNumber">Starts from 1.</param>
    /// <returns></returns>
    public async Task<AiurPagedCollection<AuditLog>> ViewAuditLogAsync(string accessToken, string userId,
        int pageNumber)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ViewAuditLog", new ViewAuditLogAddressModel
        {
            AccessToken = accessToken,
            OpenId = userId,
            PageNumber = pageNumber
        });
        var result = await _http.Get(url, true);
        var jResult = JsonConvert.DeserializeObject<AiurPagedCollection<AuditLog>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurCollection<AiurThirdPartyAccount>> ViewSocialAccountsAsync(string accessToken, string userId)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ViewSocialAccounts", new UserOperationAddressModel
        {
            AccessToken = accessToken,
            OpenId = userId
        });
        var result = await _http.Get(url, true);
        var jResult = JsonConvert.DeserializeObject<AiurCollection<AiurThirdPartyAccount>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> UnBindSocialAccountAsync(string accessToken, string userId, string providerName)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "UnBindSocialAccount", new { });
        var form = new AiurUrl(string.Empty, new UnBindSocialAccountAddressModel
        {
            AccessToken = accessToken,
            OpenId = userId,
            ProviderName = providerName
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurValue<bool>> ViewHas2FAKeyAsync(string openId, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ViewHas2FAKey", new { });
        var form = new AiurUrl(string.Empty, new UserOperationAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurValue<bool>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurValue<bool>> ViewTwoFactorEnabledAsync(string openId, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ViewTwoFactorEnabled", new { });
        var form = new AiurUrl(string.Empty, new UserOperationAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurValue<bool>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<View2FAKeyViewModel> View2FAKeyAsync(string openId, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "View2FAKey", new UserOperationAddressModel());
        var form = new AiurUrl(string.Empty, new UserOperationAddressModel
        {
            OpenId = openId,
            AccessToken = accessToken
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<View2FAKeyViewModel>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurValue<bool>> SetTwoFAKeyAsync(string openId, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "SetTwoFAKey", new { });
        var form = new AiurUrl(string.Empty, new UserOperationAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurValue<bool>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurProtocol> ResetTwoFAKeyAsync(string openId, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "ResetTwoFAKey", new { });
        var form = new AiurUrl(string.Empty, new UserOperationAddressModel
        {
            AccessToken = accessToken,
            OpenId = openId
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurProtocol>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurValue<bool>> TwoFAVerifyCodeAsync(string openId, string accessToken, string code)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "TwoFAVerifyCode", new TwoFAVerifyCodeAddressModel());
        var form = new AiurUrl(string.Empty, new TwoFAVerifyCodeAddressModel
        {
            OpenId = openId,
            AccessToken = accessToken,
            Code = code
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurValue<bool>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurValue<bool>> DisableTwoFAAsync(string openId, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "DisableTwoFA", new UserOperationAddressModel());
        var form = new AiurUrl(string.Empty, new UserOperationAddressModel
        {
            OpenId = openId,
            AccessToken = accessToken
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurValue<bool>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }

    public async Task<AiurCollection<string>> GetRecoveryCodesAsync(string openId, string accessToken)
    {
        var url = new AiurUrl(_serviceLocation.Instance, "User", "GetRecoveryCodes", new UserOperationAddressModel());
        var form = new AiurUrl(string.Empty, new UserOperationAddressModel
        {
            OpenId = openId,
            AccessToken = accessToken
        });
        var result = await _http.Post(url, form, true);
        var jResult = JsonConvert.DeserializeObject<AiurCollection<string>>(result);
        if (jResult.Code != ErrorType.Success)
        {
            throw new AiurUnexpectedResponse(jResult);
        }

        return jResult;
    }
}