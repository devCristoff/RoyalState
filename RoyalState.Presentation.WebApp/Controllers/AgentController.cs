﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Property;
using RoyalState.Core.Application.ViewModels.Users;
using System.Text.Json;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public AgentController(IAgentService agentService, IHttpContextAccessor httpContextAccessor, IFileService fileService, IPropertyService propertyService, IPropertyTypeService propertyTypeService)
        {
            _agentService = agentService;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _fileService = fileService;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
        }

        #region Agent Index
        public async Task<IActionResult> Index()
        {
            var propertyViewModel = new PropertyViewModel();

            if (TempData["PropertyViewModel"] != null)
            {
                var json = TempData["PropertyViewModel"].ToString();
                propertyViewModel = JsonSerializer.Deserialize<PropertyViewModel>(json);
            }

            bool? isEmpty = TempData.Peek("IsEmpty") as bool?;

            TempData.Remove("PropertyViewModel");
            TempData.Remove("IsEmpty");

            if (isEmpty != null)
            {
                ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
                ViewBag.isEmpty = isEmpty;
                if (propertyViewModel != null)
                {
                    var propertiesHome = new List<PropertyViewModel> { propertyViewModel };
                    return View(propertiesHome);
                }
            }

            var agent = await _agentService.GetByUserIdViewModel(authViewModel.Id);
            var properties = await _propertyService.GetAgentProperties(agent.Id);

            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View(properties);

        }
        #endregion

        #region SearchPropertyByFilters
        [HttpPost]
        public async Task<IActionResult> SearchPropertyByFilters(int? propertyTypeId, double? maxPrice, double? minPrice, int? roomsNumber, int? bathsNumber)
        {
            var propertyTypes = await _propertyTypeService.GetAllViewModel();
            FilterPropertyViewModel filter = new()
            {
                PropertyTypeId = propertyTypeId,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                Bedrooms = roomsNumber,
                Bathrooms = bathsNumber
            };

            var properties = await _propertyService.GetAllViewModelWIthFilters(filter);
            bool isEmpty = properties == null || properties.Count == 0;

            ViewBag.IsEmpty = isEmpty;
            ViewBag.PropertyTypes = propertyTypes;
            return View("Index", properties ?? new List<PropertyViewModel>());
        }
        #endregion

        #region Edit Profile
        public async Task<IActionResult> EditProfile()
        {
            SaveUserViewModel vm = await _agentService.GetProfileDetails();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please check for missing or incorrect fields";
                return View("EditProfile", vm);
            }

            if (vm.File != null)
            {

                vm.ImageUrl = await _fileService.UploadFileAsync(vm.File, authViewModel.Email, true, vm.ImageUrl);

            }

            UpdateUserResponse response = new();
            response = await _agentService.UpdateAsync(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("EditProfile", vm);
            }

            return RedirectToAction("EditProfile");
        }
        #endregion        

    }
}
