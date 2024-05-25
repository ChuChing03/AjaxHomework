using Microsoft.AspNetCore.Mvc;
using MSIT158Site.Models;

namespace MSIT158Site.Controllers
{
    public class HomeworkController : Controller
    {
        private readonly ILogger<HomeworkController> _logger;
        private readonly MyDBContext _context;
        public HomeworkController(ILogger<HomeworkController> logger, MyDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Homework1()
        {
            return View();
        }
        public IActionResult Homework2()
        {
            return View();
        }
        public IActionResult Homework3()
        {
            return View();
        }
        public IActionResult Homework4()
        {
            return View();
        }

    }
}
