using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToursAndTravels.Models
{
    public class Trip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TripId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string VehicleNo { get; set; }

        [Required]
        public string FromLocation { get; set; }

        [Required]
        public string ToLocation { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive")]
        public decimal TripRent { get; set; }

        // ✅ Trip Number Auto-Generated Property
        [NotMapped]
        public string TripNumber => $"TRIP-{TripId:D5}";

        // ✅ Navigation Property (Initialize to avoid null reference issues)
        public ICollection<TripExpense> TripExpenses { get; set; } = new List<TripExpense>();
    }
}
