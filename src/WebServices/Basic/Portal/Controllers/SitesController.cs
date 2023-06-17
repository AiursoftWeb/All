﻿using System.Threading.Tasks;
using Aiursoft.Directory.SDK.Services;
using Aiursoft.Portal.Data;
using Aiursoft.Portal.Models;
using Aiursoft.Portal.Models.SitesViewModels;
using Aiursoft.Handler.Attributes;
using Aiursoft.Handler.Exceptions;
using Aiursoft.Handler.Models;
using Aiursoft.Identity.Attributes;
using Aiursoft.Probe.SDK.Services.ToProbeServer;
using Aiursoft.XelNaga.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aiursoft.Canon;

namespace Aiursoft.Portal.Controllers;

[AiurForceAuth]
[LimitPerMin]
[Route("Dashboard")]
public class SitesController : Controller
{
    private readonly DirectoryAppTokenService _directoryAppTokenService;
    private readonly CacheService _cache;
    private readonly FilesService _filesService;
    private readonly FoldersService _foldersService;
    private readonly SitesService _sitesService;
    public PortalDbContext _dbContext;

    public SitesController(
        PortalDbContext dbContext,
        DirectoryAppTokenService directoryAppTokenService,
        SitesService sitesService,
        FoldersService foldersService,
        FilesService filesService,
        CacheService cache)
    {
        _dbContext = dbContext;
        _directoryAppTokenService = directoryAppTokenService;
        _sitesService = sitesService;
        _foldersService = foldersService;
        _filesService = filesService;
        _cache = cache;
    }

    [Route("Apps/{id}/CreateSite")]
    public async Task<IActionResult> CreateSite([FromRoute] string id) // app id
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(id);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        var model = new CreateSiteViewModel(user)
        {
            AppId = id,
            AppName = app.AppName
        };
        return View(model);
    }

    [HttpPost]
    [Route("Apps/{AppId}/CreateSite")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSite(CreateSiteViewModel model)
    {
        var user = await GetCurrentUserAsync();
        if (!ModelState.IsValid)
        {
            model.Recover(user);
            return View(model);
        }

        var app = await _dbContext.Apps.FindAsync(model.AppId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        try
        {
            var token = await _directoryAppTokenService.GetAccessTokenWithAppInfoAsync(app.AppId, app.AppSecret);
            await _sitesService.CreateNewSiteAsync(token, model.SiteName, model.OpenToUpload, model.OpenToDownload);
            return RedirectToAction(nameof(AppsController.ViewApp), "Apps",
                new { id = app.AppId, JustHaveUpdated = true });
        }
        catch (AiurUnexpectedResponse e)
        {
            ModelState.AddModelError(string.Empty, e.Response.Message);
            model.Recover(user);
            return View(model);
        }
    }

    [Route("Apps/{appId}/Sites/{siteName}/ViewFiles/{**path}")]
    public async Task<IActionResult> ViewFiles(string appId, string siteName, string path) // siteName
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(appId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        try
        {
            var token = await _directoryAppTokenService.GetAccessTokenWithAppInfoAsync(app.AppId, app.AppSecret);
            var data = await _foldersService.ViewContentAsync(token, siteName, path);
            ViewData["AccessToken"] = token;
            var model = new ViewFilesViewModel(user)
            {
                Folder = data.Value,
                AppId = appId,
                SiteName = siteName,
                AppName = app.AppName,
                Path = path
            };
            return View(model);
        }
        catch (AiurUnexpectedResponse e) when (e.Code == ErrorType.NotFound)
        {
            return NotFound();
        }
    }

    [Route("Apps/{appId}/Sites/{siteName}/NewFolder/{**path}")]
    public async Task<IActionResult> NewFolder(string appId, string siteName, string path)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(appId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        var model = new NewFolderViewModel(user)
        {
            AppId = appId,
            SiteName = siteName,
            Path = path,
            AppName = app.AppName
        };
        return View(model);
    }

    [HttpPost]
    [Route("Apps/{appId}/Sites/{siteName}/NewFolder/{**path}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewFolder(NewFolderViewModel model)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(model.AppId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            model.Recover(user, app.AppName);
            return View(model);
        }

        try
        {
            var token = await _directoryAppTokenService.GetAccessTokenWithAppInfoAsync(app.AppId, app.AppSecret);
            await _foldersService.CreateNewFolderAsync(token, model.SiteName, model.Path, model.NewFolderName, false);
            return RedirectToAction(nameof(ViewFiles),
                new { appId = model.AppId, siteName = model.SiteName, path = model.Path });
        }
        catch (AiurUnexpectedResponse e)
        {
            ModelState.AddModelError(string.Empty, e.Response.Message);
            model.Recover(user, app.AppName);
            return View(model);
        }
    }

    [Route("Apps/{appId}/Sites/{siteName}/DeleteFolder/{**path}")]
    public async Task<IActionResult> DeleteFolder([FromRoute] string appId, [FromRoute] string siteName,
        [FromRoute] string path)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(appId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        var model = new DeleteFolderViewModel(user)
        {
            AppId = appId,
            SiteName = siteName,
            Path = path,
            AppName = app.AppName
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Apps/{appId}/Sites/{siteName}/DeleteFolder/{**path}")]
    public async Task<IActionResult> DeleteFolder(DeleteFolderViewModel model)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(model.AppId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            model.Recover(user, app.AppName);
            return View(model);
        }

        try
        {
            var token = await _directoryAppTokenService.GetAccessTokenWithAppInfoAsync(app.AppId, app.AppSecret);
            await _foldersService.DeleteFolderAsync(token, model.SiteName, model.Path);
            return RedirectToAction(nameof(ViewFiles),
                new { appId = model.AppId, siteName = model.SiteName, path = model.Path.DetachPath() });
        }
        catch (AiurUnexpectedResponse e)
        {
            ModelState.AddModelError(string.Empty, e.Response.Message);
            model.Recover(user, app.AppName);
            return View(model);
        }
    }

    [Route("Apps/{appId}/Sites/{siteName}/DeleteFile/{**path}")]
    public async Task<IActionResult> DeleteFile([FromRoute] string appId, [FromRoute] string siteName,
        [FromRoute] string path)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(appId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        var model = new DeleteFileViewModel(user)
        {
            AppId = appId,
            SiteName = siteName,
            Path = path,
            AppName = app.AppName
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Apps/{appId}/Sites/{siteName}/DeleteFile/{**path}")]
    public async Task<IActionResult> DeleteFile(DeleteFileViewModel model)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(model.AppId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            model.Recover(user, app.AppName);
            return View(model);
        }

        try
        {
            var token = await _directoryAppTokenService.GetAccessTokenWithAppInfoAsync(app.AppId, app.AppSecret);
            await _filesService.DeleteFileAsync(token, model.SiteName, model.Path);
            return RedirectToAction(nameof(ViewFiles),
                new { appId = model.AppId, siteName = model.SiteName, path = model.Path.DetachPath() });
        }
        catch (AiurUnexpectedResponse e)
        {
            ModelState.AddModelError(string.Empty, e.Response.Message);
            model.Recover(user, app.AppName);
            return View(model);
        }
    }

    [Route("Apps/{appId}/Sites/{siteName}/RenameFile/{**path}")]
    public async Task<IActionResult> RenameFile([FromRoute] string appId, [FromRoute] string siteName,
        [FromRoute] string path)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(appId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        var model = new RenameFileViewModel(user)
        {
            AppId = appId,
            SiteName = siteName,
            Path = path,
            AppName = app.AppName
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Apps/{appId}/Sites/{siteName}/RenameFile/{**path}")]
    public async Task<IActionResult> RenameFile(RenameFileViewModel model)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(model.AppId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            model.Recover(user, app.AppName);
            return View(model);
        }

        try
        {
            var token = await _directoryAppTokenService.GetAccessTokenWithAppInfoAsync(app.AppId, app.AppSecret);
            await _filesService.RenameFileAsync(token, model.SiteName, model.Path, model.NewName);
            return RedirectToAction(nameof(ViewFiles),
                new { appId = model.AppId, siteName = model.SiteName, path = model.Path.DetachPath() });
        }
        catch (AiurUnexpectedResponse e)
        {
            ModelState.AddModelError(string.Empty, e.Response.Message);
            model.Recover(user, app.AppName);
            return View(model);
        }
    }

    [Route("Apps/{appId}/Sites/{siteName}/Edit")]
    public async Task<IActionResult> Edit(string appId, string siteName)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(appId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        var accessToken = _directoryAppTokenService.GetAccessTokenWithAppInfoAsync(app.AppId, app.AppSecret);
        var siteDetail = await _sitesService.ViewSiteDetailAsync(await accessToken, siteName);
        var model = new EditViewModel(user)
        {
            AppId = appId,
            OldSiteName = siteName,
            NewSiteName = siteName,
            AppName = app.AppName,
            OpenToDownload = siteDetail.Site.OpenToDownload,
            OpenToUpload = siteDetail.Site.OpenToUpload,
            Size = siteDetail.Size
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Apps/{appId}/Sites/{oldSiteName}/Edit")]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(model.AppId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            model.Recover(user, app.AppName);
            return View(model);
        }

        try
        {
            var token = await _directoryAppTokenService.GetAccessTokenWithAppInfoAsync(app.AppId, app.AppSecret);
            await _sitesService.UpdateSiteInfoAsync(token, model.OldSiteName, model.NewSiteName, model.OpenToUpload,
                model.OpenToDownload);
            _cache.Clear($"site-public-status-{model.OldSiteName}");
            _cache.Clear($"site-public-status-{model.NewSiteName}");
            return RedirectToAction(nameof(AppsController.ViewApp), "Apps",
                new { id = app.AppId, JustHaveUpdated = true });
        }
        catch (AiurUnexpectedResponse e)
        {
            ModelState.AddModelError(string.Empty, e.Response.Message);
            model.Recover(user, app.AppName);
            model.NewSiteName = model.OldSiteName;
            return View(model);
        }
    }

    [Route("Apps/{appId}/Sites/{siteName}/Delete")]
    public async Task<IActionResult> Delete(string appId, string siteName)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(appId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        var model = new DeleteViewModel(user)
        {
            AppId = appId,
            SiteName = siteName,
            AppName = app.AppName
        };
        return View(model);
    }

    [HttpPost]
    [Route("Apps/{appId}/Sites/{siteName}/Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(DeleteViewModel model)
    {
        var user = await GetCurrentUserAsync();
        var app = await _dbContext.Apps.FindAsync(model.AppId);
        if (app == null)
        {
            return NotFound();
        }

        if (app.CreatorId != user.Id)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            model.Recover(user, app.AppName);
            return View(model);
        }

        try
        {
            var token = await _directoryAppTokenService.GetAccessTokenWithAppInfoAsync(app.AppId, app.AppSecret);
            await _sitesService.DeleteSiteAsync(token, model.SiteName);
            return RedirectToAction(nameof(AppsController.ViewApp), "Apps",
                new { id = app.AppId, JustHaveUpdated = true });
        }
        catch (AiurUnexpectedResponse e)
        {
            ModelState.AddModelError(string.Empty, e.Response.Message);
            model.Recover(user, app.AppName);
            return View(model);
        }
    }

    private async Task<PortalUser> GetCurrentUserAsync()
    {
        return await _dbContext.Users.Include(t => t.MyApps)
            .SingleOrDefaultAsync(t => t.UserName == User.Identity.Name);
    }
}