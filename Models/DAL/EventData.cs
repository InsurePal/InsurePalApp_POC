using Neolab.Code.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Models.DAL
{
    public class EventData : BaseData
    {
        public EventData()
        {

        }
        public EventData(Guid _betGuid, int _userID, int _activityID)
        {
            BetGuid = _betGuid;
            UserID = _userID;
            ActivityID = _activityID;
        }
        public int EventID { get; set; }
        public int BetID { get; set; }
        public Guid BetGuid { get; set; }
        public int UserID { get; set; }
        public string User { get; set; }
        public int ActivityID { get; set; }
        public string ActivityDescription { get; set; }
    }

    public static class EventDataExtension
    {
        public static void Add(this EventData data)
        {
            IEventDataRepository repo = new EventDataRepository();
            repo.Insert(data);
        }
    }
}