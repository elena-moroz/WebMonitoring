using System;

namespace WebMonitoring.Models.HomeModels
{
    public class UpdateUserWebsiteModel
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
    }
}