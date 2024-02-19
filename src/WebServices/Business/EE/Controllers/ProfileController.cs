﻿using Aiursoft.AiurProtocol.Models;
using Aiursoft.AiurProtocol.Server;
using Aiursoft.AiurProtocol.Server.Attributes;
using Aiursoft.EE.Data;
using Aiursoft.EE.Models;
using Aiursoft.EE.Models.ProfileViewModels;
using Aiursoft.Identity.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aiursoft.EE.Controllers;


public class ProfileController : Controller
{
    public readonly EEDbContext _dbContext;
    public readonly SignInManager<EEUser> _signInManager;
    public readonly UserManager<EEUser> _userManager;

    public ProfileController(
        UserManager<EEUser> userManager,
        SignInManager<EEUser> signInManager,
        EEDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = context;
    }

    public async Task<IActionResult> Overview(string id)
    {
        var user = await _userManager.FindByNameAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var courses = _dbContext.Courses.Where(t => t.OwnerId == user.Id).Take(6);
        var model = new OverviewViewModel
        {
            CoursesDisplaying = courses.Take(6)
        };
        await model.Restore(user, 0, _dbContext, await GetCurrentUserAsync());
        return View(model);
    }

    public async Task<IActionResult> Courses(string id)
    {
        var user = await _userManager.FindByNameAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var courses = _dbContext.Courses.Where(t => t.OwnerId == user.Id);
        var model = new CoursesViewModel
        {
            AllCourses = courses
        };
        await model.Restore(user, 1, _dbContext, await GetCurrentUserAsync());
        return View(model);
    }

    public async Task<IActionResult> Subscriptions(string id)
    {
        var user = await _userManager.FindByNameAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var mySubs = _dbContext
            .Subscriptions
            .Include(t => t.Course)
            .Where(t => t.UserId == user.Id);
        var model = new SubscriptionsViewModel
        {
            MySubscriptions = mySubs
        };
        await model.Restore(user, 2, _dbContext, await GetCurrentUserAsync());
        return View(model);
    }

    public async Task<IActionResult> Followings(string id) //Viewing user name
    {
        var currentUser = await GetCurrentUserAsync();
        var user = await _userManager.FindByNameAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var followings = await _dbContext
            .Follows
            .Include(t => t.Receiver)
            .Where(t => t.TriggerId == user.Id).ToListAsync();
        var model = new FollowingsViewModel
        {
            Followings = followings,
            IsMe = currentUser?.Id == user.Id
        };
        await model.Restore(user, 3, _dbContext, await GetCurrentUserAsync());
        return View(model);
    }

    public async Task<IActionResult> Followers(string id)
    {
        var user = await _userManager.FindByNameAsync(id); //Viewing user name
        if (user == null)
        {
            return NotFound();
        }

        var followers = await _dbContext
            .Follows
            .Include(t => t.Trigger)
            .Where(t => t.ReceiverId == user.Id).ToListAsync();
        var model = new FollowersViewModel
        {
            Followers = followers
        };
        await model.Restore(user, 4, _dbContext, await GetCurrentUserAsync());
        return View(model);
    }

    [HttpPost]
    [ApiExceptionHandler]
    [ApiModelStateChecker]
    [AiurForceAuth("", "", false)]
    public async Task<IActionResult> Follow(string id) //Target user id
    {
        var currentUser = await GetCurrentUserAsync();
        var user = await _dbContext.Users.SingleOrDefaultAsync(t => t.Id == id);
        if (user == null)
        {
            return this.Protocol(Code.NotFound, $"The target user with id:{id} was not found!");
        }

        var follow =
            await _dbContext.Follows.SingleOrDefaultAsync(t =>
                t.TriggerId == currentUser.Id && t.ReceiverId == user.Id);
        if (follow != null)
        {
            return this.Protocol(Code.NoActionTaken, "You have already followed the target user!");
        }

        await _dbContext.Follows.AddAsync(new Follow
        {
            TriggerId = currentUser.Id,
            ReceiverId = user.Id
        });
        await _dbContext.SaveChangesAsync();
        return this.Protocol(Code.JobDone, "You have successfully followed the target user!");
    }

    [HttpPost]
    [ApiExceptionHandler]
    [ApiModelStateChecker]
    [AiurForceAuth("", "", false)]
    public async Task<IActionResult> UnFollow(string id) //Target User Id
    {
        var currentUser = await GetCurrentUserAsync();
        var user = await _dbContext.Users.SingleOrDefaultAsync(t => t.Id == id);
        if (user == null)
        {
            return this.Protocol(Code.NotFound, $"The target user with id:{id} was not found!");
        }

        var follow =
            await _dbContext.Follows.SingleOrDefaultAsync(t =>
                t.TriggerId == currentUser.Id && t.ReceiverId == user.Id);
        if (follow == null)
        {
            return this.Protocol(Code.Conflict,
                "You did not follow the target user and can not unFollow him!");
        }

        _dbContext.Follows.Remove(follow);
        await _dbContext.SaveChangesAsync();
        return this.Protocol(Code.JobDone, "You have successfully unfollowed the target user!");
    }

    [AiurForceAuth]
    public async Task<IActionResult> EditDes()
    {
        var user = await GetCurrentUserAsync();
        var model = new EditDesViewModel
        {
            NewDes = user.LongDescription
        };
        await model.Restore(user, int.MaxValue, _dbContext, user);
        return View(model);
    }

    [AiurForceAuth]
    [HttpPost]
    public async Task<IActionResult> EditDes(EditDesViewModel model)
    {
        var user = await GetCurrentUserAsync();
        if (!ModelState.IsValid)
        {
            await model.Restore(user, int.MaxValue, _dbContext, user);
            return View(model);
        }

        user.LongDescription = model.NewDes;
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(Overview), new { id = user.UserName });
    }

    private async Task<EEUser> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(User);
    }
}