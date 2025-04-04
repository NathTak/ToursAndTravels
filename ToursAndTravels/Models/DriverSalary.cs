using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToursAndTravels.Models
{
    public class DriverSalary
    {
        [Key]
        public int SalaryId { get; set; }  // Primary Key

        [Required(ErrorMessage = "Driver selection is required.")]
        [ForeignKey("Driver")]
        public int DriverId { get; set; }  // Foreign Key (Driver)

        // Navigation Property
        public virtual Driver Driver { get; set; }

        [Required(ErrorMessage = "Salary Date is required.")]
        [DataType(DataType.Date)]
        public DateTime SalaryDate { get; set; }

        [Required(ErrorMessage = "Salary Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Salary must be a positive amount.")]
        [Column(TypeName = "decimal(18,2)")]  // Defines precision in the database
        public decimal SalaryAmount { get; set; }

        [StringLength(200, ErrorMessage = "Notes cannot exceed 200 characters.")]
        public string? Notes { get; set; }
    }
}
