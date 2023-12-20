using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    //a class that returns chain details  and sum summary details from the trip table
    public class ChainSummaryDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; }  //Such as Shimano
        public string Model { get; set; }  //Such as CM8100
        public DateTime DatePurchased { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
        public string ChainLetter { get; set; }     //A, B etc. A way of identifying the chain aside from just the int Id
        public string ImageURL { get; set; }
        public string PurchasedFrom { get; set; }
        public int CurrentRotation { get; set; } = 1;
        public double TripsTake { get; set; } = 0;
        public decimal DistanceTravelled { get; set; } = 0;
    }
}
