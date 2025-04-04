using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ToursAndTravels.Models
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required(ErrorMessage = "Driver name is required.")]
        [StringLength(100)]
        public string DriverName { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [StringLength(15)]
        [Phone]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "License number is required.")]
        [StringLength(50)]
        public string LicenseNumber { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        // ✅ Ensuring Salaries list is initialized to avoid null reference issues
        public ICollection<DriverSalary> Salaries { get; set; } = new List<DriverSalary>();
    }
}
