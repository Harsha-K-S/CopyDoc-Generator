using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTool
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("Users/Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            IList<ApplicationUser> users = await _userManager.GetUsersInRoleAsync("*");

            return View("List", new ApplicationUserCollection(users));
        }

        [HttpGet("Users/Create")]
        public IActionResult Create()
        {
            return View("User", new ApplicationUser());
        }

        [HttpGet("Users/Edit/{userId}")]
        public async Task<IActionResult> Edit([FromRoute] string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("NotFound");
            }

            return View("User", user);
        }

        [HttpGet("Users/Delete/{userId}")]
        public async Task<IActionResult> Delete([FromRoute] string userId)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return View("NotFound");
                }

                IdentityResult result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ViewBag.Error = string.Join(",", result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost("Users/Save")]
        public async Task<ActionResult> Save(ApplicationUser model)
        {
            try
            {
                bool isNew = !model.Id.IsDefined();
                IdentityResult result;

                if (isNew)
                {
                    model.Id = Guid.NewGuid().ToString();
                    result = await _userManager.CreateAsync(model);
                }
                else
                {
                    result = await _userManager.UpdateAsync(model);
                }

                if (!result.Succeeded)
                {
                    ViewBag.Error = string.Join(",", result.Errors.Select(e => e.Description));
                }
                else
                {
                    return RedirectToAction("Dashboard");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View("User", model);
        }
    }
}