
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    //a model to store notes/discussions about bikes
    public class BikeNote
    {
        public int Id { get; set; }
        public string _id { get; set; }
        public int BikeId { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        public List<UploadResult> UploadResult { get; set; } = new List<UploadResult>();
    }
}
