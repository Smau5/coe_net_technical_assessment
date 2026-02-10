using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Attributes;

namespace TA_API.Services.Data
{
    public class AssessmentDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public AssessmentDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(x =>
            {
                x.HasData(new Customer { Id = 1, Name = "John Doe" });
                x.HasData(new Customer { Id = 2, Name = "Juan Dos" });
            });

            modelBuilder.Entity<Order>(x =>
            {
                x.HasData(new Order { Id = 1, CustomerId = 1, Status = OrderStatus.Completed });
            });
        }

    }

    public class Order
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }

    public enum OrderStatus
    {
        [Display("Completed")]
        Completed,
        [Display("Pending")]
        Pending,
        [Display("Cancelled")]
        Cancelled
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Order> Orders { get; set; }
    }
}
