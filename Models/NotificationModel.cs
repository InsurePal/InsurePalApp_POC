using SupportFriends.Code.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Notifications
{
    public class NotificationModel
    {
        public NotificationModel(string _massage, NotificationType _type)
        {
            this.Message = _massage;
            this.Type = _type;
        }

        public string Message { get; set; }
        public NotificationType Type { get; set; }
    }
}