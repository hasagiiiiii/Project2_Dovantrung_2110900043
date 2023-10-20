using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2_Dovantrung_2110900043.DB
{
    [Table("build")]
    public class Build
    {
        [Key]
        public int id_build { get; set; }

        public int id_user { get; set; }

        public Decimal total { get; set; }
    }
}
