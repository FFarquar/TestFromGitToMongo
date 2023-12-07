using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    public class Bike
    {
        
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public DateTime DatePurchased { get; set; }
        [Required]
        public int WarrantyPeriodYears { get; set; }
        [Required]
        public string ImageURL { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
        [Required]
        public string PurchasedFrom { get; set; }
        public string Notes { get; set; }
    }
}
