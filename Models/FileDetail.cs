﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFromGitToMongo.Models
{
    //A class to handle the details of a file to be stored in the database.
    //This can be used by any class. Only envisigoning storing receipts for the bike Part at start
    //but this could be expanded to store photos of trips, as an example
    //ClassOfRelatedEntity will store the class name of the related entity, Will do some trickery
    //to find the actual class name later
    public class FileDetail
    {
        public int id { get; set; }
        public string OriginalFileName { get; set; }        
        public string StoredFileName { get; set; }
        public string ServerPath { get; set; }
        public string MimeType { get; set; }
        public string ClassOfRelatedEntity { get; set; }        //This is the class name
        public int IdOfRelatedEntity{ get; set; }
     //   public object ClassFilesAttachedTo { get; set; }        //This is the actual object that the file is attached too.
    }
}
