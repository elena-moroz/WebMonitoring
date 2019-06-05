using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                IsActive = item.IsActive
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
                Url = model.Url,
                IsActive = model.IsActive
            });
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