using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public class ExceptionData
    {
        public ExceptionData ()
        {

        }

        public int ExceptionID { get; set; }
        public string HelpLink { get; set; }
        public string StoredProcedure { get; set; }
        public string InnerException { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string TargetSite { get; set; }
        public string CurrentUser { get; set; }
        public int UserID { get; set; }
        public string CurrentExecutionFilePath { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}