using Neolab.Code.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public interface IEventDataRepository : IDataRepository<EventData>
    {
        List<EventData> SelectListByvoucher(string voucherCode);
    }
}