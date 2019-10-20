﻿using Aiursoft.Gateway.Controllers;
using Aiursoft.Gateway.Data;
using Aiursoft.Gateway.Models;
using Aiursoft.Pylon.Interfaces;
using Aiursoft.Pylon.Models;
using Aiursoft.Pylon.Models.API.OAuthViewModels;
using Aiursoft.Pylon.Models.ForApps.AddressModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aiursoft.Gateway.Services
{
    public class AuthFinisher : IScopedDependency
    {
        private readonly GatewayDbContext _dbContext;

        public AuthFinisher(GatewayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> FinishAuth(GatewayUser user, FinishAuthInfo model, bool forceGrant = false)
        {
            if (await user.HasAuthorizedApp(_dbContext, model.AppId) && forceGrant == false)
            {
                var pack = await user.GeneratePack(_dbContext, model.AppId);
                var url = new AiurUrl(GetRegexRedirectUrl(model.RedirectUrl), new AuthResultAddressModel
                {
                    Code = pack.Code,
                    State = model.State
                });
                return new RedirectResult(url.ToString());
            }
            else
            {
                return new RedirectToActionResult(nameof(OAuthController.AuthorizeConfirm), "OAuth", model);
            }
        }

        private Task<GatewayUser> GetUserFromEmail(string email)
        {
            return _dbContext
                .Users
                .Include(t => t.Emails)
                .SingleOrDefaultAsync(t => t.Emails.Any(p => p.EmailAddress == email));
        }

        private string GetRegexRedirectUrl(string sourceUrl)
        {
            var url = new Uri(sourceUrl);
            return $@"{url.Scheme}://{url.Host}:{url.Port}{url.AbsolutePath}";
        }
    }
}
