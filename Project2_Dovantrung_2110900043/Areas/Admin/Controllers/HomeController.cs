using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Project2_Dovantrung_2110900043.DB;
using Project2_Dovantrung_2110900043.Models;

namespace Project2_Dovantrung_2110900043.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDb db;
        private int valueLogin;
        private readonly IWebHostEnvironment webHostEnvironment;
        public HomeController(IWebHostEnvironment webHostEnvironment )    
        {
            this.webHostEnvironment = webHostEnvironment;
            db = new AppDb();
        }

        [Area("Admin")]
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                foreach (var claim in User.Claims)
                {
                    //valueLogin = claim.Value;
                    int.TryParse(claim.Value, out valueLogin);
                    break;

                }
               
                // show user
                var user = db.Users.FirstOrDefault(u => u.id_user == valueLogin);
                if (user.is_admin)
                {
                    var Listuser = from u in db.Users
                               select new UserModel()
                               {
                                   id_user = u.id_user,
                                   username = u.username,
                                   passsword=u.passsword,
                                   email = u.email,
                                   bio = u.bio,
                                   is_admin = u.is_admin,
                                   img_user = u.img_user,
                               };
                    List<UserModel> list = Listuser.ToList();

                    return View(list);
                }
            }

            return NotFound();
        }
        public IActionResult addUser(UserModel user) { 

            if (User.Identity.IsAuthenticated)
            {
                var newUser = new User()
                {
                    username = user.username,
                    email = user.email,
                    passsword = BC.HashPassword(user.passsword.Replace(" ", "")),
                    bio = "",
                    img_user = "",
                    is_admin = user.is_admin
                };

                db.Users.Add(newUser);
                db.SaveChanges();
            }
            return Redirect("/Admin/Home");
        }

        public IActionResult Update(int id)
        {
            
                //var userUpdate = db.Users.Find(user.id_user);
                //userUpdate.username = user.username;
                //userUpdate.email = user.email;
                //userUpdate.bio = user.bio;
                //userUpdate.passsword = BC.HashPassword(user.passsword);
                //db.SaveChanges();
                var userUpdate = from u in db.Users
                                 where u.id_user == id
                                 select new UserModel()
                                 {
                                     id_user=u.id_user,
                                     username = u.username,
                                     passsword = u.passsword,
                                     bio = u.bio,
                                     img_user = u.img_user,
                                     email = u.email,

                                 };
                List<UserModel> userUD = userUpdate.ToList();
                return View(userUD);
        }
        public IActionResult Edit(UserModel u)
        {
            if (u != null)
            {
                var userUpdate = db.Users.Find(u.id_user);
                if (userUpdate != null)
                {
                   userUpdate.username = u.username!=null ? u.username : userUpdate.username;
                   userUpdate.passsword = u.passsword!=null ? BC.HashPassword(u.passsword) : userUpdate.passsword;
                   userUpdate.bio = u.bio!=null ? u.bio : userUpdate.bio;
                   userUpdate.img_user = u.img_user!=null ? u.img_user : userUpdate.img_user;
                   userUpdate.email=u.email!=null ? u.email : userUpdate.email;
                   userUpdate.is_admin = u.is_admin;
                    db.SaveChanges();
                }
            }
            return Redirect("/Admin/Home");
        }
        public IActionResult Delete(int id)
        {
            if(id > 0)
            {
                var userDelete = db.Users.Find(id);
                db.Users.Remove(userDelete);
                db.SaveChanges();
                
            }
            return Redirect("/Admin/Home");

        }

        //--------Addproduct-----------\\
        public IActionResult AddProduct()
        {
            return View();
        }
        public async Task<IActionResult> setPRoduct(ProductModel product)
        {
            if (product.URLimg != null)
            {
                string folder = "image/";
                folder += product.URLimg.FileName;
                string severFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                await product.URLimg.CopyToAsync(new FileStream(severFolder, FileMode.Create));
                if (product != null)
                {
                    var newProduct = new Product()
                    {
                        name_product = product.name_product,
                        img_product = folder,
                        description = product.description,
                        is_new = product.is_new,
                        is_sell = product.is_sell,
                        is_favorites = false,
                        price = product.price,
                        time = DateTime.Now,
                        id_chef = product.id_chef,
                        id_type = product.id_type,

                    };
                    db.Products.Add(newProduct);
                    db.SaveChanges();
                }
            }
            return Redirect("/Admin/Home");
        }
    }

}
