namespace Project2_Dovantrung_2110900043.Models
{
    public class CardModel
    {
        public int id_card { get; set; }
        public int id_product { get; set; }
        public int quantity { get; set; }
        public Decimal total { get; set; }
        public Decimal siglePrice { get; set; }
        public string img_product { get; set; }
        public string name_product { get; set; }
        public string detail { get; set; }
        public int id_user { get; set; }
        public Decimal sum { get; set; }
    }
}
