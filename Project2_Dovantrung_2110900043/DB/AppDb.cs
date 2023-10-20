using Microsoft.EntityFrameworkCore;

namespace Project2_Dovantrung_2110900043.DB
{
    public class AppDb:DbContext
    {
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string query = "Server=TRUNG\\SQLEXPRESS;Database=Project2;Trusted_Connection=True";
            optionsBuilder.UseSqlServer(query);
        }
        public DbSet <User> Users { get; set; } 
        public DbSet<Product> Products { get; set; }
        public DbSet<Chef> Chef { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Build> Builds { get; set; }
    }
}
