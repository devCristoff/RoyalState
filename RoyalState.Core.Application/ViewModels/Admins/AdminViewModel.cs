﻿namespace RoyalState.Core.Application.ViewModels.Admins
{
    public class AdminViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
