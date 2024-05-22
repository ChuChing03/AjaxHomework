using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158Site.Models;
using Newtonsoft.Json;

namespace MSIT158Site.Controllers
{
    public class ApiController : Controller
    {
     private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ApiController(MyDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }


        public IActionResult Index()
        {
            System.Threading.Thread.Sleep(10000);
            return Content("世界, 您好!!","text/html", System.Text.Encoding.UTF8);
        }

        public IActionResult Cities()
        {
            var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }
        //根據城市名讀出不會重複的鄉鎮區
        public IActionResult Districts(string city)
        {
            var districts = _context.Addresses.Where(a => a.City == city).Select(a => a.SiteId).Distinct();
            return Json(districts);
        }
        //根據鄉鎮區讀出路名
        public IActionResult Roads(string districts)
        {
            var roads = _context.Addresses.Where(a => a.SiteId == districts).Select(a => a.Road);
            return Json(roads);
        }
        public IActionResult Avatar(int id =1)
        {
            Member? member = _context.Members.Find(id);
            if(member != null)
            {
                byte[] img = member.FileData;
                if(img != null)
                {
                    return File(img, "image/jpeg");
                }

            }
            return NotFound();
        }

       

        [HttpGet]
        public IActionResult CheckAccountAction(string name, string email, int age)
        {
            var nameExists = _context.Members.Any(m => m.Name == name);
            var emailExists = _context.Members.Any(m => m.Email == email);

            if (nameExists)
            {
                return Content("帳號已存在", "text/html", System.Text.Encoding.UTF8);
            }
            else if (emailExists)
            {
                return Content("電子郵件已存在", "text/html", System.Text.Encoding.UTF8);
            }
            else
            {
                return Content($"Hello {name} !! 你 {age} 歲了,電子郵件是{email}", "text/html", System.Text.Encoding.UTF8);
            }
        }

        [HttpGet]
        public IActionResult CheckName(string name)
        {
            var nameExists = _context.Members.Any(m => m.Name == name);

            if (nameExists)
            {
                return Content("姓名已存在", "text/html", System.Text.Encoding.UTF8);
            }
            else
            {
                return Content("姓名可使用", "text/html", System.Text.Encoding.UTF8);
            }
        }

        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _context.Members.Any(m => m.Email == email);

            if (emailExists)
            {
                return Content("電子郵件已存在", "text/html", System.Text.Encoding.UTF8);
            }
            else
            {
                return Content("電子郵件可使用", "text/html", System.Text.Encoding.UTF8);
            }
        }

        //public IActionResult Register(string userName, string email, int age = 20)
        //IFormFile 是 ASP.NET Core 中用於處理檔案上傳的介面，avatar 參數用於接收使用者上傳的頭像檔案
        public IActionResult Register(Member member, IFormFile avatar)
        {
            if (string.IsNullOrEmpty(member.Name))
            {
                member.Name = "guest";
            }

            //取得上傳檔案的資訊
            //string info = $"{avatar.FileName} - {avatar.Length} - {avatar.ContentType}";
            //string info = _hostEnvironment.ContentRootPath;

            //檔案上傳
            //實際路徑
            //string uploadPath = @"C:\Users\User\Documents\workspace\MSIT158Site\wwwroot\uploads\a.png";
            //WebRootPath：C: \Users\User\Documents\workspace\MSIT158Site\wwwroot
            //ContentRootPath：C:\Users\User\Documents\workspace\MSIT158Site

            // 組合上傳路徑，將檔案儲存在 Web 根目錄下的 "uploads" 資料夾中
            string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", avatar.FileName);
            // 將上傳路徑存儲在 info 變數中，以供後續使用
            string info = uploadPath;
            // 使用檔案串流創建一個檔案，並將上傳的檔案資料寫入到該檔案中
            //uploadPath 指定要創建的檔案的路徑 FileMode.Create:表示檔案的打開模式 如果指定的檔案不存在，就創建一個新的檔案
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                //將從客戶端上傳的檔案avatar 資料複製到先前建立的路徑
                avatar.CopyTo(fileStream); // 將上傳的檔案資料複製到創建的檔案串流中
            }


            return Content(info, "text/plain", System.Text.Encoding.UTF8);
            //寫進資料庫
            //_context.Members.Add(member);
            //_context.SaveChanges();

            // return Content($"Hello {member.Name}，{member.Age} 歲了，電子郵件是 {member.Email}", "text/html", System.Text.Encoding.UTF8);
        }
    }

}

