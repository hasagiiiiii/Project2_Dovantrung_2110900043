using Microsoft.AspNetCore.Identity;

namespace Project2_Dovantrung_2110900043.Models
{
    public class UserModel:IdentityUser
    {
        public int id_user { get; set; }
        public string username { get; set; }
        public string passsword { get; set; }
        public string email { get; set; }
        public string bio { get; set; }
        public string img_user { get; set; }
        public IFormFile Url_img { get; set; }

        public Boolean is_admin { get; set; }
    }
}
