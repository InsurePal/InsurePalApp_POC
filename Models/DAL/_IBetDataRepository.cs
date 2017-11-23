using Neolab.Code.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public interface IBetDataRepository : IDataRepository<BetData>
    {
        BetData Find(Guid id, int actionID);
        List<BetData> SelectVouchersByCode(string voucherCode);
        //void VoucherCashIn(Guid betGUID, string betStatus);
        void VoucherUpdateStatus(Guid betGUID, int betStatusID);
        void VoucherUpdateStatusAndReference(Guid betGUID, int betStatusID, string voucherReference);
    }
}