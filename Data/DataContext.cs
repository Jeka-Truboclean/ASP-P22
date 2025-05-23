﻿using Microsoft.EntityFrameworkCore;

namespace ASP_P22.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.User>       Users       { get; set; }
        public DbSet<Entities.UserAccess> UsersAccess { get; set; }
        public DbSet<Entities.Category>   Categories  { get; set; }
        public DbSet<Entities.Product>    Products    { get; set; }
        public DbSet<Entities.Cart>       Carts       { get; set; }
        public DbSet<Entities.CartDetail> CartDetails { get; set; }
        public DbSet<Entities.Rate>       Rates       { get; set; }
        public DbSet<Entities.AuthToken>  AuthTokens  { get; set; }
        public DbSet<Entities.UserRole>   UserRoles   { get; set; }
        public DbSet<Entities.Promotion>  Promotions { get; set; }


        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("site");

            modelBuilder.Entity<Entities.UserAccess>()
                .HasOne(ua => ua.UserRole)
                .WithMany()
                .HasForeignKey(ua => ua.RoleId);

            modelBuilder.Entity<Entities.UserRole>().HasData(
                new Entities.UserRole { 
                    Id = Guid.Parse("D1A3D3A4-3A3D-4D1A-3A3D-4D1A3D3A4D1A"), 
                    Name = "Admin",
                    Description = "Адміністратор",
                    CanCreate = true,
                    CanRead = true,
                    CanUpdate = true,
                    CanDelete = true,
                    IsEmployee = true
                },
                new Entities.UserRole { 
                    Id = Guid.Parse("D1A3D3A4-3A3D-4D1A-3A3D-4D1A3D3A4D2A"), 
                    Name = "User",
                    Description = "Користувач",
                    CanCreate = false,
                    CanRead = false,
                    CanUpdate = false,
                    CanDelete = false,
                    IsEmployee = false
                },
                new Entities.UserRole
                {
                    Id = Guid.Parse("D1A3D3A4-3A3D-4D1A-3A3D-4D1A3D3A4D3A"),
                    Name = "Employee",
                    Description = "Співробітник",
                    CanCreate = false,
                    CanRead = false,
                    CanUpdate = false,
                    CanDelete = false,
                    IsEmployee = true
                }
            );

            modelBuilder.Entity<Entities.AuthToken>()
                .HasKey(t => t.Jti);

            modelBuilder.Entity<Entities.AuthToken>()
                .HasOne(t => t.UserAccess)
                .WithMany()
                .HasPrincipalKey(ua => ua.Id)
                .HasForeignKey(t => t.Sub);

            modelBuilder.Entity<Entities.Rate>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rates);
            modelBuilder.Entity<Entities.Rate>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Rates);

            modelBuilder.Entity<Entities.UserAccess>()
                .HasIndex(a => a.Login)
                .IsUnique();

            modelBuilder.Entity<Entities.UserAccess>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accesses)
                .HasPrincipalKey(u => u.Id)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Entities.User>()
                .HasIndex(u => u.Slug);

            modelBuilder.Entity<Entities.Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products);

            modelBuilder.Entity<Entities.Product>()
                .HasIndex(p => p.Slug);

            modelBuilder.Entity<Entities.Category>()
                .HasIndex(c => c.Slug);

            modelBuilder.Entity<Entities.Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts);

            modelBuilder.Entity<Entities.CartDetail>()
                .HasOne(cd => cd.Product)
                .WithMany();
            modelBuilder.Entity<Entities.CartDetail>()
                .HasOne(cd => cd.Cart)
                .WithMany(c => c.CartDetails);

            modelBuilder.Entity<Entities.Category>().HasData(
                new Entities.Category
                {
                    Id = Guid.Parse("706C9D0D-D766-48B2-8615-3DFE795B048E"),
                    Name = "Скло",
                    Description = "Товари та вироби зі скла",
                    ImagesCsv = "glass.jpg",
                    Slug = "glass"
                },
                new Entities.Category
                {
                    Id = Guid.Parse("CC51B8CA-AD48-456D-B83F-023F17A7CEC8"),
                    Name = "Офіс",
                    Description = "Офісні та настільні товари",
                    ImagesCsv = "office.jpg",
                    Slug = "office"
                },
                new Entities.Category
                {
                    Id = Guid.Parse("3CF44C28-9B0B-4314-A7BD-410864432F7A"),
                    Name = "Каміння",
                    Description = "Вироби з натурального та штучного каміння",
                    ImagesCsv = "stone.jpg",
                    Slug = "stone"
                },
                new Entities.Category
                {
                    Id = Guid.Parse("1E7B62ED-1810-441B-A781-622F2BF86D66"),
                    Name = "Дерево",
                    Description = "Товари та вироби з деревини",
                    ImagesCsv = "wood.jpg",
                    Slug = "wood"
                }
            );
        }
    }
}
/*
 [Product]    [CartDetails]       [Cart]      [User]
    Id  ---\   Id          /------ Id       -- Id
            -- ProductId  /        UserId -/
               CartId----/         MomentOpen
               Cnt                 MomentBuy
               Price               MomentCancel
               Moment              Price


Д.З. Реалізувати відображення alert-повідомлень від сервера
щодо додавання товарів до кошику, а також помилок цього процесу
у вигляді модальних вікон Bootstrap.
 */
