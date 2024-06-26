﻿using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.Developers;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IDeveloperService : IGenericService<SaveDeveloperViewModel, DeveloperViewModel, Developer>
    {
        Task<RegisterResponse> Add(SaveUserViewModel vm, string origin);
        Task<UpdateUserResponse> Update(SaveUserViewModel vm);
        Task<GenericResponse> UpdateUserStatus(string username);
    }
}
