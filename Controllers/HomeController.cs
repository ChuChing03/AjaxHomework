using Microsoft.AspNetCore.Mvc;
using MSIT158Site.Models;
using MSIT158Site.Models.DTO;
using System.Diagnostics;

namespace MSIT158Site.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly ILogger<HomeController> _logger;
        private readonly MyDBContext _context;

        public HomeController(ILogger<HomeController> logger, MyDBContext context)
        {
            _logger = logger;
            _context = context;
            

        }

        public IActionResult Index()
        {          
            return View(_context.Categories);
        }

        public IActionResult First()
        {
            return View();
        }
        public IActionResult Address()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult JSONTest()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //它使用了 FromBody 屬性，表示要從 HTTP 請求的主體中讀取資料
        public IActionResult Spots([FromBody] SearchDTO search)
        {
           // 返回 JSON 物件，包含從客戶端接收到的搜尋條件或其他資訊
           return Json(search);
        }
        public IActionResult CallApi()
        {
            return View();
        }

    }
}