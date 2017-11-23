using Dapper;
using Neolab.Code.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public class ExceptionDataRepository : DbAccessObject, IDataRepository<ExceptionData>
    {
        public ExceptionData Select(int id)
        {
            //return this._db.Query<ExceptionData>("spAFW_Exception_Select", param: new { ExceptionID = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            throw new NotImplementedException();
        }

        public List<ExceptionData> SelectList()
        {
            //return this._db.Query<ExceptionData>("spAFW_Exception_SelectList", commandType: CommandType.StoredProcedure).ToList();
            throw new NotImplementedException();
        }

        public ExceptionData Insert(ExceptionData exception)
        {
            this._db.Execute("spAFW_Exception_Insert", param: exception, commandType: CommandType.StoredProcedure);
            return exception;
        }

        public ExceptionData Update(ExceptionData exception)
        {
            //this._db.Execute("spAFW_Exception_Update", param: exception, commandType: CommandType.StoredProcedure);
            //return exception;
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            //this._db.Execute("spAFW_Exception_Delete", param: new { ExceptionID = id }, commandType: CommandType.StoredProcedure);
            throw new NotImplementedException();
        }
    }
}