﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Aiursoft.Probe.SDK.Models.FilesAddressModels;

public class UploadFileAddressModel
{
    public string Token { get; set; }

    [Required] [FromRoute] public string SiteName { get; set; }

    [FromRoute] public string FolderNames { get; set; }

    [FromQuery(Name = "recursiveCreate")] public bool RecursiveCreate { get; set; }
}