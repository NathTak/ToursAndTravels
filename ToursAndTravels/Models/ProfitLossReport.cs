using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToursAndTravels.Models
{
    public class ProfitLossReport
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        public int TripId { get; set; } // Foreign Key to Trip Table

        [ForeignKey("TripId")]
        public Trip Trip { get; set; } // Navigation Property

        [Required]
        public DateTime TripDate { get; set; } // 🆕 Trip की Date

        [Required]
        public string VehicleNo { get; set; } // 🆕 गाड़ी नंबर

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TripRent { get; set; } // ट्रिप से हुई कमाई

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalExpense { get; set; } // ट्रिप के खर्चे

        [NotMapped] // 🆕 Database में Save नहीं होगा, सिर्फ Calculation के लिए
        public decimal ProfitOrLoss => TripRent - TotalExpense; // ✅ Auto-Calculate Profit/Loss
    }
}
