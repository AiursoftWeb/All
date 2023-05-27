﻿using System;
using Aiursoft.Handler.Models;

namespace Aiursoft.Directory.SDK.Models.API.HomeViewModels;

public class DirectoryServerConfiguration : AiurProtocol
{
    public DateTime ServerTime { get; set; }
    public bool SignedIn { get; set; }
    public string Local { get; set; }
    public string Exponent { get; set; }
    public string Modulus { get; set; }
    public AiurUserBase User { get; set; }
}