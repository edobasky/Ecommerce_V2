using Microsoft.EntityFrameworkCore;
using Ordering.Entities;

namespace Ordering.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        protected OrderContext()
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "Edore"; // Replace with user from IUserService
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "Edore";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OutboxMessage>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasIndex(x => x.CorrelationId); // for faster lookups
                builder.Property(x => x.Type).IsRequired();
                builder.Property(x => x.Content).IsRequired();
                builder.Property(x => x.OccuredOn).IsRequired();
                builder.Property(x => x.ProcessedOn).IsRequired(false);
            });

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
        }
    }
}
