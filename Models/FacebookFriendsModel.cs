using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportFriends.Models
{
    public class FacebookFriendsModel
    {
        public List<FacebookFriend> friendsListing { get; set; }
    }

    public class FacebookFriend
    {
        public string name { get; set; }
        public string id { get; set; }
    }
}