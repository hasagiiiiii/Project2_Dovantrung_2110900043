using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2_Dovantrung_2110900043.DB
{
    [Table("card")]
    public class Card
    {
        [Key]
        public int id_card { get; set; }
        public int id_product { get; set; }
        public int quantity { get; set; }
        public Decimal total { get; set; }
        public Decimal siglePrice { get; set; }
        public string img_product { get; set; }
        public int id_user { get; set; }
    }
}
