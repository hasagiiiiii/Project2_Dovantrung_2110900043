using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2_Dovantrung_2110900043.DB;
using Project2_Dovantrung_2110900043.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var cardUser = from c in db.Cards
                           join pr in db.Products on c.id_product equals pr.id_product
                           where c.id_user == valueLogin
                           select new CardModel()
                           {
                               id_card = c.id_card,
                               name_product = pr.name_product,
                               img_product = c.img_product,
                               siglePrice = pr.price,
                               quantity=c.quantity,
                               total = c.total

                           };
            var nguoidung = new ParentModel
            {
                Users = user.ToList(),
                cards = cardUser.ToList(),
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
            var menu = from pro in db.Products
                       select new ProductModel()
                       {
                           id_product = pro.id_product,
                           name_product = pro.name_product,
                           description = pro.description.Substring(0, 100),
                           img_product = pro.img_product,
                           price = pro.price,
                           time = pro.time

            
                       };
            List<ProductModel> list = menu.ToList();
            return View(list);
        }
        public IActionResult Detail(int id)
        {
            var pro = from product in db.Products
                      join chef in db.Chef
                      on product.id_chef equals chef.id_chef
                      where product.id_product == id
                      
                      select new ProductModel()
                      {
                          id_product = product.id_product,
                          name_product = product.name_product,
                          description = product.description,
                          img_product = product.img_product,
                          price = product.price,
                          time = product.time,
                          id_chef=product.id_chef,
                      };
            var masterchef = from chef in db.Chef
                             join product in db.Products
                             on chef.id_chef equals product.id_chef
                             where product.id_product == id
                             select new ChefModel()
                             {
                                 id_chef= chef.id_chef,
                                 name=chef.name,
                                 address=chef.address,
                                 age=chef.age,
                             };

            var detail = new ParentModel()
            {
                productModels = pro.ToList(),

                chefModels = masterchef.ToList(),
            };
            
            return View(detail);
        }


        //______________AddCARD_________________\\
        public IActionResult AddCard(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                foreach (var claim in User.Claims)
                {
                    //valueLogin = claim.Value;
                    int.TryParse(claim.Value, out valueLogin);
                    break;

                }
                var checkCard = from card in db.Cards
                                where card.id_user == valueLogin && card.id_product == id
                                select new CardModel()
                                {
                                    id_card = card.id_card,
                                };
                List<CardModel> list = checkCard.ToList();
                if (checkCard.ToList().Count() == 0)
                {
                    var query = db.Products.Find(id);
                    if (query != null)
                    {
                        var newCard = new Card()
                        {
                            id_product = query.id_product,
                            id_user = valueLogin,
                            quantity = 1,
                            img_product = query.img_product,
                            siglePrice = query.price,
                            total = query.price

                        };
                        db.Cards.Add(newCard);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var query = db.Cards.Find(list[0].id_card);
                    query.quantity += 1;
                    query.total = query.quantity * query.siglePrice;
                    db.Cards.Update(query);
                    db.SaveChanges();
                }
                return Redirect("/Home/Menu");

            }
            return Redirect("/Home/Login");
        }

        //_______________GetCard-------------------\\
        public IActionResult GetCard()
        {
           if(User.Identity.IsAuthenticated)
            {
                foreach (var claim in User.Claims)
                {
                    //valueLogin = claim.Value;
                    int.TryParse(claim.Value, out valueLogin);
                    break;

                }
            }

            decimal sum = 0;
                var subtotal = from c in db.Cards
                               where c.id_user == valueLogin
                               select new CardModel()
                               {
                                   id_card = c.id_card,
                                   total = c.total
                               };
                foreach (var i in subtotal.ToList())
                {
                    sum += i.total;
                }
                var card = from c in db.Cards
                           join pr in db.Products
                           on c.id_product equals pr.id_product
                           where c.id_user == valueLogin
                           
                           select new CardModel()
                           {
                               id_card = c.id_card,
                               id_user = c.id_user,
                               id_product = c.id_product,
                               name_product=pr.name_product,
                               quantity = c.quantity,
                               img_product = c.img_product,
                               siglePrice = c.siglePrice,
                               detail = pr.description.Substring(0, 50),
                               total = c.total,
                               sum = sum


                           };

                return Ok(card);
        }

        public IActionResult UpdateCard(int id, int quantity)
        {
            foreach (var claim in User.Claims)
            {
                //valueLogin = claim.Value;
                int.TryParse(claim.Value, out valueLogin);
                break;

            }

            var cardUpdate = db.Cards.Find(id);
            cardUpdate.quantity = quantity;
            cardUpdate.total = cardUpdate.siglePrice * quantity;
            db.Cards.Update(cardUpdate);
            var x = db.SaveChanges();
            return Ok(x);
        }
        public IActionResult DeleteCard(int id)
        {
            var x = db.Cards.Find(id);
            db.Cards.Remove(x);
            db.SaveChanges();
            return Redirect("/Home/Menu");
        }



        //___________Build________________\\

        public IActionResult CreateBuild()
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
            var valueCard = from c in db.Cards
                        where c.id_user == valueLogin
                        select new CardModel()
                        {
                            total = c.total,
                            img_product = c.img_product,
                        };
            List<CardModel> list = valueCard.ToList();
            decimal sum = 0;
            foreach (var i in list)
            {
                sum += i.total;
            }
            var addBuild = new Build
            {
                id_user = valueLogin,
                total = sum,

            };
            db.Builds.Add(addBuild);
            db.SaveChanges();
            return Redirect("/Home/Menu");
        }



        public IActionResult ViewBuild()
        {
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

        //----------------Sign-Up---------------------------\\
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