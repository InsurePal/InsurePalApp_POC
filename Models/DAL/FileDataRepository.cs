using Dapper;
using Neolab.Code.DAL;
using Neolab.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace SupportFriends.Models.DAL
{
    public class FileDataRepository : DbAccessObject, IFileDataRepository
    {
        public FileData Select(int id)
        {
            throw new NotImplementedException();
        }
        public List<FileData> SelectList()
        {
            throw new NotImplementedException();
        }
        public FileData Insert(FileData data)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("FileTypeID", data.FileTypeID);
                p.Add("BetGuid", data.BetGuid);
                p.Add("FilePath", data.FilePath);
                p.Add("FileExtension", data.FileExtension);
                p.Add("InsertUserID", WebSecurity.CurrentUserId);
                p.Add("InsertUsername", WebSecurity.CurrentUserName);

                this._db.Execute("spSUP_File_Insert", p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                NeoException.Handle(e);
            }

            return data;
        }
        public FileData Update(FileData data)
        {
            throw new NotImplementedException();
        }
        public void Delete(int eventID)
        {
            throw new NotImplementedException();
        }
    }
}