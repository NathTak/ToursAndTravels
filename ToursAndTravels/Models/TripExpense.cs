using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToursAndTravels.Models
{
    public class TripExpense
    {
        [Key]
        public int ExpenseID { get; set; } // Primary Key

        [Required]
        public int TripID { get; set; } // Foreign Key to Trips Table

        [ForeignKey("TripID")]
        public Trip? Trip { get; set; } // Navigation Property

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DriverAdvance { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal LoadingCharges { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PoliceEntryFee { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Commission { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Diesel { get; set; } = 0;

        // ✅ 🔹 Amount Property Add की गई है
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // अगर Controller में Use हो रही है

        // ✅ 🔹 TotalExpense Property को Amount से Replace कर दिया है
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalExpense
        {
            get => DriverAdvance + LoadingCharges + PoliceEntryFee + Commission + Diesel;
            private set { } // Read-only
        }
        // ✅ Ensure this method exists
        public void CalculateTotalExpense()
        {
            TotalExpense = DriverAdvance + LoadingCharges + PoliceEntryFee + Commission + Diesel;
        }
    }
}
