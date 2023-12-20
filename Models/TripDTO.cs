using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    public class TripDTO
    {
        public int Id { get; set; }
        public string ChainLetter { get; set; }
        public int ChainId { get; set; }
        public DateTime Date { get; set; }
        public double TripDistance { get; set; }
        public int ChainRotation { get; set; }
        public string? TripDescription { get; set; }
        public string? TripNotes { get; set; }

    }
}
