using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158Site.Models;
using Newtonsoft.Json;

namespace MSIT158Site.Controllers
{
    public class ApiController : Controller
    {
     private readonly MyDBContext _context;
        public ApiController(MyDBContext context)
        {
            _context = context;
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

        public IActionResult Register(MemberDTO member)
        {
            if (string.IsNullOrEmpty(member.userName))
            {
                member.userName = "guest";
            }
            return Content($"Hello {member.userName}，{member.Age} 歲了，電子郵件是 {member.Email}", "text/html", System.Text.Encoding.UTF8);
        }

    }
}
