﻿namespace Aiursoft.Directory.SDK.Models.API.UserAddressModels;

public class SetPhoneNumberAddressModel : UserOperationAddressModel
{
    /// <summary>
    ///     Not required to set it null!
    /// </summary>
    public string Phone { get; set; }
}