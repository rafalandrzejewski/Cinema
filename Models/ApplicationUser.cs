﻿using Microsoft.AspNetCore.Identity;

namespace Cinema.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
