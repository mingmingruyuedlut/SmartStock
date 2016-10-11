
namespace SmartStock.Core.Context
{
    using Entities;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    public partial class SSContext : DbContext
    {
        public SSContext() : base("name=SmartStockConnection")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            
            modelBuilder.Entity<TradingStock>().Property(x => x.ClosingPrice).HasPrecision(18, 3);

            modelBuilder.Entity<TradingClient>().Property(x => x.ProfitPercent).HasPrecision(18, 3);
            modelBuilder.Entity<TradingClient>().Property(x => x.OriginalAmount).HasPrecision(18, 3);
            modelBuilder.Entity<TradingClient>().Property(x => x.StockAmount).HasPrecision(18, 3);
            modelBuilder.Entity<TradingClient>().Property(x => x.AvaliableAmount).HasPrecision(18, 3);

            modelBuilder.Entity<TradingOrder>().Property(x => x.SettleAmount).HasPrecision(18, 3);
            modelBuilder.Entity<TradingOrder>().Property(x => x.TradingAmount).HasPrecision(18, 3);
            modelBuilder.Entity<TradingOrder>().Property(x => x.TradingPrice).HasPrecision(18, 3);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<TradingStock> TradingStock { get; set; }
        public DbSet<TradingOrder> TradingOrder { get; set; }
        public DbSet<TradingClient> TradingClient { get; set; }
        public DbSet<TransferOrder> TransferOrder { get; set; }
        
        //public DbSet<ErrorLog> ErrorLog { get; set; } // this db table doesn't have primary key. it's unnormal table design. so can't created by entity framework.
    }
}
