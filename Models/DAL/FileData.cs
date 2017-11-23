using Neolab.Code.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public class FileData : BaseData
    {
        public FileData()
        {

        }

        public int FileID { get; set; }
        public int FileTypeID { get; set; }
        public int BetID { get; set; }
        public Guid BetGuid { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
    }
}