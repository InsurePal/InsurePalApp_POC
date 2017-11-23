using System;
using System.Web;
using SupportFriends.Models.DAL;
using Neolab.Code.DAL;

namespace Neolab
{
    namespace Common
    {
        /// <summary>
        /// Handles exceptions.
        /// Saves Exception data to database
        /// </summary>
        public static class NeoException
        {
            /// <summary>
            /// Handles the exception - writes it to database or file
            /// </summary>
            /// <param name="exc">The exception that was raised</param>
            public static void Handle(Exception exc)
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.RawUrl != null)
                    exc.HelpLink = HttpContext.Current.Request.RawUrl;

                IDataRepository<ExceptionData> _repository = new ExceptionDataRepository();
                ExceptionData excData = new ExceptionData();

                if(exc.HelpLink != null)
                    excData.HelpLink = (exc.HelpLink.ToString()).Replace("'", "''").ToString();
                if (exc.InnerException != null)
                    excData.InnerException = (exc.InnerException.ToString()).Replace("'", "''").ToString();
                if (exc.Message != null)
                    excData.Message = (exc.Message.ToString()).Replace("'", "''").ToString();
                if (exc.Source != null)
                    excData.Source = (exc.Source.ToString()).Replace("'", "''").ToString();
                if (exc.StackTrace != null)
                    excData.StackTrace = (exc.StackTrace.ToString()).Replace("'", "''").ToString();
                if (exc.TargetSite != null)
                    excData.TargetSite = (exc.TargetSite.ToString()).Replace("'", "''").ToString();
                excData.TimeStamp = DateTime.Now;

                _repository.Insert(excData);
            }
        }
    }
}