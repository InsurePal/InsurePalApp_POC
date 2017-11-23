using Dapper;
using Neolab.Code.DAL;
using Neolab.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public class EventDataRepository : DbAccessObject, IEventDataRepository
    {
        public List<EventData> SelectListByvoucher(string voucherCode)
        {
            return this._db.Query<EventData>("spSUP_Event_SelectListByVoucher", param: new { VoucherCode = voucherCode }, commandType: CommandType.StoredProcedure).ToList();
        }
        public EventData Select(int id)
        {
            throw new NotImplementedException();
        }
        public List<EventData> SelectList()
        {
            throw new NotImplementedException();
        }
        public EventData Insert(EventData data)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("BetGuid", data.BetGuid);
                p.Add("UserID", data.UserID);
                p.Add("ActivityID", data.ActivityID);

                this._db.Execute("spSUP_Event_Insert", p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                NeoException.Handle(e);
            }

            return data;
        }
        public EventData Update(EventData data)
        {
            throw new NotImplementedException();
        }
        public void Delete(int eventID)
        {
            throw new NotImplementedException();
        }
    }
}