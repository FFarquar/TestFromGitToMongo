using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    //a model to store bike parts in
    public class BikePartWithFileAttachmentsDTO
    {
        public int Id { get; set; }
        public string _id { get; set; }
        public int bikeId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime DatePurchased {get; set; }
        public string PurchasedFrom { get; set; }
        public string Notes { get; set; } = string.Empty;
        //public List<FileDetail> Files { get; set; } = new List<FileDetail>();
        public List<UploadResult> Files { get; set; } = new List<UploadResult>();
    }
}
