using IODynamicObject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IODynamicObject.Infrastructure.Persistence
{
    public class IODataContext : DbContext
    {
        public IODataContext(DbContextOptions<IODataContext> options) : base(options)
        {
        }

        public DbSet<IODynamicObject.Domain.Entities.IODynamicObject> IODynamicObjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IODynamicObject.Domain.Entities.IODynamicObject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SchemaType).IsRequired();
                entity.Property(e => e.Data).IsRequired().HasColumnType("JSON");
                entity.Property(e => e.Deleted).HasDefaultValue(false);
                entity.Property(e => e.CreationDateUtc).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.ModificationDateUtc).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
