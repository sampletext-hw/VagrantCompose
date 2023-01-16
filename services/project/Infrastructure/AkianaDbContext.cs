using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Db.Account;
using Models.Db.Common;
using Models.Db.CompanyInfo;
using Models.Db.DbCart;
using Models.Db.DbOrder;
using Models.Db.DbRestaurant;
using Models.Db.LatLngs;
using Models.Db.Menu;
using Models.Db.MobilePushes;
using Models.Db.Payments;
using Models.Db.Relations;
using Models.Db.RestaurantStop;
using Models.Db.Schedule;
using Models.Db.Sessions;

namespace Infrastructure
{
    public class AkianaDbContext : DbContext
    {
        public AkianaDbContext()
        {
        }

        public AkianaDbContext(DbContextOptions<AkianaDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            if (optionsBuilder.IsConfigured) return;

            var debugcs = Environment.GetEnvironmentVariable("DEBUG_CONNECT");
            if (debugcs is not null)
            {
                optionsBuilder.UseNpgsql(debugcs);
            }
            else
            {
                throw new NotSupportedException("DbContext was not configured");
            }
        }

        private static LambdaExpression IsDeletedRestriction(Type type)
        {
            var propMethod = typeof(EF).GetMethod(nameof(EF.Property),
                BindingFlags.Static |
                BindingFlags.Public)?.MakeGenericMethod(typeof(bool));

            var parameterExpression = Expression.Parameter(type, "it");
            var constantExpression = Expression.Constant(nameof(IdEntity.IsSoftDeleted));

            var methodCallExpression = Expression.Call(
                propMethod ??
                throw new InvalidOperationException(), parameterExpression, constantExpression);

            var falseConst = Expression.Constant(false);
            var expressionCondition = Expression.MakeBinary(ExpressionType.Equal, methodCallExpression, falseConst);

            return Expression.Lambda(expressionCondition, parameterExpression);
        }

        private static void SetupSoftDelete(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                //other automated configurations left out
                if (!typeof(IdEntity).IsAssignableFrom(entityType.ClrType)) continue;

                entityType.AddIndex(entityType.FindProperty(nameof(IdEntity.IsSoftDeleted)));

                modelBuilder
                    .Entity(entityType.ClrType)
                    .HasQueryFilter(IsDeletedRestriction(entityType.ClrType));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SetupSoftDelete(modelBuilder);

            modelBuilder.Entity<TokenSession>().HasIndex(t => t.Token);

            modelBuilder
                .Entity<PriceGroup>()
                .HasMany(c => c.MenuItems)
                .WithMany(s => s.PriceGroups)
                .UsingEntity<MenuItemToPriceGroup>(
                    arg => arg.HasOne(r => r.MenuItem).WithMany(item => item.PriceGroupsRelation),
                    arg => arg.HasOne(r => r.PriceGroup).WithMany(priceGroup => priceGroup.PriceGroupItems),
                    obj => obj.HasKey(r => new {r.PriceGroupId, r.MenuItemId}));

            modelBuilder
                .Entity<MenuProduct>()
                .HasMany(c => c.MenuItems)
                .WithMany(i => i.MenuProducts)
                .UsingEntity<MenuItemToMenuProduct>(
                    arg => arg.HasOne(r => r.MenuItem).WithMany(item => item.MenuProductsRelation),
                    arg => arg.HasOne(r => r.MenuProduct).WithMany(product => product.MenuItemsRelation),
                    obj => obj.HasKey(r => new {r.MenuProductId, r.MenuItemId}));

            modelBuilder.Entity<City>()
                .HasMany(c => c.Banners)
                .WithMany(b => b.Cities)
                .UsingEntity<BannerToCity>(
                    arg => arg.HasOne(r => r.Banner).WithMany(banner => banner.CitiesRelation),
                    arg => arg.HasOne(r => r.City).WithMany(city => city.BannersRelation),
                    obj => obj.HasKey(r => new {r.BannerId, r.CityId}));

            modelBuilder.Entity<City>()
                .HasMany(c => c.MobilePushes)
                .WithMany(n => n.Cities)
                .UsingEntity<MobilePushToCity>(
                    arg => arg.HasOne(c => c.MobilePushByCity).WithMany(push => push.CitiesRelation),
                    arg => arg.HasOne(c => c.City).WithMany(city => city.MobilePushesRelation),
                    obj => obj.HasKey(r => new {r.CityId, r.MobilePushByCityId})
                );

            modelBuilder.Entity<PriceGroup>()
                .HasMany(c => c.MobilePushes)
                .WithMany(n => n.PriceGroups)
                .UsingEntity<MobilePushToPriceGroup>(
                    arg => arg.HasOne(r => r.MobilePushByPriceGroup).WithMany(push => push.PriceGroupsRelation),
                    arg => arg.HasOne(r => r.PriceGroup).WithMany(priceGroup => priceGroup.MobilePushesRelation),
                    obj => obj.HasKey(r => new {r.PriceGroupId, r.MobilePushByPriceGroupId})
                );

            modelBuilder.Entity<WorkerAccount>()
                .HasMany(a => a.Restaurants)
                .WithMany(r => r.WorkerAccounts)
                .UsingEntity<WorkerAccountToRestaurant>(
                    arg => arg.HasOne(r => r.Restaurant).WithMany(r => r.WorkerAccountsRelation),
                    arg => arg.HasOne(r => r.WorkerAccount).WithMany(a => a.RestaurantsRelation),
                    obj => obj.HasKey(r => new {r.WorkerAccountId, r.RestaurantId})
                );

            modelBuilder.Entity<WorkerAccount>()
                .HasMany(a => a.WorkerRoles)
                .WithMany(r => r.WorkerAccounts)
                .UsingEntity<WorkerAccountToRole>(
                    arg => arg.HasOne(r => r.WorkerRole).WithMany(r => r.WorkerAccountsRelation),
                    arg => arg.HasOne(r => r.WorkerAccount).WithMany(a => a.WorkerRolesRelation),
                    obj => obj.HasKey(r => new {r.WorkerAccountId, r.WorkerRoleId})
                );
        }

        public DbSet<ClientAccount> ClientAccounts { get; set; }

        public DbSet<WorkerAccount> WorkerAccounts { get; set; }

        public DbSet<WorkerRole> WorkerRoles { get; set; }
        public DbSet<TokenSession> TokenSessions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<MenuCPFC> MenuCPFCs { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<PickupOpenCloseTime> PickupTimeOpenCloses { get; set; }
        public DbSet<DeliveryOpenCloseTime> DeliveryTimeOpenCloses { get; set; }

        public DbSet<RestaurantPickupStop> PickupStops { get; set; }
        public DbSet<RestaurantDeliveryStop> DeliveryStops { get; set; }

        public DbSet<PriceGroup> PriceGroups { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Banner> Banners { get; set; }

        public DbSet<RestaurantLatLng> RestaurantLatLngs { get; set; }
        public DbSet<DeliveryZoneLatLng> ZoneLatLngs { get; set; }
        public DbSet<DeliveryAddressLatLng> DeliveryAddressLatLngs { get; set; }
        public DbSet<CityLatLng> CityLatLngs { get; set; }

        public DbSet<AboutData> AboutDatas { get; set; }
        public DbSet<DeliveryTermsData> DeliveryTermsDatas { get; set; }
        public DbSet<VacanciesData> VacanciesDatas { get; set; }
        public DbSet<ApplicationStartupImageData> ApplicationStartupImageDatas { get; set; }
        public DbSet<ApplicationTerminationData> ApplicationTerminationDatas { get; set; }
        public DbSet<VkUrlData> VkUrlDatas { get; set; }
        public DbSet<InstagramUrlData> InstagramUrlDatas { get; set; }

        public DbSet<MobilePushByCity> MobileNotificationsByCity { get; set; }
        public DbSet<MobilePushByPriceGroup> MobileNotificationsByPriceGroup { get; set; }

        public DbSet<ClientLoginRequest> ClientLoginRequests { get; set; }

        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<FavoriteItem> FavoriteItems { get; set; }
        public DbSet<OnlinePayment> OnlinePayments { get; set; }
    }
}