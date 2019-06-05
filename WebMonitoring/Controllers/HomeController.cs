using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebMonitoring.Data;
using WebMonitoring.Models;
using WebMonitoring.Models.HomeModels;

namespace WebMonitoring.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            // todo: use Automapper here
            var websites = user.Websites.Select(item => new WebsiteModel
            {
                Id = item.Id,
                Url = item.Url,
                IsActive = item.IsActive,
                Name = item.Name
            }).ToList();

            return View(new HomepageModel
            {
                Websites = websites
            });
        }

        [HttpGet]
        public IActionResult AddWebsite()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWebsite(AddUserWebsiteModel model)
        {
            var user = await GetCurrentUserAsync();
            user.Websites.Add(new Website
            {
                Name = model.Name,
                Url = model.Url,
                IsActive = model.IsActive
            });
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateWebsite(Guid id)
        {
            var user = await GetCurrentUserAsync();
            var website = user.Websites.First(item => item.Id == id);
            return View(new UpdateUserWebsiteModel
            {
                Id = id,
                IsActive = website.IsActive,
                Name = website.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWebsite(UpdateUserWebsiteModel model)
        {
            var user = await GetCurrentUserAsync();
            var website = user.Websites.First(item => item.Id == model.Id);
            website.Name = model.Name;
            website.IsActive = model.IsActive;
            _dbContext.Entry(website).CurrentValues.SetValues(website);
            //_dbContext.Entry(website).Property(nameof(website.Id)).IsModified = false;
            //_dbContext.Entry(website).Property(nameof(website.Url)).IsModified = false;

            await _dbContext.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteWebsite(Guid id)
        {
            var user = await GetCurrentUserAsync();
            var website = user.Websites.First(item => item.Id == id);
            _dbContext.Websites.Remove(website);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
           var user = await _userManager.GetUserAsync(HttpContext.User);

            return _dbContext.Users.Include(item => item.Websites).Single(item => item.Id == user.Id);
        }
    }
}