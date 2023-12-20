using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    public class Chain
    {
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; }  //Such as Shimano
        [Required]
        public string Model { get; set; }  //Such as CM8100
        [Required]
        public DateTime DatePurchased { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public string ChainLetter { get; set; }     //A, B etc. A way of identifying the chain aside from just the int Id
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public string PurchasedFrom { get; set; }
        [Required]
        public int CurrentRotation { get; set; } = 1;
        [Required]
        public int BikeId { get; set; }     //Id of the bike this chain is applied to

    }
}
