namespace Project2_Dovantrung_2110900043.Models
{
    public class ProductModel
    {
        public int id_product { get; set; }
        public string name_product { get; set; }
        public string img_product { get; set; }
        public IFormFile URLimg { get; set; }
        public Decimal price { get; set; }
        public string description { get; set; }
        public bool is_sell { get; set; }
        public bool is_new { get; set; }
        public bool is_favorites { get; set; }
        public DateTime time { get; set; }
        public int id_chef { get; set; }
        public int id_type { get; set; }
    }
}
