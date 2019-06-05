using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMonitoring.Data
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }
        public virtual string Source { get; set; }
        public virtual string Author { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Url { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual DateTime? PublishedAt { get; set; }
    }
}