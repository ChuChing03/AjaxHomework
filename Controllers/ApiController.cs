using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158Site.Models;
using MSIT158Site.Models.DTO;
using Newtonsoft.Json;

namespace MSIT158Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IPasswordHasher<Member> _passwordHasher;
        public ApiController(MyDBContext context, IWebHostEnvironment hostEnvironment, IPasswordHasher<Member> passwordHasher)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _passwordHasher = passwordHasher;
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
        //public IActionResult Register(Member member, IFormFile avatar)
        //{
        //    if (string.IsNullOrEmpty(member.Name))
        //    {
        //        member.Name = "guest";
        //    }

        //    //取得上傳檔案的資訊
        //    //string info = $"{avatar.FileName} - {avatar.Length} - {avatar.ContentType}";
        //    //string info = _hostEnvironment.ContentRootPath;

        //    //檔案上傳
        //    //實際路徑
        //    //string uploadPath = @"C:\Users\User\Documents\workspace\MSIT158Site\wwwroot\uploads\a.png";
        //    //WebRootPath：C: \Users\User\Documents\workspace\MSIT158Site\wwwroot
        //    //ContentRootPath：C:\Users\User\Documents\workspace\MSIT158Site

        //    // 組合上傳路徑，將檔案儲存在 Web 根目錄下的 "uploads" 資料夾中
        //    string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", avatar.FileName);
        //    // 將上傳路徑存儲在 info 變數中，以供後續使用
        //    string info = uploadPath;
        //    // 使用檔案串流創建一個檔案，並將上傳的檔案資料寫入到該檔案中
        //    //uploadPath 指定要創建的檔案的路徑 FileMode.Create:表示檔案的打開模式 如果指定的檔案不存在，就創建一個新的檔案
        //    using (var fileStream = new FileStream(uploadPath, FileMode.Create))
        //    {
        //        //將從客戶端上傳的檔案avatar 資料複製到先前建立的路徑
        //        avatar.CopyTo(fileStream); // 將上傳的檔案資料複製到創建的檔案串流中
        //    }


        //    // 宣告一個用來存放二進位資料的 byte 陣列
        //    byte[] imgByte = null;

        //    // 使用 MemoryStream 創建一個記憶體串流，用來臨時存放從客戶端上傳的檔案資料
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        // 將從客戶端上傳的檔案資料複製到記憶體串流中
        //        avatar.CopyTo(memoryStream);

        //        // 將記憶體串流中的資料轉換為 byte 陣列
        //        imgByte = memoryStream.ToArray();
        //    }


        //    //寫進資料庫
        //    member.FileName = avatar.FileName;
        //    member.FileData = imgByte;
        //    _context.Members.Add(member);
        //    _context.SaveChanges();

        //    return Content(info, "text/plain", System.Text.Encoding.UTF8);



        //}
        [HttpPost]
        public IActionResult Register(Member member, IFormFile avatar, string confirmpassword)
        {
            if (member == null)
            {
                return BadRequest("必須新增會員資料");
            }

            if (string.IsNullOrEmpty(member.Name))
            {
                return BadRequest("必須新增會員姓名");
            }

            if (string.IsNullOrEmpty(member.Email))
            {
                return BadRequest("必須新增電子郵件");
            }

            if (string.IsNullOrEmpty(member.Password))
            {
                return BadRequest("必須新增密碼");
            }

            if (member.Password != confirmpassword)
            {
                return BadRequest("密碼與確認密碼不一致");
            }

            var nameExists = _context.Members.Any(m => m.Name == member.Name);
            var emailExists = _context.Members.Any(m => m.Email == member.Email);

            if (nameExists)
            {
                return Content("姓名已存在", "text/plain", System.Text.Encoding.UTF8);
            }

            if (emailExists)
            {
                return Content("電子郵件已存在", "text/plain", System.Text.Encoding.UTF8);
            }

            if (_hostEnvironment == null || string.IsNullOrEmpty(_hostEnvironment.WebRootPath))
            {
                return StatusCode(500, "Web root path is not configured");
            }

            if (avatar != null)
            {
                string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", avatar.FileName);

                try
                {
                    using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                    {
                        avatar.CopyTo(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred while saving the file: {ex.Message}");
                }

                byte[] imgByte;
                using (var memoryStream = new MemoryStream())
                {
                    avatar.CopyTo(memoryStream);
                    imgByte = memoryStream.ToArray();
                }

                member.FileName = avatar.FileName;
                member.FileData = imgByte;
            }

            // Hash the password before saving it to the database
            member.Password = _passwordHasher.HashPassword(member, member.Password);

            _context.Members.Add(member);
            _context.SaveChanges();

            return Content("註冊成功", "text/plain", System.Text.Encoding.UTF8);
        }



        [HttpPost]
        public IActionResult Spots([FromBody] SearchDTO searchDTO)
        {
            //根據分類編號搜尋景點資料
            var spots = searchDTO.categoryId == 0 ? _context.SpotImagesSpots : _context.SpotImagesSpots.Where(s => s.CategoryId == searchDTO.categoryId);

            //根據關鍵字搜尋景點資料(title、desc)
            if (!string.IsNullOrEmpty(searchDTO.keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(searchDTO.keyword) || s.SpotDescription.Contains(searchDTO.keyword));
            }




            //總共有多少筆資料
            int totalCount = spots.Count();
            //每頁要顯示幾筆資料
            int pageSize = searchDTO.pageSize;   //searchDTO.pageSize ?? 9;
            //目前第幾頁
            int page = searchDTO.page;

            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            //分頁
            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);


            //包裝要傳給client端的資料
            SpotsPagingDTO spotsPaging = new SpotsPagingDTO();
            spotsPaging.TotalCount = totalCount;
            spotsPaging.TotalPages = totalPages;
            spotsPaging.SpotsResult = spots.ToList();


            return Json(spotsPaging);
        }

    }

}
