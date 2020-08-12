using Microsoft.EntityFrameworkCore;
using PizzaBox.Domain.Models;

namespace PizzaBox.Storing
{

  public class PizzaBoxDbContext : DbContext
  {
    
    public DbSet<PizzaModel> Pizza {get;set;}

    public DbSet<ToppingModel> Topping {get;set;}

    public DbSet<CrustModel> Crust {get;set;}

    public DbSet<OrderModel> Order {get;set;}

    public DbSet<SizeModel> Size {get;set;}

    public DbSet<StoreModel> Store {get;set;}

    public DbSet<UserModel> User {get;set;}
    public DbSet<OrderPizzas> OrderPizzas {get;set;}
    public DbSet<PizzaToppings> PizzaToppings {get;set;}

    public DbSet<SpecialtyPizza> SpecialtyPizzas {get;set;}

    public DbSet<LoginModel> Login {get;set;}



    public PizzaBoxDbContext(){


    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;database=PizzaBoxDb;User ID=sa;Password=Password123;");
            }
        }

    public PizzaBoxDbContext(DbContextOptions options) : base(options){}

    protected override void OnModelCreating(ModelBuilder builder){

      /* builder.Entity<PizzaModel>().HasKey(e => e.Id);
      builder.Entity<CrustModel>().HasKey(e => e.Id);
      builder.Entity<SizeModel>().HasKey(e => e.Id);
      builder.Entity<ToppingModel>().HasKey(e => e.Id);
       */
      builder.Entity<OrderPizzas>().HasKey(e => e.PizzaOrderId);
      builder.Entity<PizzaToppings>().HasKey(e => new {e.PizzaId, e.ToppingId});
      builder.Entity<LoginModel>().HasKey(e => e.Id);
      builder.Entity<SpecialtyPizza>().HasKey(e => e.Id);
      base.OnModelCreating(builder);

    }

  }

}