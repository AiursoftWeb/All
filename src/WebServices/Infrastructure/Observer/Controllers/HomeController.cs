﻿using Aiursoft.Handler.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aiursoft.Observer.Controllers;

public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return this.Protocol(ErrorType.Success, "Welcome to Aiursoft Observer!");
    }
}