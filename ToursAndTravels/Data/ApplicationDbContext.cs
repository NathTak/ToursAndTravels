using ToursAndTravels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToursAndTravels.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripExpense> TripExpenses { get; set; }
        public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
        public DbSet<DriverSalary> DriverSalaries { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<ProfitLossReport> ProfitLossReports { get; set; }

        // ✅ Define Relationships and Decimal Precision
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🎯 Define decimal precision to avoid truncation issues
            modelBuilder.Entity<Trip>()
                .Property(t => t.TripRent)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<VehicleMaintenance>()
                .Property(vm => vm.Amount)
                .HasColumnType("decimal(18,2)");

            // ✅ Define Relationship: One Trip → Many TripExpenses
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.TripExpenses)  // One Trip has Many Expenses
                .WithOne(te => te.Trip)        // Each Expense belongs to One Trip
                .HasForeignKey(te => te.TripID) // Foreign Key
                .OnDelete(DeleteBehavior.Cascade); // If a Trip is deleted, delete its Expenses too
        }
    }
}
