using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Products.Domain
{
    public class DataContext : DbContext
    {
        public DataContext():base("DefaultConnection")
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}
