﻿using System.ComponentModel.DataAnnotations;

namespace Aiursoft.Account.Models.AccountViewModels;

public class EnterCodeViewModel : AccountViewModel
{
    [Obsolete(error: true, message: "This method is only for framework!")]
    public EnterCodeViewModel()
    {
    }

    public EnterCodeViewModel(AccountUser user) : base(user, "Enter Code")
    {
    }

    [Required] [Display(Name = "Code")] public string Code { get; set; }

    [Required] public string NewPhoneNumber { get; set; }

    public void Recover(AccountUser user)
    {
        RootRecover(user, "Enter Code");
    }
}