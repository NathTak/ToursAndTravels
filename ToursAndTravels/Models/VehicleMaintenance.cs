using System.ComponentModel.DataAnnotations;

namespace ToursAndTravels.Models
{
    public class VehicleMaintenance
    {
        [Key]
        public int MaintenanceID { get; set; }

        [Required]
        public string VehicleNo { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
