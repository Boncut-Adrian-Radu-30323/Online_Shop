using Microsoft.EntityFrameworkCore;
using OnlineShop.Common.DTOs;
using OnlineShop.DataAccess.EFModels.OnlineShop.DataAccess.EFModels;

namespace OnlineShop.DataAccess.EFModels
{
    public class OnlineShopContext : DbContext
    {
        public OnlineShopContext(DbContextOptions<OnlineShopContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderDetail>()
                 .Property(od => od.UnitPrice)
                 .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderDetail>()
                 .Property(od => od.TotalPrice)
                 .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>()
                 .Property(o => o.TotalOrderPrice)
                 .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<User>()
               .HasOne(u => u.ShoppingCart)
               .WithOne(sc => sc.User)
               .HasForeignKey<ShoppingCart>(sc => sc.UserId);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(sc => sc.ShoppingCartItems)
                .WithOne(sci => sci.ShoppingCart)
                .HasForeignKey(sci => sci.ShoppingCartId);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany()
                .HasForeignKey(sci => sci.ProductId);
            base.OnModelCreating(modelBuilder);
        }

    }
}
