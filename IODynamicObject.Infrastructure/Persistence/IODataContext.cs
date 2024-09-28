using IODynamicObject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IODynamicObject.Infrastructure.Persistence
{
    public class IODataContext : DbContext
    {
        public IODataContext(DbContextOptions<IODataContext> options) : base(options)
        {
        }

        public DbSet<IOCustomer> Customers { get; set; }
        public DbSet<IOProduct> Products { get; set; }
        public DbSet<IOOrder> Order { get; set; }
        public DbSet<IOOrderItem> OrderItems { get; set; }

        #region Dynamic Object Tables
        public DbSet<IOObject> Objects { get; set; }
        public DbSet<IOField> Fields { get; set; }
        public DbSet<IOValue> Values { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
