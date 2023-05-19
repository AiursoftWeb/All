﻿using System;
using System.Text;
using System.Threading.Tasks;
using Aiursoft.Gateway.SDK.Services;
using Aiursoft.Gateway.SDK.Models;
using Aiursoft.Gateway.SDK.Models.ForApps.AddressModels;
using Aiursoft.Gateway.SDK.Services.ToGatewayServer;
using Microsoft.AspNetCore.Identity;

namespace Aiursoft.Identity.Services;

public class AuthService<T> where T : AiurUserBase, new()
{
    private readonly AccountService _accountService;
    private readonly AppsContainer _appsContainer;
    private readonly SignInManager<T> _signInManager;
    private readonly UserManager<T> _userManager;

    public AuthService(
        UserManager<T> userManager,
        SignInManager<T> signInManager,
        AccountService accountService,
        AppsContainer appsContainer)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _accountService = accountService;
        _appsContainer = appsContainer;
    }

    public async Task<T> AuthApp(AuthResultAddressModel model, bool isPersistent = false)
    {
        var openId = await _accountService.CodeToOpenIdAsync(await _appsContainer.AccessTokenAsync(), model.Code);
        var userInfo = await _accountService.OpenIdToUserInfo(await _appsContainer.AccessTokenAsync(), openId.OpenId);
        var current = await _userManager.FindByIdAsync(userInfo.User.Id);
        if (current == null)
        {
            current = new T();
            current.Update(userInfo);
            var result = await _userManager.CreateAsync(current);
            if (!result.Succeeded)
            {
                var message = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    message.AppendLine(error.Description);
                }

                throw new InvalidOperationException(
                    $"The user info ({userInfo.User.Id}) we get could not register to our database because {message}.");
            }
        }
        else
        {
            current.Update(userInfo);
            await _userManager.UpdateAsync(current);
        }

        await _signInManager.SignInAsync(current, isPersistent);
        return current;
    }

    public async Task<T> OnlyUpdate(T user)
    {
        var userInfo = await _accountService.OpenIdToUserInfo(await _appsContainer.AccessTokenAsync(), user.Id);
        user.Update(userInfo);
        await _userManager.UpdateAsync(user);
        return user;
    }
}