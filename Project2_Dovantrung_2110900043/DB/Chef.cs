using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2_Dovantrung_2110900043.DB
{
    [Table("master_chef")]
    public class Chef
    {
        [Key]
        public int id_chef { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string address { get; set; }
    }
}
