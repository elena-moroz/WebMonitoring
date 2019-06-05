using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMonitoring.Data;
using WebMonitoring.Models;

namespace WebMonitoring.Services
{
    public class WebMonitoringJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public WebMonitoringJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var client = scope.ServiceProvider.GetService<NewsApiClient>();

                var websites = dbContext.Websites.Include(item => item.Articles);
                foreach (var website in websites)
                {
                    var request = new EverythingRequest
                    {
                        Domains = new List<string> { website.Url },
                        Q = "*",
                        SortBy = SortBys.Popularity,
                        From = website.LastMonitoringDate,
                        Language = Languages.UK,
                        
                    };
                    var result = await client.GetEverythingAsync(request);

                    if (result.Status == Statuses.Ok)
                    {
                        website.LastMonitoringDate = DateTime.UtcNow;
                        website.LastPublishedArticleCount = result.TotalResults;
                        website.Articles.AddRange(result.Articles.Select(item => new Data.Article
                        {
                            Author = item.Author,
                            Description = item.Description,
                            ImageUrl = item.UrlToImage,
                            PublishedAt = item.PublishedAt,
                            Source = item.Source.Name,
                            Title = item.Title,
                            Url = item.Url
                        }));

                        await dbContext.SaveChangesAsync();

                        //todo: send request on Vlad API
                        //var articles = result.Articles.Select(item => new ArticleSchema
                        //{
                        //    Title = item.Title,
                        //    Text = item.Description,
                        //    Source = item.Source.Name
                        //}).ToList();
                    }
                }
            }
        }
    }
}