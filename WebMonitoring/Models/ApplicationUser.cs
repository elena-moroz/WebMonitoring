using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WebMonitoring.Data;

namespace WebMonitoring.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Website> Websites { get; set; }
    }
}