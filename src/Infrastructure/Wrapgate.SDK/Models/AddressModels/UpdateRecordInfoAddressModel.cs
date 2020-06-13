﻿using Aiursoft.SDKTools.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aiursoft.Wrapgate.SDK.Models.AddressModels
{
    public class UpdateRecordInfoAddressModel
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        [ValidDomainName]
        public string OldRecordName { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        [ValidDomainName]
        public string NewRecordName { get; set; }
        [Required]
        public RecordType NewType { get; set; }
        [Required]
        public string NewUrl { get; set; }
    }
}