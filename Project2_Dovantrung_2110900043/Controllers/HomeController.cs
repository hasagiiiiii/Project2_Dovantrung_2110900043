using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2_Dovantrung_2110900043.DB;
using Project2_Dovantrung_2110900043.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace Project2_Dovantrung_2110900043.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDb db;
        private readonly IHttpContextAccessor context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public int valueLogin;
        public HomeController(ILogger<HomeController> logger , IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            db = new AppDb();
            context = httpContextAccessor;
            this.webHostEnvironment = webHostEnvironment;
        }
        [Authorize]
        public IActionResult Index()
        {
            //if (context.HttpContext.Session.TryGetValue("id_user", out byte[] userIdByte))
            //{
            //    var index = context.HttpContext.Session.GetInt32("id_user");
            //    //var index = BitConverter.ToInt32(userIdByte, 0);  
            //}
            //    ViewBag.id_user = TempData["id_user"];
            //    int value = ViewBag.id_user;
            //TempData["id_profile"] = value;
            ////int check = Int32.Parse(value);

           
            return View();
        }

        //--------Profile-----------\\



        [Authorize]
        public IActionResult Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                foreach (var claim in User.Claims)
                {
                    //valueLogin = claim.Value;
                    int.TryParse(claim.Value, out valueLogin);
                    break;

                }
            }
            var user = from u in db.Users
                       where u.id_user == valueLogin
                       select new UserModel()
                       {
                           id_user = u.id_user,
                           username = u.username,
                           email = u.email,
                           bio= u.bio,
                           passsword = u.passsword,
                           img_user = u.img_user,

                       };
            var nguoidung = new ParentModel
            {
                Users = user.ToList(),
                
            };
            return View( nguoidung);
        }


        public async Task<IActionResult> UpdateProfile (UserModel user)
        {
            if (user.Url_img != null)
            {
                // Guid.NewGuid().ToString() + "" +
                string folder = "image/";
                folder +=user.Url_img.FileName ;
                string severFolder = Path.Combine(webHostEnvironment.WebRootPath,folder);
                await user.Url_img.CopyToAsync(new FileStream(severFolder,FileMode.Create));
                //System.IO.File.Delete(severFolder);

                var updaeU = db.Users.Find(user.id_user);
                if (updaeU != null)
                {
                    updaeU.username = (user.UserName!=null) ? user.UserName : updaeU.username;
                    updaeU.email = (user.email!=null) ? user.email : updaeU.email;
                    updaeU.img_user = folder;
                      db.SaveChanges();
                }
                return Redirect("/Home/Profile");
            }
            return View("Profile");
        }

        //--------Menu-----------\\
        public IActionResult Menu()
        {
            ViewBag.id_profile = TempData["id_profile"];
            int value = ViewBag.id_profile;
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Home/Login");
        }


        // Login 
        public IActionResult Login()
        {
            ClaimsPrincipal claimsUSer = HttpContext.User;
            if (claimsUSer.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
        [HttpPost]
       public async Task<IActionResult > getLogin(UserModel model)
        {
            
            var user = db.Users.FirstOrDefault(u => u.username == model.username);
            if (user == null)
            {
                ViewData["Error"] = "Username not found";
                return Redirect("/Home/Login");
            }
            else
            {
                bool isCorrectPasword = BC.Verify(model.passsword, user.passsword);

                if (!isCorrectPasword)
                {
                    ViewData["Error"] = "password incorrect";

                    return Redirect("/Home/Login");


                }


                //List<Claim> claims = new List<Claim>();
                //new Claim(ClaimTypes.NameIdentifier,model.UserName);
                //new Claim("Other", "emxaple role");

                //ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.id_user.ToString())); // Thêm Claim với NameIdentifier
                claims.Add(new Claim("Other", "example role")); // Thêm Claim với tên "Other" và giá trị "example role"

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                };


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);


                context.HttpContext.Session.SetInt32("id_user", user.id_user);

                var index = context.HttpContext.Session.GetInt32("id_user");
                TempData["id_user"] = index;
                if (user.is_admin)
                {
                    return Redirect("/Admin/Home/");
                }
            }
                return Redirect("/");

        }
        public IActionResult setLogin()
        {
           

            return View();
        }

        //----------------Sign-Up---------------------------
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult setRegister(UserModel model)
        {
            
            var newUser = new User()
            {
                username = model.username,
                email = model.email,
                passsword = BC.HashPassword(model.passsword.Replace(" ", "")),
                bio = "",
                img_user = "",
                is_admin = false
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return Redirect("/Home/Login");
        }





 


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}