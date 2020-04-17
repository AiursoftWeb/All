﻿using Aiursoft.SDKTools.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aiursoft.Account.Models.AccountViewModels
{
    public class SecurityViewModel : AccountViewModel
    {
        [Obsolete(error: true, message: "This method is only for framework!")]
        public SecurityViewModel() { }
        public SecurityViewModel(AccountUser user) : base(user, "Security") { }
        public void Recover(AccountUser user)
        {
            base.Recover(user, "Security");
        }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        [MinLength(6)]
        [MaxLength(32)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [MinLength(6)]
        [MaxLength(32)]
        [NoSpace]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
        [MinLength(6)]
        [MaxLength(32)]
        [NoSpace]
        public string RepeatPassword { get; set; }
    }
}