﻿using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aiursoft.Configuration.Controllers;

public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return this.Protocol(Code.Success, "Welcome to Aiursoft Configuration center!");
    }
}