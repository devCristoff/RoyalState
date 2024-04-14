﻿using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;

namespace RoyalState.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminService _adminService;
        private readonly AuthenticationResponse _authViewModel;

        public AdminController(IHttpContextAccessor httpContextAccessor, IAdminService adminService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }



        #region Active & Unactive User
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string userId, string controller)
        {
            var response = await _adminService.UpdateUserStatus(userId);

            if (response.HasError)
            {
                return RedirectToRoute(new { controller = controller, action = "Index", hasError = response.HasError, message = response.Error });
            }

            return RedirectToRoute(new { controller = controller, action = "Index", });
        }
        #endregion
    }
}
