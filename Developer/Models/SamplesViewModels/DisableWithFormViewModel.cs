﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aiursoft.Developer.Models.SamplesViewModels
{
    public class DisableWithFormViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Aiursoft Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
