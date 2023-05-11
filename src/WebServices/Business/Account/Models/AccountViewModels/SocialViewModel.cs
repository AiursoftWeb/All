﻿using System;
using System.Collections.Generic;
using Aiursoft.Gateway.SDK.Models.API;
using Aiursoft.Identity.Services.Authentication;

namespace Aiursoft.Account.Models.AccountViewModels;

public class SocialViewModel : AccountViewModel
{
    [Obsolete(error: true, message: "This method is only for framework!")]
    public SocialViewModel()
    {
    }

    public SocialViewModel(AccountUser user) : base(user, "Audit Log")
    {
    }

    public List<AiurThirdPartyAccount> Accounts { get; set; }
    public IEnumerable<IAuthProvider> Providers { get; set; }
}