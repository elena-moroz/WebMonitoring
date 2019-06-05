using Microsoft.EntityFrameworkCore;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMonitoring.Data;

namespace WebMonitoring.Services
{
    public class WebMonitoringJob : IJob
    {
        private readonly NewsApiClient _client;
        private readonly ApplicationDbContext _dbContext;

        public WebMonitoringJob(NewsApiClient client,
            ApplicationDbContext dbContext)
        {
            _client = client;
            _dbContext = dbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var websites = _dbContext.Websites.Include(item => item.Articles);
            foreach(var website in websites)
            {
                var request = new EverythingRequest
                {
                    Domains = new List<string> { website.Url },
                    Q = "*",
                    SortBy = SortBys.Popularity,
                    From = website.LastMonitoringDate,
                    Language = Languages.UK
                };
                var result = await _client.GetEverythingAsync(request);

                if (result.Status == Statuses.Ok)
                {
                    foreach (var article in result.Articles)
                    {
                        // title
                        Console.WriteLine(article.Title);
                        // author
                        Console.WriteLine(article.Author);
                        // description
                        Console.WriteLine(article.Description);
                        // url
                        Console.WriteLine(article.Url);
                        // image
                        Console.WriteLine(article.UrlToImage);
                        // published at
                        Console.WriteLine(article.PublishedAt);
                    }
                }

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

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}