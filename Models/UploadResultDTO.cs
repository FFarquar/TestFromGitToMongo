using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    public class UploadResultDTO
    {
        public List<UploadResult> UploadResults { get; set; }
        public string relatedEntityName { get; set; }
        public int relatedEntityId { get; set; }
    }
}
