using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    //A model for getting all trips for a particular rotation of a chain
    public class ChainRotationTripsDTO
    {
        public int Id { get; set; }
        public int ChainId { get; set; }
        public int ChainRotation { get; set; }
        public decimal TotalDistance { get; set; }
        public List<TripDTO> Trips { get; set; }
    }
}
