using Project2_Dovantrung_2110900043.DB;

namespace Project2_Dovantrung_2110900043.Models
{
    public class ParentModel
    {
        public List<UserModel> Users { get; set; }

        public List<UserModel> UserUpdate { get; set; }

        public List<ProductModel> productModels { get; set; }
        public List<ChefModel> chefModels { get; set; }

        public List<CardModel> cards { get; set; }
    }
}
