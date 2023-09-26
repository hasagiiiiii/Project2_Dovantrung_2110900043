using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2_Dovantrung_2110900043.DB
{
    [Table("Nguoidung")]
    public class User
    {
        [Key]
       public int id_user { get; set; }
        public string username { get; set; }
        public string passsword { get; set; }
        public string email { get; set; }
        public string bio { get; set; }
        public string img_user { get; set; }
        public bool is_admin { get; set;}
    }
}
