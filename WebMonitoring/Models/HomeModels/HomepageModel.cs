using System.Collections.Generic;

namespace WebMonitoring.Models.HomeModels
{
    public class HomepageModel
    {
        public ICollection<WebsiteModel> Websites { get; set; }
    }
}