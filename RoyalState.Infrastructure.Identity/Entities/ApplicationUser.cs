﻿using Microsoft.AspNetCore.Identity;

namespace RoyalState.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }


        public string LastName { get; set; }

    }
}
