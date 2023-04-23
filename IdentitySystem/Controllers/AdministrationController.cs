﻿using IdentitySystem.Models;
using IdentitySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using IdentitySystem.DTOS;

namespace IdentitySystem.Controllers
{
    //Moderators and Admins roles are only allowed to access this controller (resources).

    [Authorize(Roles = "Moderators,Admins")]
    public class AdministrationController : Controller
    {
        /* RoleManager<IdentityRole>:-
         * 1.CreateAsync Role
         * 2.UpdateAsync Role
         * 3.DeleteAsync Role
         */

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> _roleManager,
               UserManager<AppUser> _userManager)
        {
            this.roleManager = _roleManager;
            this.userManager = _userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto dto)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = dto.Name
                };

                IdentityResult result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(dto);
        }

        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            //find role by id
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMeassage = $"Role with id = {id} not found in the system";
                return View("NotFound");
            }

            var model = new RoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name
            };

            //Retrieve all the users from UserManager  ==>  model.Users
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel roleModel)
        {
            var role = await roleManager.FindByIdAsync(roleModel.Id);

            if (role == null)
            {
                ViewBag.ErrorMeassage = $"Role with id = {roleModel.Id} not found in the system";
                return View("NotFound");
            }
            //Edit Role Name
            role.Name = roleModel.RoleName;

            //update in DB

            var result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles", "Administration");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(roleModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMeassage = $"Role with id = {id} not found in the system";
                return View("NotFound");
            }


            var usersInRoles = await userManager.GetUsersInRoleAsync(role.Name);

            if (usersInRoles.Count == 0)
            {
                var result = await roleManager.DeleteAsync(role);
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return RedirectToAction("ListRoles");
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} not found in the System";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserID = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} not found in the System";
                return View("NotFound");
            }

            for (int item = 0; item < model.Count; item++)
            {
                var user = await userManager.FindByIdAsync(model[item].UserID);
                IdentityResult result = null;

                if (model[item].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }

                else if (!model[item].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            return RedirectToAction("EditRole", new { id = roleId });
        }

        private async Task<bool> CheckRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} not found in the System";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
