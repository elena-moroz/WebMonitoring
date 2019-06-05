using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using WebMonitoring.Data;

namespace WebMonitoring.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public List<Website> Websites { get; set; }
    }
}