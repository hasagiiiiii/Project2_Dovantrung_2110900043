using Microsoft.AspNetCore.Authorization;
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
        public HomeController( )    
        {
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
            if (id>0)
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
                                     username = u.username,
                                     passsword = u.passsword,
                                     bio = u.bio,
                                     img_user = u.img_user,
                                     email = u.email,

                                 };
                List<UserModel> userUD = userUpdate.ToList();
                return View(userUD);
            }
            return View();
        }




    }

}
