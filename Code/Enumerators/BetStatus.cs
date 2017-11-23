using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Code.Enumerators
{
    public enum BetStatus
    {
        NotDefined = -1,
        Invitation = 101,
    	Accepted = 102,
    	Rejected = 103,
    	Guaranteed = 110,
    	GuaranteeFailed = 111,
    	Voucher1Cashed = 112,
    	Voucher2Cashed = 113
    }
}