﻿using System.ComponentModel.DataAnnotations;

namespace Aiursoft.Directory.SDK.Models.ForApps.AddressModels;

public class AuthResultAddressModel
{
    public string State { get; set; }

    [Required] public int Code { get; set; }
}