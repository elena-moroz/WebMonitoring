using System;

namespace WebMonitoring.Models.HomeModels
{
    public class WebsiteModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
}