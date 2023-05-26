﻿using System.ComponentModel.DataAnnotations;

namespace Aiursoft.Directory.SDK.Models.API.UserAddressModels;

public class ChangePasswordAddressModel : UserOperationAddressModel
{
    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    [MaxLength(32)]
    public string OldPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    [MaxLength(32)]
    public string NewPassword { get; set; }
}