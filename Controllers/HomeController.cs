using OttBlog.Models;
using OttBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OttBlog.Services.Interfaces;
using OttBlog.Services;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using OttBlog.Data;
using Microsoft.AspNetCore.Identity;
using OttBlog.Models;
using OttBlog.Data;
using System.Threading.Tasks;
using OttBlog.ViewModels;
using OttBlog.Services.Interfaces;

namespace OttBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        public HomeController(ILogger<HomeController> logger, IBlogEmailSender emailSender, ApplicationDbContext context, IImageService imageService, UserManager<BlogUser> userManager)
        {
            _userManager = userManager;
            _imageService = imageService;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactAsync(ContactMe model)
        {
            if (ModelState.IsValid)
            {
                model.Message = $"{model.Message} <hr/> Phone: {model.Phone}";
                await _emailSender.SendContactEmailAsync(email: model.Email,
                                                         name: model.Name,
                                                         subject: model.Subject,
                                                         message: model.Message);
                return RedirectToAction("Index");


            }
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
