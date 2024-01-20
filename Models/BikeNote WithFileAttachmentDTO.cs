using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    //a model to store notes/discussions about bikes
    public class BikeNoteWithFileAttachmentDTO
    {
        public int Id { get; set; }
        public string _id { get; set; }
        public int BikeId { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        //public List<FileDetail> Files { get; set; } = new List<FileDetail>();
        public List<UploadResult> Files { get; set; } = new List<UploadResult>();
    }
}
