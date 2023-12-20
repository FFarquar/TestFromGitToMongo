using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    public class Trip
    {
        public int Id { get; set; }
        //public Chain Chain { get; set; }
        [Required]
        public int  ChainId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double TripDistance { get; set; }
        [Required]
        public int ChainRotation { get; set; }
        public string? TripDescription { get; set; }
        public string? TripNotes { get; set; }
    }
}
