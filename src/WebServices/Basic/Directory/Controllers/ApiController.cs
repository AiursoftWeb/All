using System;
using System.Globalization;
using System.Threading.Tasks;
using Aiursoft.Directory.Data;
using Aiursoft.Directory.Models;
using Aiursoft.Handler.Models;
using Aiursoft.WebTools;
using Aiursoft.XelNaga.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aiursoft.Directory.Models.ApiViewModels;

namespace Aiursoft.Directory.Controllers;

public class ApiController : Controller
{
    private readonly DirectoryDbContext _dbContext;
    private readonly UserManager<DirectoryUser> _userManager;

    public ApiController(
        UserManager<DirectoryUser> userManager,
        DirectoryDbContext context)
    {
        _userManager = userManager;
        _dbContext = context;
    }

    private void _ApplyCultureCookie(string culture)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                Secure = true,
                SameSite = SameSiteMode.Strict,
                HttpOnly = true
            });
    }

    [HttpGet("set-language")]
    public IActionResult SetLang(SetLangAddressModel model)
    {
        return View(new SetLangViewModel
        {
            Host = model.Host,
            Path = model.Path
        });
    }

    [HttpPost("set-language")]
    public async Task<IActionResult> SetLang(SetLangViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            _ApplyCultureCookie(model.Culture);
        }
        catch (CultureNotFoundException)
        {
            return this.Protocol(new AiurProtocol { Message = "Not a language.", Code = ErrorType.InvalidInput });
        }

        var user = await GetCurrentUserAsync();
        if (user != null)
        {
            user.PreferedLanguage = model.Culture;
            await _userManager.UpdateAsync(user);
        }

        var toGo = new AiurUrl(model.Host, "/switch-language", new
        {
            model.Culture,
            ReturnUrl = model.Path
        });
        return Redirect(toGo.ToString());
    }

    private Task<DirectoryUser> GetCurrentUserAsync()
    {
        return _dbContext
            .Users
            .Include(t => t.Emails)
            .FirstOrDefaultAsync(t => t.UserName == User.Identity.Name);
    }
}