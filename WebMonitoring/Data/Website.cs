using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMonitoring.Data
{
    public class Website
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastMonitoringDate { get; set; }
        public int LastPublishedArticleCount { get; set; }
        public List<Article> Articles { get; set; } 
    }
}